module EventSourcing.CommandHandler
#load "02_aggregate.fsx"

open System
open EventSourcing.Domain
open EventSourcing.Aggregate

module InMemoryES =
    let private journal = ResizeArray<Event>()
    let store evns = evns |> List.iter journal.Add
    let load () = journal |> Seq.toList

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