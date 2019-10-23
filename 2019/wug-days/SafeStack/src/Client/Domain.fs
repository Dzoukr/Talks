module Client.Domain

type Model = {
    Count : int
}

type Msg =
    | Increment
    | Decrement
    | Reset
