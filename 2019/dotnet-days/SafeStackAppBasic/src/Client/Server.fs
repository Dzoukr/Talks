﻿module Client.Server

open Fable.Remoting.Client
open Shared.Domain

let countAPI : ServerCountAPI =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ServerCountAPI>