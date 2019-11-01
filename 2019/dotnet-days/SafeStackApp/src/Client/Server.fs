module Client.Server

open Fable.Remoting.Client
open Shared.Domain

let api : ServerAPI =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ServerAPI>