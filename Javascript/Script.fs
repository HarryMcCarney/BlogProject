
namespace blog

module Scripts = 

    open Browser.Dom
    open Fable.Core
    open Model 
    open Thoth.Json
    open System
    open Fable.SimpleHttp

    let categoryDecoder : Decoder<Category> =
        Decode.string
        |> Decode.andThen (function
            | "Draft" -> Decode.succeed Draft
            | "Note" -> Decode.succeed Note
            | "Article" -> Decode.succeed Article
            | other -> Decode.fail $"Unknown category: {other}")

    let dateTimeDecoder : Decoder<DateTime> =
        Decode.string
        |> Decode.andThen (fun dateString ->
            match DateTime.TryParse(dateString) with
            | (true, date) -> Decode.succeed date
            | _ -> Decode.fail $"Invalid date: {dateString}")

    let postDecoder : Decoder<Post> =
        Decode.object (fun get ->
            {
                FileName = get.Required.Field "FileName" Decode.string
                Title = get.Required.Field "Title" Decode.string
                Summary = get.Required.Field "Summary" Decode.string
                Content = get.Required.Field "Content" Decode.string
                Tags = get.Required.Field "Tags" (Decode.array Decode.string)
                Category = get.Required.Field "Category" categoryDecoder
                Updated = get.Required.Field "Updated" dateTimeDecoder
                Created = get.Required.Field "Created" dateTimeDecoder
            }
        )

    let jsonContainerDecoder : Decoder<JsonContainer> =
        Decode.object (fun get ->
            {
                Posts = get.Required.Field "Posts" (Decode.seq postDecoder)
            }
        )

    let fetchJson (url: string) =
        async {
            let! response =
                Http.request (url)
                |> Http.method GET
                |> Http.overrideMimeType "application/json"
                |> Http.send
            
            match Decode.fromString jsonContainerDecoder response.responseText with
            | Ok data ->
                printfn "%A" data
                return Some data
            | Error error ->
                printfn "Failed to decode JSON: %s" error
                return None
        }

    let addTagFilters() =
        async{
            
            let! searchIndex = (fetchJson "SearchIndex.json")

            match searchIndex with 
            | Some si -> 
                let nodes = document.getElementsByClassName("tag")
                let tags = seq { for i in 0 .. nodes.length - 1 -> nodes.[i] }
                let postNodes = document.getElementsByClassName("post-card")
                let posts = seq { for i in 0 .. postNodes.length - 1 -> postNodes.[i] }

                tags
                |> Seq.iter(fun t -> 
                    t.addEventListener("click", fun _ ->

                        let postsToHide = 
                            si.Posts
                            |> Seq.filter(fun p -> not (p.Tags |> Array.contains t.id))
                            |> Seq.map(fun p -> p.FileName)

                        posts
                        |> Seq.filter(fun p -> postsToHide |> Seq.contains p.id)
                        |> Seq.iter(fun p -> 
                            p.classList.toggle("is-hidden") 
                            |> ignore
                        )
                    )
                )
            | None -> failwith "cannot access search index"
        } |> Async.StartImmediate
    
    let expandHamburger() =
        
            let burger = document.getElementById("navbar-burger")
            let menu = document.getElementById("navbarBasicExample")

            burger.addEventListener("click", fun _ ->
                burger.classList.toggle("is-active") 
                |> ignore
                menu.classList.toggle("is-active") 
                |> ignore 
            )

    let execScripts() =
        async{
            expandHamburger() |> ignore
            addTagFilters() |> ignore
        } |> Async.StartImmediate

    [<Emit("window.execScripts = $0")>]
    let exportexecScripts (greet: obj) = jsNative
  
    exportexecScripts execScripts




