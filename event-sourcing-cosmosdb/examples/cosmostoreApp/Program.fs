// Learn more about F# at http://fsharp.org

open System
open Domain

[<EntryPoint>]
let main argv =

    AddTask { Id = 2; Name = "Give cool talk"; DueDate = None } |> ReadSide.handle |> ignore
    CompleteTask { Id = 2 } |> ReadSide.handle |> ignore
    let stateBeforeClear = CommandHandler.getCurrentState()

    ClearAllTasks  |> ReadSide.handle |> ignore

    let stateAfterClear = CommandHandler.getCurrentState()
    0