module Server.ColumnsManager

open Shared.Domain

let private storage = ResizeArray<Column>()

let getAll () = storage |> Seq.toList

let addColumn name =
    let allCols = getAll()
    if Shared.Validation.canAddColumn allCols name then
        storage.Add({ Name = name; Items = [] })
    else failwith "Sorry, this column cannot be added"

let removeColumn name =
    let toRemove = getAll() |> List.find (fun x -> x.Name = name)
    if Shared.Validation.canDeleteColumn toRemove then
        storage.Remove(toRemove) |> ignore
    else failwith "Sorry, this column cannot be removed"

let addItemToColumn columnName itemName =
    let toReplace = getAll() |> List.find (fun x -> x.Name = columnName)
    if Shared.Validation.canAddItem toReplace.Items itemName then
        storage.Remove(toReplace) |> ignore
        let item = { Name = itemName; Status = New }
        storage.Add({ toReplace with Items = item :: toReplace.Items})
    else failwith "Sorry, this item cannot be added"