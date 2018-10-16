module EventSourcing.ReadSide
open EventSourcing

#load "03_commandhandler.fsx"

open EventSourcing
open EventSourcing.Domain

// create some fancy SQL command here
let fakeSqlInsert (args:CmdArgs.AddTask) = ()
let fakeSqlDelete (args:CmdArgs.RemoveTask) = ()

let handleEventToConsole = function
    | TaskAdded args -> printfn "Hurrayyyy, we have a task!"
    | AllTasksCleared -> printfn "...and now they are all gone"
    | _ -> ()

let handleEventToSql = function
    | TaskAdded args -> args |> fakeSqlInsert
    | TaskRemoved args -> args |> fakeSqlDelete
    | _ -> ()

let handleAll e =
    e |> handleEventToConsole 
    e |> handleEventToSql
    e

let handle = CommandHandler.handle >> List.map handleAll

AddTask { Id = 2; Name = "Give cool talk"; DueDate = None } |> handle
CommandHandler.getCurrentState()
CompleteTask { Id = 2 } |> handle
ClearAllTasks |> handle

CommandHandler.InMemoryES.load()