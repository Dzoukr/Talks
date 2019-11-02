module Client.State

open Domain
open Shared.Domain
open Elmish

let init () : Model * Cmd<Msg> =
    { Counter = -1; IsLoading = false }, Cmd.ofMsg LoadFromServer

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | Increment ->
        let nextModel = { currentModel with Counter = currentModel.Counter + 1  }
        nextModel, Cmd.none
    | Decrement ->
        let nextModel = { currentModel with Counter = currentModel.Counter - 1 }
        nextModel, Cmd.none
    | LoadFromServer ->
        { currentModel with IsLoading = true }, Cmd.OfAsync.perform Server.countAPI.GetRandomCount () CountLoadedFromServer
    | CountLoadedFromServer initialCount->
        let nextModel = { currentModel with Counter = initialCount; IsLoading = false }
        nextModel, Cmd.none