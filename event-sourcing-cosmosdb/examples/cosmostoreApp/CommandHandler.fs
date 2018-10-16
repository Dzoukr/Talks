module CommandHandler

open System
open Domain
open Aggregate

module InMemoryES =
    let private journal = ResizeArray<Event>()
    let store evns = evns |> List.iter journal.Add
    let load () = journal |> Seq.toList

module CosmosDBStore = 
    open Newtonsoft.Json.Linq
    open CosmoStore

    let toStoredEvent evn = 
        match evn with
        | TaskAdded args -> "TaskAdded", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | TaskRemoved args -> "TaskRemoved", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | AllTasksCleared -> "AllTasksCleared", (Newtonsoft.Json.Linq.JValue.CreateNull() :> JToken)
        | TaskCompleted args -> "TaskCompleted", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | TaskDueDateChanged args -> "TaskDueDateChanged", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
    
    let toDomainEvent data =
        match data with
        | "TaskAdded", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskAdded 
        | "TaskRemoved", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskRemoved 
        | "AllTasksCleared", _ -> AllTasksCleared 
        | "TaskCompleted", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskCompleted 
        | "TaskDueDateChanged", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskDueDateChanged 


    let private configuration = CosmosDb.Configuration.CreateDefault (System.Uri "https://localhost:8081") "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
    let private client = CosmosDb.EventStore.getEventStore configuration
    let store evns =
        evns 
        |> List.map toStoredEvent
        |> List.map (fun (name,data) -> { Id = Guid.NewGuid(); CorrelationId = Guid.NewGuid(); Name = name; Data = data; Metadata = None })
        |> client.AppendEvents "Tasks" ExpectedPosition.Any
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore
     
    let load() =
        client.GetEvents "Tasks" EventsReadRange.AllEvents
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> List.map (fun x -> toDomainEvent (x.Name, x.Data))

//let getCurrentState () = InMemoryES.load() |> List.fold aggregate.Apply State.Init
let getCurrentState () = CosmosDBStore.load() |> List.fold aggregate.Apply State.Init

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
    newEvents |> CosmosDBStore.store
    // return events
    newEvents

let handle = validate >> handleCommand