﻿module Shared.Validation

open System
open Shared.Domain

let canAddColumn (allColumns:Column list) name =
    let sameColumnExists =
        allColumns
        |> List.exists (fun x -> x.Name = name)
    not (String.IsNullOrWhiteSpace name) && not sameColumnExists

let canAddItem (allItems:Item list) name =
    let sameItemExists =
        allItems
        |> List.exists (fun x -> x.Name = name)
    not (String.IsNullOrWhiteSpace name) && not sameItemExists

let canDeleteColumn (col:Column) = col.Items.Length = 0