module Client.Domain

open Shared.Domain

//type Model = {
//    Counter: int
//    IsLoading : bool
//
//}
//
//type Msg =
//    | Increment
//    | Decrement
//    | LoadFromServer
//    | CountLoadedFromServer of int

type Model = {
    IsAddingNew : bool
    NewColumnName : string
    Columns : Column list
}

type Msg =
    | StartEditing
    | StopEditing
    | TextEdited of string
    | AddNewColumn
    | RemoveColumn of string
    | AddItemToColumn of column:string * item:string