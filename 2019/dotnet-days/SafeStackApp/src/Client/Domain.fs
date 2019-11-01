module Client.Domain

open Shared

type Model = {
    Counter: int
    IsLoading : bool
}

type Msg =
    | Increment
    | Decrement
    | LoadFromServer
    | CountLoadedFromServer of int