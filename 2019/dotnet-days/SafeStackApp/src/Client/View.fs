module Client.View

open System
open Domain
open Fable.React
open Feliz
open Fable.Core.JsInterop
open Shared.Domain

let private navbarTitle (str:string) =
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

let private section (childs:seq<ReactElement>) = Html.section [ prop.className "section"; prop.children childs ]
let private container (childs:seq<ReactElement>) = Html.div [ prop.className "container"; prop.children childs ]
let private columns (childs:seq<ReactElement>) = Html.div [ prop.className "columns"; prop.children childs ]
let private column (childs:seq<ReactElement>) = Html.div [ prop.className "column"; prop.children childs ]
let private icon (className:string) = Html.span [ prop.className "icon"; prop.children [ Html.i [ prop.className className ] ] ]


//let view (model : Model) (dispatch : Msg -> unit) =
//    Html.div [
//        navbarTitle "Stafe Stack App"
//        section [
//            container [
//                Html.h3 [
//                    prop.className "title is-3"
//                    prop.text (sprintf "Actual count is: %i" model.Counter)
//                ]
//
//                columns [
//                    column [
//                        Html.button [
//                            prop.className "is-primary button is-large"
//                            prop.text "Increment"
//                            prop.onClick (fun _ -> dispatch Increment)
//                        ]
//                    ]
//                    column [
//                        Html.button [
//                            prop.className "is-danger button is-large"
//                            prop.text "Decrement"
//                            prop.onClick (fun _ -> dispatch Decrement)
//                        ]
//                    ]
//                    column [
//                        Html.button [
//                            prop.className [ true, "is-info button is-large"; model.IsLoading, "is-loading" ]
//                            prop.text "Reload from Server"
//                            prop.onClick (fun _ -> dispatch LoadFromServer)
//                        ]
//                    ]
//                ]
//            ]
//        ]
//     ]

let colToView dispatch (col:Shared.Domain.Column) =
    let addOnlyIfCanBeAdded value =
        if value |> Shared.Validation.canAddItem col.Items then
            (col.Name, value) |> AddItemToColumn |> dispatch

    let itemToView (i:Item) =
        Html.li [
            prop.text i.Name
        ]
    let items = col.Items |> List.sortBy (fun x -> x.Name) |> List.map itemToView
    column [
        Html.div [
            prop.children [
                Html.label [ prop.text col.Name ]
                Html.i [
                    prop.className [true,"delete"; not (Shared.Validation.canDeleteColumn col), "is-hidden"]
                    prop.onClick (fun _ -> col.Name |> RemoveColumn |> dispatch)
                ]
                Html.input [
                    prop.className "input"
                    prop.placeholder "Add task"
                    prop.onBlur (fun e -> !!e.target?value |> addOnlyIfCanBeAdded)
                ]
                Html.ul [
                    prop.children items
                ]
            ]
        ]
    ]

let view (model : Model) (dispatch : Msg -> unit) =
    let data =
        model.Columns
        |> List.sortBy (fun x -> x.Name)
        |> List.map (colToView dispatch)

    Html.div [
        navbarTitle "Stafe Stack App"
        section [
            container [
                columns [
                    yield! data

                    yield column [
                        Html.button [
                            prop.className [ true, "button is-primary"; model.IsAddingNew, "is-hidden" ]
                            prop.children [
                                icon "fas fa-plus"
                                Html.span "Add new column"
                            ]
                            prop.onClick (fun _ -> dispatch StartEditing)
                        ]
                        Html.div [
                            prop.className [ not model.IsAddingNew, "is-hidden" ]
                            prop.children [
                                columns [
                                    column [
                                        Html.input [
                                            prop.className "input"
                                            prop.placeholder "Type in name"
                                            prop.value model.NewColumnName
                                            prop.onTextChange (fun t -> t |> TextEdited |> dispatch)
                                        ]
                                        columns [
                                            column [
                                                Html.button [
                                                    prop.className "button is-info"
                                                    prop.text "Add"
                                                    prop.onClick (fun _ -> dispatch AddNewColumn)
                                                    prop.disabled (Shared.Validation.canAddColumn model.Columns model.NewColumnName |> not)
                                                ]
                                                Html.button [
                                                    prop.className "button is-warning"
                                                    prop.text "Cancel"
                                                    prop.onClick (fun _ -> dispatch StopEditing)
                                                ]
                                            ]
                                        ]
                                    ]
                                ]

                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]