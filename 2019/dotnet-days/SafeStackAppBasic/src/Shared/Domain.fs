module Shared.Domain

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ServerCountAPI = {
    GetRandomCount : unit -> Async<int>
}