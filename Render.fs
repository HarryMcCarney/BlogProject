namespace blog

module Render = 
    open System.IO
    open FSharp.Formatting.Markdown
    open FSharp.Formatting.Literate 
    open Article    
    open Feliz.ViewEngine
    open Layout

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
            ]  
            navbar
            content
            footer
        ]
        |> Render.htmlView
  
    let build() = 
        Directory.EnumerateFiles "content"
        |> Seq.iter(fun f -> 
            let fileName = sprintf "public/%s.html" (Path.GetFileNameWithoutExtension f)
            let article = File.ReadAllText f
            let parsed = Literate.ParseMarkdownString(article)
            let html = getArticleHtml parsed
            let title = (getMetaData parsed)["title"]
            let category = (getMetaData parsed)["category"]

            printfn "%s" category

            match category with 
            | "Article" -> renderArticle title html 
            | "Note" -> renderArticle title html 
            | _ -> failwith "unknown category"
            |> render
            |> fun x -> System.IO.File.WriteAllText(fileName, x)
        




    

    
    
    )