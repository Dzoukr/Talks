module Shared.Domain

type Counter = { Value : int }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ServerAPI =
    { initialCounter : unit -> Async<int> }

type Item = {
    Name : string
}

type Column = {
    Name : string
    Items : Item list
}