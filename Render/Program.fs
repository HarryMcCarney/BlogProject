namespace blog
module builder = 
    [<EntryPoint>]
    let main args =
        Render.build (args[0])
        0

