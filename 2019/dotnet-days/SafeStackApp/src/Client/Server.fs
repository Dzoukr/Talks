module Client.Server

open Fable.Remoting.Client
open Shared

let api : ICounterApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ICounterApi>