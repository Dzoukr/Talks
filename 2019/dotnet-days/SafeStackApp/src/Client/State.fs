module Client.State

open System
open Domain
open Shared.Domain
open Elmish

//let init () : Model * Cmd<Msg> =
//    //{ Counter = -1; IsLoading = false }, Cmd.ofMsg LoadFromServer
//
//let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
//    match msg with
//    | Increment ->
//        let nextModel = { currentModel with Counter = currentModel.Counter + 1  }
//        nextModel, Cmd.none
//    | Decrement ->
//        let nextModel = { currentModel with Counter = currentModel.Counter - 1 }
//        nextModel, Cmd.none
//    | LoadFromServer ->
//        { currentModel with IsLoading = true }, Cmd.OfAsync.perform Server.api.initialCounter () CountLoadedFromServer
//    | CountLoadedFromServer initialCount->
//        let nextModel = { currentModel with Counter = initialCount; IsLoading = false }
//        nextModel, Cmd.none

module Data =
    let replaceColumn cols newCol =
        let others =
            cols |> List.filter (fun x -> x.Name <> newCol.Name)
        newCol :: others
    let replaceItem cols colName (item:Item) =
        let col = cols |> List.find (fun x -> x.Name = colName)
        let others =
            col.Items |> List.filter (fun x -> x.Name <> item.Name)
        { col with Items = item :: others } |> replaceColumn cols
    let removeItem cols colName itemName =
        let col = cols |> List.find (fun x -> x.Name = colName)
        let others =
            col.Items |> List.filter (fun x -> x.Name <> itemName)
        { col with Items = others } |> replaceColumn cols
    let addItem cols colName (item:Item) =
        let col = cols |> List.find (fun x -> x.Name = colName)
        { col with Items = item :: col.Items } |> replaceColumn cols
    let getItem cols colName itemName =
        let col = cols |> List.find (fun x -> x.Name = colName)
        col.Items |> List.find (fun x -> x.Name = itemName)

let init () : Model * Cmd<Msg> =
    { IsAddingNew = false; NewColumnName = ""; Columns = []; NewItemNames = [] }, Cmd.none

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | StartEditing -> { currentModel with IsAddingNew = true }, Cmd.none
    | StopEditing -> { currentModel with IsAddingNew = false }, Cmd.none
    | TextEdited v -> { currentModel with NewColumnName = v }, Cmd.none
    | AddNewColumn ->
        let newCol = { Name = currentModel.NewColumnName; Items = [] }
        { currentModel with Columns = newCol :: currentModel.Columns; IsAddingNew = false; NewColumnName = "" }, Cmd.none
    | RemoveColumn name ->
        let newCols = currentModel.Columns |> List.filter (fun x -> x.Name <> name)
        { currentModel with Columns = newCols }, Cmd.none
    | ItemTextEdited (col,v) ->
        let texts = currentModel.NewItemNames |> List.filter (fun (c,_) -> c <> col )
        { currentModel with NewItemNames = (col,v) :: texts }, Cmd.none
    | AddItemToColumn colName ->
        let found,others = currentModel.NewItemNames |> List.partition (fun (c,_) -> c = colName)
        let newCols =
            { Name = found |> List.head |> snd; Status = New }
            |> Data.addItem currentModel.Columns colName
        { currentModel with Columns = newCols; NewItemNames = others }, Cmd.none
    | RemoveItemFromColumn (colName,item) ->
        let newCols = Data.removeItem currentModel.Columns colName item
        { currentModel with Columns = newCols }, Cmd.none
    | CompleteItem (colName,item) ->
        let foundItem = Data.getItem currentModel.Columns colName item
        let newCols =
            { foundItem with Status = Completed(DateTime.UtcNow) }
            |> Data.replaceItem currentModel.Columns colName
        { currentModel with Columns = newCols }, Cmd.none