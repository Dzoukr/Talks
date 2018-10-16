module ReadSide

open Domain

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