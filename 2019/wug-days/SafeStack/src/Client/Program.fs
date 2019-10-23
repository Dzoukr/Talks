module Client.Program

open Elmish
open Elmish.React
open Domain

let init () : Model * Cmd<Msg> =
    let initialModel = { Count = 0 }
    initialModel, Cmd.none//Cmd.ofMsg Msg.LoadRandomCount

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init Update.update View.view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run