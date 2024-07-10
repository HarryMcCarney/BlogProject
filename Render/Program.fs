namespace blog
module builder = 
    [<EntryPoint>]
    let main args =

        printfn "%A" args
        let fastRender = 
            if args[1] = "true" then true else false
        Render.build (args[0]) fastRender
        0

