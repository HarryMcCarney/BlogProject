namespace blog 

module About = 

    open Feliz.ViewEngine
    open System.IO
    open FSharp.Formatting.Markdown
    open FSharp.Formatting.Literate

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
            File.ReadAllText "content/About.md" 
            |> Literate.ParseMarkdownString
            |> getAboutHtml

        Html.div [ 
            prop.classes ["container"; "content"; "is-medium"]
            prop.style [
                style.backgroundColor "#F3F3F3"
            ]
            prop.children [
                Html.section [
                    prop.className "section"
                    prop.children [
                        Html.div [
                            prop.classes ["title";"is-2"]
                            prop.text "Some stuff about me"
                        ]
                    ]
                ]
                Html.div [
                    prop.dangerouslySetInnerHTML content
                ]
            ]
        ]
    


