module EventSourcing.CommandHandler
#load "02_aggregate.fsx"
//#r "../packages/CosmoStore/lib/netstandard2.0/CosmoStore.dll"
//#r "../packages/Newtonsoft.Json/lib/netstandard2.0/Newtonsoft.Json.dll"
//#r "../packages/NETStandard.Library/build/netstandard2.0/ref/netstandard.dll"

open System
open EventSourcing.Domain
open EventSourcing.Aggregate

module InMemoryES =
    let private journal = ResizeArray<Event>()
    let store evns = evns |> List.iter journal.Add
    let load () = journal |> Seq.toList

// module CosmosDBStore = 
//     open Newtonsoft.Json.Linq
//     open CosmoStore

//     let toStoredEvent evn = 
//         match evn with
//         | TaskAdded args -> "TaskAdded", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
//         | TaskRemoved args -> "TaskRemoved", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
//         | AllTasksCleared -> "AllTasksCleared", (Newtonsoft.Json.Linq.JValue.CreateNull() :> JToken)
//         | TaskCompleted args -> "TaskCompleted", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
//         | TaskDueDateChanged args -> "TaskDueDateChanged", args |> CosmoStore.CosmosDb.Serialization.objectToJToken


//     let private configuration = CosmosDb.Configuration.CreateDefault (System.Uri "https://localhost:8081") "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
//     let private client = CosmosDb.EventStore.getEventStore configuration
//     let store evns =
//         evns 
//         |> List.map toStoredEvent
//         |> List.map (fun (name,data) -> { Id = Guid.NewGuid(); CorrelationId = Guid.NewGuid(); Name = name; Data = data; Metadata = None })
//         |> client.AppendEvents "Tasks" ExpectedPosition.Any
//         |> Async.AwaitTask
//         |> Async.RunSynchronously




let getCurrentState () = InMemoryES.load() |> List.fold aggregate.Apply State.Init

let validate cmd =
    match cmd with
    | AddTask args -> if args.Name.Length = 0 then failwith "Gimme some name!" else cmd
    | ChangeTaskDueDate args -> if args.DueDate.IsSome && args.DueDate.Value < DateTime.Now then failwith "Are you Marty McFly?!" else cmd
    | _ -> cmd

let handleCommand command = 
    // get the latest state from store
    let currentState = getCurrentState()
    // execute command to get new events
    let newEvents = command |> aggregate.Execute currentState
    // store events to event store
    newEvents |> InMemoryES.store
    // return events
    newEvents

let handle = validate >> handleCommand

AddTask { Id = 2; Name = "Give cool talk"; DueDate = None } |> handle
CompleteTask { Id = 2 } |> handle
ClearAllTasks |> handle

getCurrentState ()