namespace blog 

module About = 

    open Feliz.StaticHtml
    open System.IO
    open FSharp.Formatting.Markdown
    open FSharp.Formatting.Literate
    open System.Text

    let getAboutHtml (doc: LiterateDocument) = 
        doc.Paragraphs
        |> Seq.filter(fun p -> 
            match p with 
            | YamlFrontmatter (_,_) -> false
            | _ -> true
            )
        |> Seq.toList
        |> fun paras -> MarkdownDocument(paras, dict [] )
        |> Markdown.ToHtml


    let renderAboutPage() = 

        let content = 
            File.ReadAllText("content/About.md", Encoding.UTF8)
            |> Literate.ParseMarkdownString
            |> getAboutHtml

        Html.div [ 
            Attr.classes ["container"; "content"; "is-medium"; "has-text-dark"; "has-background-light"]
            Html.section [
                Attr.className "section"
                Html.div [
                    Attr.classes ["title";"is-2"]
                    Html.text "About"
                ]
            ]
            
            Html.div [
                Html.text content
            ]
        ]
    


