module Client.State

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

let init () : Model * Cmd<Msg> =
    { IsAddingNew = false; NewColumnName = ""; Columns = [] }, Cmd.none

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
    | AddItemToColumn (colName,item) ->
        let newItem = { Name = item }
        let colToReplace =
            currentModel.Columns
            |> List.find (fun x -> x.Name = colName)
            |> (fun col -> { col with Items = newItem :: col.Items })
        let newCols =
            currentModel.Columns
            |> List.filter (fun x -> x.Name <> colToReplace.Name)
            |> (fun cols -> colToReplace :: cols)
        { currentModel with Columns = newCols}, Cmd.none


