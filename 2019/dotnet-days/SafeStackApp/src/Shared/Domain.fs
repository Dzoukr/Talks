module Shared.Domain
open System

type Counter = { Value : int }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ServerAPI =
    { initialCounter : unit -> Async<int> }

type ItemStatus =
    | New
    | Completed of DateTime

type Item = {
    Name : string
    Status : ItemStatus
}

type Column = {
    Name : string
    Items : Item list
}