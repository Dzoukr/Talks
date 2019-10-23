module Server.WebApp

open System

let random = Random()
let getRandomCount () =
    async {
        return random.Next(0,999)
    }

let serverAPI : Shared.ServerAPI = {
    GetRandomCount = getRandomCount
}