namespace EventSourcing.Domain

open System

module CmdArgs =
    type AddTask = {
        Id : int
        Name : string
        DueDate : DateTime option
    }

    type RemoveTask = {
        Id : int
    }

    type CompleteTask = {
        Id : int
    }

    type ChangeTaskDueDate = {
        Id : int
        DueDate : DateTime option
    }

type Comand = 
    | AddTask of CmdArgs.AddTask
    | RemoveTask of CmdArgs.RemoveTask
    | ClearAllTasks
    | CompleteTask of CmdArgs.CompleteTask
    | ChangeTaskDueDate of CmdArgs.ChangeTaskDueDate

type Event =
    | TaskAdded of CmdArgs.AddTask
    | TaskRemoved of CmdArgs.RemoveTask
    | AllTasksCleared
    | TaskCompleted of CmdArgs.CompleteTask
    | TaskDueDateChanged of CmdArgs.ChangeTaskDueDate

type Task = {
    Id : int
    Name : string
    DueDate : DateTime option
    IsComplete : bool
}

type State = {
    Tasks : Task list
}
    with 
        static member Init = {
            Tasks = []
        }