﻿namespace CarParking.WebApi

open CarParking.Core
open CarParking.Utils
open System

module Responses =
    [<CLIMutable; NoEquality; NoComparison>]
    type PaymentResponse =
        { Id: Guid 
          CreateDate: DateTime }
        static member FromPayment(x: Payment) =
            { Id = PaymentId.toGuid x.Id
              CreateDate = x.CreateDate }
        static member Null = 
            Unchecked.defaultof<PaymentResponse>

    [<CLIMutable; NoEquality; NoComparison>]
    type ParkingResponse =
        { Id: Guid
          Type: string
          ArrivalDate: DateTime
          CompleteDate: Nullable<DateTime>
          Payment: PaymentResponse }
        static member FromParking(x: Parking) = 
            let parkingType = ClientConstants.Parking.ofParking x
            match x with
            | StartedFree prk ->
                { Id           = ParkingId.toGuid prk.Id
                  Type         = parkingType
                  ArrivalDate  = prk.ArrivalDate
                  CompleteDate = Nullable()
                  Payment      = PaymentResponse.Null }
            | CompletedFree prk ->
                let (arrivalDate, completeDate) = ParkingInterval.getDates prk.Interval
                { Id           = ParkingId.toGuid prk.Id
                  Type         = parkingType
                  ArrivalDate  = arrivalDate
                  CompleteDate = Nullable(completeDate)
                  Payment      = PaymentResponse.Null }
            | CompletedFirst prk ->
                let (arrivalDate, completeDate) = ParkingInterval.getDates prk.Interval
                { Id           = ParkingId.toGuid prk.Id
                  Type         = parkingType
                  ArrivalDate  = arrivalDate
                  CompleteDate = Nullable(completeDate)
                  Payment      = PaymentResponse.FromPayment prk.Payment }
    
    [<CLIMutable; NoEquality; NoComparison>]
    type ParkingResponseModel =
        { Parking: ParkingResponse }
        static member FromResponse x = { Parking = x }

    [<CLIMutable; NoEquality; NoComparison>]
    type ParkingsResponseModel =
        { Parkings: ParkingResponse list }
        static member FromResponse x = { Parkings = x }

    [<CLIMutable; NoEquality; NoComparison>]
    type TransitionErrorReponse = 
        { ErrorType: string }

    [<CLIMutable; NoEquality; NoComparison>]
    type TransitionResponse =
        { Name: string
          FromTariff: string 
          FromStatus: string 
          ToTariff: string
          ToStatus: string }
        static member FromTransition (transition: Transition) =
            { Name = transition.Name
              FromTariff = transition.FromTariff.MapOrDefault Tariff.toString
              FromStatus = transition.FromStatus.MapOrDefault ParkingStatus.toString
              ToTariff = transition.ToTariff |> Tariff.toString
              ToStatus = transition.ToStatus |> ParkingStatus.toString }

    [<CLIMutable; NoEquality; NoComparison>]
    type TransitionReponseModel =
        { Transitions: TransitionResponse seq }
        static member FromResponse x = { Transitions = x }