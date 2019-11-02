module Client.View

open Domain
open Fable.React
open Feliz
open Shared.Domain

module Helpers =

    let navbarTitle (str:string) =
        Html.nav [
            prop.className "navbar is-dark"
            prop.children [
                Html.div [
                    prop.className "navbar-start"
                    prop.children [
                        Html.div [
                            prop.className "navbar-item"
                            prop.children [
                                Html.h2 [
                                    prop.text str
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]

    let section (childs:seq<ReactElement>) = Html.section [ prop.className "section"; prop.children childs ]
    let container (childs:seq<ReactElement>) = Html.div [ prop.className "container"; prop.children childs ]
    let columns (childs:seq<ReactElement>) = Html.div [ prop.className "columns"; prop.children childs ]
    let column (childs:seq<ReactElement>) = Html.div [ prop.className "column"; prop.children childs ]
    let icon (className:string) = Html.span [ prop.className "icon"; prop.children [ Html.i [ prop.className className ] ] ]

    let inTemplate controls =
        Html.div [
            navbarTitle "Stafe Stack App"
            section [
                container [
                    columns controls
                ]
            ]
        ]

let itemToView dispatch col (i:Item) =
    let styles =
        match i.Status with
        | New -> [ style.marginLeft 10 ]
        | Completed _ -> [ style.marginLeft 10; style.textDecoration.lineThrough; style.fontStyle.italic ]
    let text =
        match i.Status with
        | New -> i.Name
        | Completed date -> sprintf "%s - completed on %s" i.Name (date.ToShortDateString())


    Html.li [
        Html.div [
            prop.className "box"
            prop.children [
                Html.button [
                    prop.className "button is-danger is-small"
                    prop.children [
                        Helpers.icon "fas fa-minus"
                    ]
                    prop.onClick (fun _ -> RemoveItemFromColumn(col.Name, i.Name) |> dispatch)
                ]
                Html.button [
                    prop.className "button is-success is-small"
                    prop.children [
                        Helpers.icon "fas fa-check"
                    ]
                    prop.style [
                        style.marginLeft 5
                    ]
                    prop.onClick (fun _ -> CompleteItem(col.Name, i.Name) |> dispatch)
                    prop.disabled (Shared.Validation.canCompleteItem col.Items i.Name |> not)
                ]
                Html.span [
                    prop.text text
                    prop.style styles
                ]
            ]
        ]
    ]

let colToView dispatch names (col:Shared.Domain.Column) =

    let currentColumnItemValue =
        names
        |> List.tryFind (fun (x,_) -> x = col.Name)
        |> Option.map snd
        |> Option.defaultValue ""

    let items = col.Items |> List.sortBy (fun x -> x.Name) |> List.map (itemToView dispatch col)

    Helpers.column [
        Html.div [
            prop.children [
                Html.label [ prop.text col.Name ]
                Html.i [
                    prop.className [true,"delete"; not (Shared.Validation.canDeleteColumn col), "is-hidden"]
                    prop.onClick (fun _ -> col.Name |> RemoveColumn |> dispatch)
                ]

                Html.div [
                    prop.className "field is-grouped"
                    prop.children [
                        Html.p [
                            prop.className "control is-expanded"
                            prop.children [
                                Html.input [
                                    prop.className "input"
                                    prop.placeholder "Add task"
                                    prop.value currentColumnItemValue
                                    prop.onTextChange (fun t -> ItemTextEdited(col.Name, t) |> dispatch )
                                ]
                            ]
                        ]
                        Html.p [
                            prop.className "control"
                            prop.children [
                                Html.button [
                                    prop.className "button is-info"
                                    prop.text "Add"
                                    prop.onClick (fun _ -> col.Name |> AddItemToColumn |> dispatch)
                                    prop.disabled (Shared.Validation.canAddItem col.Items currentColumnItemValue |> not)
                                ]
                            ]
                        ]
                    ]
                ]
                Html.ul [
                    prop.children items
                ]
            ]
        ]
    ]

let view (model : Model) (dispatch : Msg -> unit) =
    let columnsView =
        model.Columns
        |> List.sortBy (fun x -> x.Name)
        |> List.map (colToView dispatch model.NewItemNames)

    [
        yield! columnsView
        yield Helpers.column [
            Html.button [
                prop.className [ true, "button is-primary"; model.IsAddingNew, "is-hidden" ]
                prop.children [
                    Helpers.icon "fas fa-plus"
                    Html.span "Add new column"
                ]
                prop.onClick (fun _ -> dispatch StartEditing)
            ]
            Html.div [
                prop.className [ not model.IsAddingNew, "is-hidden" ]
                prop.children [
                    Helpers.columns [
                        Helpers.column [
                            Html.input [
                                prop.className "input"
                                prop.placeholder "Type in name"
                                prop.value model.NewColumnName
                                prop.onTextChange (fun t -> t |> TextEdited |> dispatch)
                            ]
                            Helpers.columns [
                                Helpers.column [
                                    Html.button [
                                        prop.className "button is-info"
                                        prop.text "Add"
                                        prop.onClick (fun _ -> dispatch AddNewColumn)
                                        prop.disabled (Shared.Validation.canAddColumn model.Columns model.NewColumnName |> not)
                                    ]
                                    Html.button [
                                        prop.className "button is-warning"
                                        prop.text "Cancel"
                                        prop.onClick (fun _ -> dispatch CancelEditing)
                                    ]
                                ]
                            ]
                        ]
                    ]

                ]
            ]
        ]
    ] |> Helpers.inTemplate