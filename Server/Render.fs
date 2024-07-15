namespace blog

module Render =
    open System
    open System.IO
    open System.Globalization
    open FSharp.Formatting.Markdown
    open FSharp.Formatting.Literate 
    open Post    
    open Model
    open Home
    open About
    open SearchIndex
    open System.Text
    open FSharp.Formatting.Literate.Evaluation
    open MasterLayout

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

    let deserialisePosts files =
        printfn "%A" files
        files
        |> Seq.filter(fun (f: string) -> not (f.Contains("About.md")))
        |> Seq.toArray
        |> Array.map(fun f -> 
            
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
                MainImage = metaData.TryFind("image")
                Category = category
                Tags = tags
                Updated = updated 
                Created = created
            }
        )


    let build outDir (singlePost: string option)= 
        let posts = 
            let contentPath = sprintf "%s/Content" (Directory.GetCurrentDirectory())
        
            match singlePost with 
            | Some p -> 
                printfn "fast"
                Directory.EnumerateFiles contentPath 
                |> Seq.filter(fun f -> f <> "About.md" && (f. Contains(p)))
                |> Seq.head
                |> fun f -> [f]
                |> List.toSeq
                |> deserialisePosts
            | None -> 
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

        build posts outDir

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

