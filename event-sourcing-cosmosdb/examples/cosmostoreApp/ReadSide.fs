module ReadSide

open Domain

// create some fancy SQL command here
let fakeSqlInsert (args:CmdArgs.AddTask) = 
    printfn "SQL handler says: INSERT INTO Tasks VALUES (%i, '%s', false)" args.Id args.Name
    ()

let fakeSqlUpdate (args:CmdArgs.CompleteTask) = 
    printfn "SQL handler says: UPDATE Tasks SET IsCompleted = true WHERE ID = %i" args.Id
    ()

let fakeSqlDelete () = 
    printfn "SQL handler says: DELETE FROM Tasks"
    ()

let handleEventToConsole = function
    | TaskAdded args -> printfn "Console handler says: Hurrayyyy, we have a task %s!" args.Name
    | TaskCompleted args -> printfn "Console handler says: Task with ID %A is completed" args.Id
    | AllTasksCleared -> printfn "Console handler says: ...and now they are all gone"
    | _ -> ()

let handleEventToSql = function
    | TaskAdded args -> args |> fakeSqlInsert
    | TaskCompleted args -> args |> fakeSqlUpdate
    | AllTasksCleared -> fakeSqlDelete()
    | _ -> ()

let handle evn =
    evn |> handleEventToConsole
    evn |> handleEventToSql