namespace blog
module builder = 
    open Feliz.ViewEngine

    open Suave
    open Suave.Filters
    open Suave.Operators
    open System.IO

    let app = 
        choose [
            GET >=> path "/" >=> Files.file "public/index.html"
            GET >=> Files.browseHome            
            RequestErrors.NOT_FOUND "Page not found." 
        ]

    [<EntryPoint>]
    let main args =

        Render.build()
        |> ignore

        let config =
            {defaultConfig with homeFolder = Some (Path.GetFullPath "public") }
            
        startWebServer config app
        0

