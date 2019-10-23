namespace Shared

module Routes =
    let builder typ method = sprintf "/api/%s/%s" typ method

type ServerAPI = {
    GetRandomCount : unit -> Async<int>
}