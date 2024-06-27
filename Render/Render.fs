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
                    prop.src "script.js"
                    prop.type' "module"
                ]
            ]  



            Html.body [
                prop.custom("onLoad", "execScripts()")                                  
                prop.style [
                    style.backgroundColor "#F6F5F1"
                    style.custom ("--family-serif", "'Yrsa', serif")
                    style.custom ("--family-secondary-serif", "'Merriweather', serif")
                    style.custom ("--family-primary", "var(--family-serif)")
                    style.custom ("--family-secondary", "var(--family-secondary-serif)")
                    style.fontFamily "var(--family-primary)"
                    style.fontFamily "var(--family-secondary)"
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
        |> Seq.filter(fun f -> f <> "content\\About.md")
        |> Seq.map(fun f -> 
            printfn "%s" f

            let rawPost = File.ReadAllText(f, Encoding.UTF8) |> Literate.ParseMarkdownString

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
                Summary = (getMetaData rawPost)["summary"]
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
            |> Seq.filter(fun f -> f <> "About.md")
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
                System.IO.File.WriteAllText(renderedFileName, x, Encoding.UTF8)
            |> ignore
        )

        buildSearchIndex posts

        renderAboutPage() 
        |> render 
        |> fun x -> 
            System.IO.File.WriteAllText("public/about.html", x)
        |> ignore

        // build home page
        renderHomePage posts
        |> render
        |> fun x -> 
            System.IO.File.WriteAllText("public/index.html", x)
        |> ignore





        




    

    
    