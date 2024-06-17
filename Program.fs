namespace blog
module builder = 
    open Feliz.ViewEngine

    open Suave
    open Suave.Filters
    open Suave.Operators

    let app = 
        choose [
            GET >=> path "/" >=> Files.file "index.html"
        ]

    [<EntryPoint>]
    let main args =

        Master.render
        |> fun x -> System.IO.File.WriteAllText("index.html", x)
        |> ignore

        startWebServer defaultConfig app
        0

