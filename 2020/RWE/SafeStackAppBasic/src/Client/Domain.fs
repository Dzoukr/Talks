module Client.Domain

type Model = {
    Counter: int
    IsLoading : bool

}

type Msg =
    | Increment
    | Decrement
    | LoadFromServer
    | CountLoadedFromServer of int