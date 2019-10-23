open System
open Domain

// configuration
let cosmosDbCfg = CosmoStore.CosmosDb.Configuration.CreateDefault 
                    (Uri "https://localhost:8081") 
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="

let tableStorageCfg = CosmoStore.TableStorage.Configuration.CreateDefaultForLocalEmulator ()

//let demoStore = CommandHandler.createDemoStore (CommandHandler.StorageType.CosmosDb cosmosDbCfg)
let demoStore = CommandHandler.createDemoStore (CommandHandler.StorageType.TableStorage tableStorageCfg)

// simple function composing command handler + event handlers together
let pipeline cmd =
    cmd
    |> CommandHandler.handle demoStore
    |> List.iter ReadSide.handle

let printState (desc:string) = demoStore.GetCurrentState() |> printfn "[%s] %A" (desc.ToUpper())

[<EntryPoint>]
let main argv =

    printState "Initial"

    AddTask { Id = 2; Name = "Give cool talk"; DueDate = None } |> pipeline
    
    printState "After task added"

    CompleteTask { Id = 2 } |> pipeline
    
    printState "After task completed"

    ClearAllTasks  |> pipeline
    
    printState "After clear"
    0