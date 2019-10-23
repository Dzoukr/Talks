module Client.View

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fulma
open Domain

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        Navbar.navbar [ Navbar.Color IsPrimary ] [
            Navbar.Item.div [] [
                Heading.h2 [] [ str "SAFE Template" ]
            ]
        ]

        Container.container [] [
            Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
                Heading.h3 [] [ str ("Press buttons to manipulate counter: " + model.Count.ToString()) ]
            ]
            Columns.columns [] [
                  Column.column [] [ button "-" (fun _ -> dispatch Decrement) ]
                  Column.column [] [ button "+" (fun _ -> dispatch Increment) ]
                  Column.column [] [ button "Reset" (fun _ -> dispatch Reset) ]
            ]
        ]

    ]