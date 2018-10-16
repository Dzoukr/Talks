- title : Event Sourcing with F# and Azure Cosmos DB
- description : Event Sourcing with F# and Azure Cosmos DB
- author : Roman Provazník
- theme : night
- transition : none

****************************************************************************

# Event Sourcing

## with **F#** and Azure **Cosmos DB**

<br/><br/><br/><br/><br/>
### Roman Provazník

[@rprovaznik](https://twitter.com/rprovaznik) | [@fsharping](https://twitter.com/fsharping) | [fsharping.com](https://fsharping.com)

****************************************************************************

## Hello

<table><tr><td class="table-leftcol">

**CN Group** F# Lead Developer

**FSharping** founder

**Terrible** drummer


</td><td class="table-rightcol">
<img width="250" src="images/avatar.jpg" />
</td></tr></table>


****************************************************************************

## **Event Sourcing** with **F#** and Azure **Cosmos DB** 
### (why, why not and how)

****************************************************************************

## What is **Event**?

----------------------------------------------------------------------------

### Fact that **happened in past** and **cannot** be changed

----------------------------------------------------------------------------

*Things happended and we can only react on them, but cannot affect them anymore*

<br/>

Email **sent**

Article **published**

File **deleted** 

Temperature **decreased** 

Money **withdrawn**

Anniversary **forgotten**

<br/>


*Usually created based on commands or other events*

****************************************************************************

## What is **Event Sourcing**?

----------------------------------------------------------------------------

### Storing all the **changes (events)** to the system, rather than just its current **state**.

----------------------------------------------------------------------------

### F# TL;DR

```fsharp

type EventSourcing = State -> Event -> State

```

****************************************************************************

## Why you should **not** use Event Sourcing?

----------------------------------------------------------------------------

Increases SW **complexity**

Not always **fit** for your system

Has own **specific set of issues** (versioning, splitting domain, streams, ...)

----------------------------------------------------------------------------

### Event Sourcing is **not a silver bullet**

<br/>

<img width="590" src="images/bullet.gif" />

<br/>

*sorry :-(*

****************************************************************************

## Why you **should** use Event Sourcing?

----------------------------------------------------------------------------

### **Natural approach** for FP

<br/>

Usage of append only (**immutable**) storage

Working with **immutable** structures (events)

You only need **fold** function

----------------------------------------------------------------------------

### You start **thinking different**

<img src="images/events.jpg" />

----------------------------------------------------------------------------

> "I want you to create application which will use React UI to make UPDATE command into table Users and set column IsActive to false."

said no customer **ever**

----------------------------------------------------------------------------

### Customers are **thinking in events**

<br/>

User to be **deactivated**

Email to be **sent** to supplier

Article to be **published**

----------------------------------------------------------------------------

### Focusing on events creates **technology agnostic communication** with customer

----------------------------------------------------------------------------

### Focus on **time** aspect 

<br/>

**When** should this event happen?

What if this event happen **before** that?

How should system react **after** some event happens?

----------------------------------------------------------------------------

### **Thinking in events** is mindset<br/> for **all industries**

----------------------------------------------------------------------------

### Event sourcing is **easy to test**


```fsharp

[<Test>]
let ``Cannot withdraw from blocked account`` () =
    let stateBeforeEvent = { Amount = 500; IsBlocked = true }
    Withdraw 100 |> execute stateBeforeEvent |> Assert.isError
```

----------------------------------------------------------------------------

### Event sourcing is **safe**

----------------------------------------------------------------------------

You can **replay** all events to create new projections

You can **prove system state** at any point in **history**

You have **full audit log** elevated to single source of truth

----------------------------------------------------------------------------

### But remember - **not a silver bullet!**

<img src="images/es-joke.jpg"/>


****************************************************************************

## How to start

----------------------------------------------------------------------------

### !!! Warning !!!

<br/>

Highly **opinionated** approach (one of hundreds)

Mixing **CQRS & ES** terms

----------------------------------------------------------------------------

### You will need **Commands**

```fsharp
type Command = 
    | AddTask of CmdArgs.AddTask
    | RemoveTask of CmdArgs.RemoveTask
    | ClearAllTasks
    | CompleteTask of CmdArgs.CompleteTask
    | ChangeTaskDueDate of CmdArgs.ChangeTaskDueDate
```

Something you want your system to **execute**

----------------------------------------------------------------------------

### and some **Events**

```fsharp
type Event =
    | TaskAdded of CmdArgs.AddTask
    | TaskRemoved of CmdArgs.RemoveTask
    | AllTasksCleared
    | TaskCompleted of CmdArgs.CompleteTask
    | TaskDueDateChanged of CmdArgs.ChangeTaskDueDate
```

Things that happend **based on your commands**

----------------------------------------------------------------------------

### and your **Domain (State)**

```fsharp
type Task = {
    Id : int
    Name : string
    DueDate : DateTime option
    IsComplete : bool
}

type State = {
    Tasks : Task list
}
```

*Used for validation of commands and internal state representation*

----------------------------------------------------------------------------

### and something called **Aggregate**

**Init** - default / empty state

**Execute** - function converting command to list of events

**Apply** - function for applying single event on state to create new state

----------------------------------------------------------------------------

### Init

```fsharp
type State = {
    Tasks : Task list
}
    with 
        static member Init = {
            Tasks = []
        }

```

----------------------------------------------------------------------------

### Execute

```fsharp
let execute state = function
    | AddTask args -> 
        args.Id 
        |> onlyIfTaskDoesNotAlreadyExist state 
        |> (fun _ -> TaskAdded args)
```

Can throw error or return events in Result

Validates command against internal state

----------------------------------------------------------------------------

### Apply

```fsharp
let apply state = function
    | TaskAdded args -> 
        let newTask = { 
            Id = args.Id
            Name = args.Name
            DueDate = args.DueDate
            IsComplete = false
        }
        { state with Tasks = newTask :: state.Tasks}
```

Never throwns errors - only **applies** event on current state

----------------------------------------------------------------------------

### Aggregate (as simple F# record)

```fsharp
type Aggregate<'state, 'command, 'event> = {
    Init : 'state 
    Execute: 'state -> 'command -> 'event list
    Apply: 'state -> 'event -> 'state
}

```

****************************************************************************

## Event Store

----------------------------------------------------------------------------

**Append-only** database

Data (events) stored in **Streams**

Events stored as they appeared (**chronologically**)

----------------------------------------------------------------------------

## Event Store Solutions

<table><tr><td class="table-leftcol">

**EventStore.org** - .NET API, HTTP API, Projections

**NEventStore.org** - Persistence agnostic Event Store for .NET

Custom implementation - MongoDB, PostgreSQL, **Cosmos DB**, MSSQL


</td><td class="table-rightcol">
<img src="images/eventstore.png" />
</td></tr></table>

****************************************************************************

## Cosmos DB

<table><tr><td class="table-leftcol">

**Globally distributed** DB on Azure

Multiple **APIs** - Document DB, Graph DB, Key-value storage

Really **fast**

**Terrible** pricing (pay for collection)

</td><td class="table-rightcol">
<img width="250" src="images/cosmosdb_logo.png" />
</td></tr></table>

----------------------------------------------------------------------------

<img src="images/cosmosdb.png" />

Globally distributed

----------------------------------------------------------------------------

<img width="800" src="images/cosmosdb1.png" />

Built-in JSON editor

----------------------------------------------------------------------------

<img width="650" src="images/cosmosdb-emulator.png" />

Free emulator available

----------------------------------------------------------------------------

## Why Cosmos DB?

**Azure** was a must

We did not care about pricing - need only 1 collections (Events)

Has support for **stored procedures**, **triggers** and user defined **functions**

Wanted to write it by ourselves - we are developers, right? :-)

----------------------------------------------------------------------------

## So we wrote own Event Store

```fsharp
type EventStore = {
    AppendEvent : 
        string -> ExpectedPosition -> EventWrite -> Task<EventRead>
    AppendEvents : 
        string -> ExpectedPosition -> EventWrite list -> Task<EventRead list>
    GetEvent : string -> int64 -> Task<EventRead>
    GetEvents : string -> EventsReadRange -> Task<EventRead list>
    GetStreams : StreamsReadFilter -> Task<Stream list>
}
```

Available as **CosmoStore** Nuget (also on Github)

**Only 289** LoC + 86 LoC for Stored Procedure

**Cosmos DB** support (Azure Table Storage planned for version 1.2)

----------------------------------------------------------------------------

## Lessons learned

<table><tr><td class="table-leftcol">

Use **Stored procedure** (with transaction support) to append events

Use **Unique keys** for Optimistic concurrency control

</td><td width="350" class="table-rightcol">
<img src="images/cosmosdb2.png" />
</td></tr></table>

----------------------------------------------------------------------------

## Lessons learned

### **Use already existing solution, if you can.** 

*It is fun to write, but pain to maintain.*

****************************************************************************

## Where are we?

We have **Commands & Events**

We have **Domain**

We have **Event Store**

We have **C** from **CQRS** complete

****************************************************************************

## Now it is time to prepare **Read (Query) side**

----------------------------------------------------------------------------

## Read side

Designed specially for one purpose - **provide data**

Can be **common SQL** database (with indexes, ...)

Filled based on **events**

----------------------------------------------------------------------------

## Read side

```fsharp

let handleEventToSql = function
    | TaskAdded args -> args |> makeSqlInsert
    | AllTasksCleared -> deleteAll()

```

----------------------------------------------------------------------------

## Read side - nice parts

You can **design it** based on read needs

You can **change it** anytime and replay events

You can **fill it** on different thread

Read DB failure **does not affect** your domain logic

----------------------------------------------------------------------------

## Read side - ugly parts

Still  **eventually consistent**


****************************************************************************

## **DEMO**

****************************************************************************

## Battlefield hints

----------------------------------------------------------------------------

## #1
## Never store **State in Read DB**

*React on events, not on state*

----------------------------------------------------------------------------

## #2
## **Forget** about Fire-and-Forget

*At least return success of Execute function*

----------------------------------------------------------------------------

## #3
## **Never** use **IFs** in Apply

*That is why we return Event list*

----------------------------------------------------------------------------

## #4
## **REST** is a lie

*POST for commands, GET for queries*

----------------------------------------------------------------------------

## #5
## **No tuples** in Commands or Events arguments

*Think about serialization to Event Store*

----------------------------------------------------------------------------

## #6
## **Think twice** before you use read side for command validation

*Think about eventual consistency*

----------------------------------------------------------------------------

## #7
## **Choose wisely** event arguments

*You will live with your decision longer than you think*

****************************************************************************

<img src="images/event.jpg"/>

****************************************************************************

# Thank you!

## Feel free to ask

<br/><br/><br/><br/><br/>
### Roman Provazník

[@rprovaznik](https://twitter.com/rprovaznik) | [@fsharping](https://twitter.com/fsharping) | [fsharping.com](https://fsharping.com)

