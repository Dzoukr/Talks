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
    NewItemNames : (string * string) list
    Columns : Column list
}

type Msg =
    | StartEditing
    | CancelEditing
    | TextEdited of string
    | AddNewColumn
    | RemoveColumn of string
    | ItemTextEdited of columnName:string * itemName:string
    | AddItemToColumn of columnName:string
    | RemoveItemFromColumn of columnName:string * itemName:string
    | CompleteItem of columnName:string * itemName:string
    | ReloadColumnsFromServer
    | ColumnsReloadedFromServer of Column list
    | ErrorOccured of exn