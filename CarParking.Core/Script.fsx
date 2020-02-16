﻿#load "Domain.fs"

open System
open CarParking.Core
open CarParking.Core.Parking

let s =
    { Id = ParkingId (Guid.NewGuid())
      ArrivalDate = DateTime.UtcNow.AddMinutes(-20.) }
let d = DateTime.UtcNow

calculateTariff s d

// Complete
match transitionToCompletedFree s d with
| Ok prk ->
    printfn "Save %A to DB" prk
| Error x ->
    printfn "%A" x

let pId = PaymentId (Guid.NewGuid())

// Pay
match transitionToCompletedFirst s (d, pId) with
| Ok prk ->
    printfn "%A" prk
| Error x ->
    printfn "%A" x