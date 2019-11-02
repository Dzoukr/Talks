module Client.Server

open Fable.Remoting.Client
open Shared.Domain

let columnsAPI : ServerColumnsAPI =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ServerColumnsAPI>