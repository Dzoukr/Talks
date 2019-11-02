module Shared.Domain
open System

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ServerCountAPI = {
    GetRandomCount : unit -> Async<int>
}

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

type ServerColumnsAPI = {
    AddColumn : string -> Async<unit>
    RemoveColumn : string -> Async<unit>
    AddItemToColumn : string * string -> Async<unit>
    RemoveItemFromColumn : string * string -> Async<unit>
    CompleteItem : string * string -> Async<unit>
    GetAll : unit -> Async<Column list>
}
