namespace blog
module builder = 
    open Feliz.ViewEngine

    open Suave
    open Suave.Filters
    open Suave.Operators
    open System.IO


    [<EntryPoint>]
    let main args =

        Render.build()
        |> ignore
        0

