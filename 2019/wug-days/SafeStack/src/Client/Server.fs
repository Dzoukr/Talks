module Client.Server

open Shared
open Fable.Remoting.Client

let api : ServerAPI =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Routes.builder
  |> Remoting.buildProxy<ServerAPI>