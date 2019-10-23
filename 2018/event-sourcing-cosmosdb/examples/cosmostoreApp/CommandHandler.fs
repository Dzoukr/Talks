module CommandHandler

open System
open Domain
open Aggregate
open CosmoStore

module Mapping = 
    open Newtonsoft.Json.Linq

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

type DemoStore = {
    GetCurrentState : unit -> State
    Append : Event list -> unit
}

type StorageType =
    | CosmosDb of CosmoStore.CosmosDb.Configuration
    | TableStorage of CosmoStore.TableStorage.Configuration

let createDemoStore typ =

    let store = 
        match typ with
        | CosmosDb cfg -> cfg |> CosmoStore.CosmosDb.EventStore.getEventStore
        | TableStorage cfg -> cfg |> CosmoStore.TableStorage.EventStore.getEventStore
    
    let getCurrentState () =
        store.GetEvents "Tasks" EventsReadRange.AllEvents
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> List.map (fun x -> Mapping.toDomainEvent (x.Name, x.Data))
        |> List.fold aggregate.Apply State.Init

    let append evns =
        evns 
        |> List.map Mapping.toStoredEvent
        |> List.map (fun (name,data) -> { Id = Guid.NewGuid(); CorrelationId = Guid.NewGuid(); Name = name; Data = data; Metadata = None })
        |> store.AppendEvents "Tasks" ExpectedPosition.Any
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore 

    {
        GetCurrentState = getCurrentState
        Append = append
    }


let validate cmd =
    match cmd with
    | AddTask args -> if args.Name.Length = 0 then failwith "Gimme some name!" else cmd
    | ChangeTaskDueDate args -> if args.DueDate.IsSome && args.DueDate.Value < DateTime.Now then failwith "Are you Marty McFly?!" else cmd
    | _ -> cmd

let handleCommand (store:DemoStore) command = 
    // get the latest state from store
    let currentState = store.GetCurrentState()
    // execute command to get new events
    let newEvents = command |> aggregate.Execute currentState
    // store events to event store
    newEvents |> store.Append
    // return events
    newEvents

let handle store cmd = 
    cmd 
    |> validate 
    |> handleCommand store