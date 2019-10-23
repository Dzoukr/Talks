module Client.Update

open Domain
open Elmish

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | Increment -> { currentModel with Count = currentModel.Count + 1 }, Cmd.none
    | Decrement -> { currentModel with Count = currentModel.Count - 1 }, Cmd.none
    | Reset -> { currentModel with Count = 0 }, Cmd.none
