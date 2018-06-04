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

## What is Event Sourcing?

----------------------------------------------------------------------------

Event sourcing persists the **state** of **domain entity** as a **sequence** of state-changing **events**.

or

Storing all the **changes (events)** to the system, rather than just its current **state**.

----------------------------------------------------------------------------

### F# TL;DR

```fsharp

type EventSourcing = State -> Event -> State

```

****************************************************************************

## Why you should **not** use Event Sourcing?

----------------------------------------------------------------------------

### Increases SW **complexity**

### Not always **fit** for your system
(mostly apps constantly resetting state)

****************************************************************************

## Why you **should** use Event Sourcing?

----------------------------------------------------------------------------

### **Natural approach** for FP

**Immutable** (append only) storage

Each event leads to **new state**

You only need **fold** function

----------------------------------------------------------------------------

### You start **thinking different**

<img src="images/events.jpg" />

----------------------------------------------------------------------------

> "I want you to create application which will use React UI to make UPDATE command into table Users and set column IsActive to false."

said no customer **ever**

----------------------------------------------------------------------------

### Customers are **thinking in events**

**Deactivate** user

**Send** an email to supplier

**Publish** article

----------------------------------------------------------------------------

### Focus on **time** aspect 

**When** should this happen?

What if this happen **before** that?

----------------------------------------------------------------------------

### Focus on **behavior**

**What** instead of how

a.k.a.

**Declarative** over imperative

----------------------------------------------------------------------------

### **Thinking in events** is mindset for **all industries**

----------------------------------------------------------------------------

### Event sourcing is **easy to test**


```fsharp

[<Test>]
let ``Cannot withdraw from blocked account`` () =
    let state = { Amount = 500; IsBlocked = true }
    Withdraw 100 |> execute state |> Assert.isError
```

----------------------------------------------------------------------------

### Event sourcing is **SAFE**

----------------------------------------------------------------------------

You can **replay** all events

You can **prove system state** at any point in **history**

You have **full audit log** elevated to single source of truth

****************************************************************************

## How to start

----------------------------------------------------------------------------

### You will need **Commands**

```fsharp
type Comand = 
    | AddTask of CmdArgs.AddTask
    | RemoveTask of CmdArgs.RemoveTask
    | ClearAllTasks
    | CompleteTask of CmdArgs.CompleteTask
    | ChangeTaskDueDate of CmdArgs.ChangeTaskDueDate
```

Something you want your system to execute

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

Things that happend based on your commands

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

----------------------------------------------------------------------------

### and something called **Aggregate**

**Init** - default / empty state

**Execute** - function converting command to list of events

**Apply** - applies single event on state to create new state

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

Never throwns errors

----------------------------------------------------------------------------

### Aggregate

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



****************************************************************************



****************************************************************************




# Thank you!

## Feel free to ask

<br/><br/><br/><br/><br/>
### Roman Provazník

[@rprovaznik](https://twitter.com/rprovaznik) | [@fsharping](https://twitter.com/fsharping) | [fsharping.com](https://fsharping.com)

