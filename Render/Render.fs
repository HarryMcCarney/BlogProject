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


    let getArticleHtml (doc: LiterateDocument) = 
        doc.Paragraphs
        |> Seq.filter(fun p -> 
            match p with 
            | YamlFrontmatter (_,_) -> false
            | _ -> true
            )
        |> Seq.toList
        |> fun paras -> MarkdownDocument(paras, dict [] )
        |> Markdown.ToHtml

    
    let render content =
        [
            Html.header [
                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://cdn.jsdelivr.net/npm/bulma@1.0.0/css/bulma.min.css"
                ]
                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://use.fontawesome.com/releases/v5.15.4/css/all.css"
                ]
                Html.script [
                    prop.src "script.js"
                    prop.type' "module"
                    
                ]
            ]  

            Html.body [
                prop.custom("onLoad", "execScripts()")                                  
                prop.style [
                style.backgroundColor "#F3F3F3"
                ]
                prop.children [
                    navbar
                    content
                    footer
                ]
            ]

        ]
        |> Render.htmlView
  
    let deserialisePosts files =
        Directory.EnumerateFiles "content"
        |> Seq.map(fun f -> 

            let rawPost = File.ReadAllText f |> Literate.ParseMarkdownString

            let updated = 
                let d = (getMetaData rawPost)["updated"]
                DateTime.ParseExact(d , "yyyyMMdd", CultureInfo.InvariantCulture) 
            
            let created = 
                let d = (getMetaData rawPost)["created"]
                DateTime.ParseExact(d , "yyyyMMdd", CultureInfo.InvariantCulture) 

            let category = 
                match (getMetaData rawPost)["category"] with 
                | "Article" -> Article
                | "Draft" -> Draft 
                | "Note" -> Note
                | _ -> failwith "Unknown category in markdown file"
            
            let tags = 
                (((getMetaData rawPost)["tags"]).Split ",")
                |> Array.map(fun t -> t.Trim())
                
            {
                FileName = Path.GetFileNameWithoutExtension f
                Title = (getMetaData rawPost)["title"]
                Content = getArticleHtml rawPost
                Category = category
                Tags = tags
                Updated = updated 
                Created = created
            }
        )

    let build() = 
        let posts = 
            let contentPath = sprintf "%s/content" (Directory.GetCurrentDirectory())
            Directory.EnumerateFiles contentPath 
            |> deserialisePosts
        
        posts
        |> Seq.iter(fun post -> 
            match post.Category with
            | Article ->  renderPost post
            | Note ->  renderPost post
            | Draft ->  renderPost post
            |> render
            |> fun x -> 
                let renderedFileName = sprintf "public/%s.html" post.FileName
                System.IO.File.WriteAllText(renderedFileName, x)
            |> ignore
        )

        // build home page
        renderHomePage posts
        |> render
        |> fun x -> 
            System.IO.File.WriteAllText("public/index.html", x)
        |> ignore





        




    

    
    