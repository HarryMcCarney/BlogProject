namespace blog
module builder = 
    [<EntryPoint>]
    let main args =
        Render.build()
        0

