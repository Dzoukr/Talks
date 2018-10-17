// Learn more about F# at http://fsharp.org

open System
open Domain

// simple function composing command handler + event handlers together
let pipeline cmd =
    cmd
    |> CommandHandler.handle
    |> List.iter ReadSide.handle

let printState (desc:string) = CommandHandler.getCurrentState() |> printfn "[%s] %A" (desc.ToUpper())

[<EntryPoint>]
let main argv =

    printState "Initial"

    AddTask { Id = 2; Name = "Give cool talk"; DueDate = None } |> pipeline
    
    printState "After task added"

    CompleteTask { Id = 2 } |> pipeline
    
    printState "After task completed"

    ClearAllTasks  |> pipeline
    
    printState "After clear"
    0