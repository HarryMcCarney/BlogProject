namespace blog

module Render = 
    open System
    open System.IO
    open System.Globalization
    open FSharp.Formatting.Markdown
    open FSharp.Formatting.Literate 
    open Post    
    open Feliz.ViewEngine
    open Layout
    open Model
    open Home
    open About
    open SearchIndex
    open System.Text
    open FSharp.Formatting.Literate.Evaluation

    
    let getMetaData (doc: LiterateDocument) = 
        doc.Paragraphs
        |> Seq.map(fun p -> 
            match p with 
            | YamlFrontmatter (meta,_) -> 
                meta
                |> List.map(fun md ->  md.Split ':' |> fun x -> x[0].Trim(), x[1].Trim())
                |> Some
            | _ -> None)
        |> Seq.choose id
        |> Seq.concat
        |> Map


    let getEssayHtml (doc: LiterateDocument) = 
            Literate.ToHtml(doc, "", false, false)


    let render content =
        [   
            Html.html [
                prop.custom("data-theme", "light")

            ]
            Html.header [
                
                Html.meta [
                    prop.name "viewport"
                    prop.content "width=device-width, initial-scale=1"
                ]

                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://cdn.jsdelivr.net/npm/bulma@1.0.0/css/bulma.min.css"
                ]
                Html.script [
                    prop.src "https://kit.fontawesome.com/fd17b6d7c8.js"
                    prop.crossOrigin.anonymous
                ]
                Html.link [
                    prop.rel "stylesheet"
                    prop.href "styles.css"
                ]

                Html.link [
                    prop.rel "preconnect"
                    prop.href "https://fonts.googleapis.com"
                ]

                Html.link [
                    prop.rel "preconnect"
                    prop.href "https://fonts.gstatic.com"
                ]

                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://fonts.googleapis.com/css2?family=Yrsa:ital,wght@0,300..700;1,300..700&display=swap"
                ]

                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://fonts.googleapis.com/css2?family=Merriweather:ital,wght@0,300;0,400;0,700;0,900;1,300;1,400;1,700;1,900&display=swap"
                ]

                Html.script [
                    prop.src "Script.js"
                    prop.type' "module"
                ]
            ]  

            Html.body [
                prop.classes ["has-background-light"; "has-text-dark"]
                prop.custom("onLoad", "execScripts()") 
                prop.style [
                    style.width (length.percent 100)
                    style.custom ("--family-serif", "'Yrsa', serif")
                    style.custom ("--family-secondary-serif", "'Merriweather', serif")
                    style.custom ("--family-primary", "var(--family-serif)")
                    style.custom ("--family-secondary", "var(--family-secondary-serif)")
                    style.fontFamily "var(--family-primary)"
                    style.fontFamily "var(--family-secondary)"
                ]
                prop.children [
                    Html.section [
                        prop.classes ["container"; "is-fluid"]
                        prop.children [
                            navbar
                            content
                            footer

                        ]
                    ]

                ]
            ]
        ]
        |> Render.htmlView
  
    let deserialisePosts files =
        printfn "%A" files
        files
        |> Seq.filter(fun (f: string) -> not (f.Contains("About.md")))
        |> Seq.map(fun f -> 
            
            let erf = fun s -> failwith (sprintf "%s" s) 

            let fsi = FsiEvaluator(options = [|"--eval" ; "--strict"|], onError = erf)

            let rawPost =  
                match Path.GetExtension f with 
                | ".md" -> Literate.ParseMarkdownFile(f)
                | ".fsx" -> 
                    let script = File.ReadAllText f 
                    Literate.ParseScriptString(script, fsiEvaluator = fsi, onError = erf)
                    
                | _ -> failwith "Unoken file extension in content folder"

            let metaData = getMetaData rawPost

            let updated = 
                let d = metaData["updated"]
                DateTime.ParseExact(d , "yyyyMMdd", CultureInfo.InvariantCulture) 
            
            let created = 
                let d = metaData["created"]
                DateTime.ParseExact(d , "yyyyMMdd", CultureInfo.InvariantCulture) 

            let category = 
                match metaData["category"] with 
                | "Essay" -> Essay
                | "Draft" -> Draft 
                | "Note" -> Note
                | "Talk" -> Talk
                | _ -> failwith (sprintf "Unknown category in markdown file: %s" (metaData["category"]))
            
            let tags = 
                ((metaData["tags"]).Split ",")
                |> Array.map(fun t -> t.Trim())
                
            {
                FileName = Path.GetFileNameWithoutExtension f
                Title = metaData["title"]
                Summary = metaData["summary"]
                Content = getEssayHtml rawPost
                MainImage = if category = Essay then Some metaData["image"] else None
                Category = category
                Tags = tags
                Updated = updated 
                Created = created
            }
        )

    let build outDir (isFastRender: bool)= 
        let posts = 
            let contentPath = sprintf "%s/Content" (Directory.GetCurrentDirectory())
            
            printfn "inside function %b" isFastRender
            if isFastRender then 
                printfn "fast"
                Directory.EnumerateFiles contentPath 
                |> Seq.filter(fun f -> f <> "About.md" && not (f. Contains(".fsx")))
                |> Seq.head
                |> fun f -> [f]
                |> List.toSeq
                |> deserialisePosts
                else 
                    printfn "slow"
                    Directory.EnumerateFiles contentPath 
                    |> Seq.filter(fun f -> f <> "About.md")
                    |> deserialisePosts

        printfn "%i posts found" (posts |> Seq.length)
        posts
        |> Seq.iter(fun post -> 
            match post.Category with
            | Essay ->  renderPost post
            | Note ->  renderPost post
            | Draft ->  renderPost post
            | Talk ->  renderPost post
            |> render
            |> fun x -> 
                let renderedFileName = sprintf "%s/%s.html" outDir post.FileName
                System.IO.File.WriteAllText(renderedFileName, x, Encoding.UTF8)
            |> ignore
        )

        buildSearchIndex posts outDir

        renderAboutPage() 
        |> render 
        |> fun x -> 
            let aboutPath = sprintf "%s/about.html" outDir 
            System.IO.File.WriteAllText(aboutPath, x)
        |> ignore

        // build home page
        renderHomePage posts
        |> render
        |> fun x -> 
            let homePath = sprintf "%s/index.html" outDir 
            System.IO.File.WriteAllText(homePath, x, Encoding.UTF8)
        |> ignore



 

        




    

    
    