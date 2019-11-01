module Client.View

open Domain
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Thoth.Json

open Feliz
open Shared

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

let view (model : Model) (dispatch : Msg -> unit) =
    Html.div [
        navbarTitle "Stafe Stack App"
        section [
            container [
                Html.h3 [
                    prop.className "title is-3"
                    prop.text (sprintf "Actual count is: %i" model.Counter)
                ]

                columns [
                    column [
                        Html.button [
                            prop.className "is-primary button is-large"
                            prop.text "Increment"
                            prop.onClick (fun _ -> dispatch Increment)
                        ]
                    ]
                    column [
                        Html.button [
                            prop.className "is-danger button is-large"
                            prop.text "Decrement"
                            prop.onClick (fun _ -> dispatch Decrement)
                        ]
                    ]
                    column [
                        Html.button [
                            prop.className [ true, "is-info button is-large"; model.IsLoading, "is-loading" ]
                            prop.text "Reload from Server"
                            prop.onClick (fun _ -> dispatch LoadFromServer)
                        ]
                    ]
                ]
            ]
        ]

    ]