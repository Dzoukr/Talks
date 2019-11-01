module Server.App

open System
open System.IO
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Shared.Domain

open Fable.Remoting.Server
open Fable.Remoting.Giraffe

let publicPath = Path.GetFullPath "../Client/public"

let countAPI = {
    GetRandomCount = fun () -> async { return System.Random().Next(1,1000) }
}

let countAPIHandler =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue countAPI
    |> Remoting.buildHttpHandler

let columnsAPI = {
    AddColumn = fun name -> async { return ColumnsManager.addColumn name }
    RemoveColumn = fun name -> async { return ColumnsManager.removeColumn name }
    AddItemToColumn = fun (col,item) -> async { return ColumnsManager.addItemToColumn col item }
    GetAll = fun _ -> async { return ColumnsManager.getAll () }
}

let columnsAPIHandler =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue columnsAPI
    |> Remoting.withErrorHandler (fun ex _ -> Propagate ex.Message)
    |> Remoting.buildHttpHandler

let webApp = choose [ countAPIHandler; columnsAPIHandler ]

let configureApp (app : IApplicationBuilder) =
    app.UseDefaultFiles()
       .UseStaticFiles()
       .UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

WebHost
    .CreateDefaultBuilder()
    .UseWebRoot(publicPath)
    .UseContentRoot(publicPath)
    .Configure(Action<IApplicationBuilder> configureApp)
    .ConfigureServices(configureServices)
    .UseUrls("http://0.0.0.0:8085/")
    .Build()
    .Run()