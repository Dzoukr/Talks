module Client.View

open Domain
open Fable.React
open Feliz

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
            navbarTitle "SAFE Stack App"
            section [
                container controls
            ]
        ]

let view (model : Model) (dispatch : Msg -> unit) =
    [
        Html.h3 [
            prop.className "title is-3"
            prop.text (sprintf "Actual count is: %i" model.Counter)
        ]

        Helpers.columns [
            Helpers.column [
                Html.button [
                    prop.className "is-primary button is-large"
                    prop.text "Increment"
                    prop.onClick (fun _ -> dispatch Increment)
                ]
            ]
            Helpers.column [
                Html.button [
                    prop.className "is-danger button is-large"
                    prop.text "Decrement"
                    prop.onClick (fun _ -> dispatch Decrement)
                ]
            ]
            Helpers.column [
                Html.button [
                    prop.className [ true, "is-info button is-large"; model.IsLoading, "is-loading" ]
                    prop.text "Reload from Server"
                    prop.onClick (fun _ -> dispatch LoadFromServer)
                ]
            ]
        ]
    ] |> Helpers.inTemplate