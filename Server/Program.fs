namespace blog

module builder =
    [<EntryPoint>]
    let main args =

        printfn "%A" args
        let singlePost = if args.Length > 1 then Some args[1] else None

        Render.build (args[0]) singlePost
        0
