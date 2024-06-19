namespace blog

open Feliz.ViewEngine 
open System.IO
open FSharp.Formatting.Markdown
open FSharp.Formatting.Literate
open Layout

module Master =

    let doc =  File.ReadAllText "testing.md" 
    let md = Literate.ParseMarkdownString(doc)

    let getMetaData doc = 
        md.Paragraphs
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

    let getArticleHtml doc = 
        md.Paragraphs
        |> Seq.filter(fun p -> 
            match p with 
            | YamlFrontmatter (_,_) -> false
            | _ -> true
            )
        |> Seq.toList
        |> fun paras -> MarkdownDocument(paras, dict [] )
        |> Markdown.ToHtml

    let article = File.ReadAllText "testing.md"
    let parsed = Literate.ParseMarkdownString(article)

    let html = getArticleHtml parsed
    let title = (getMetaData parsed)["title"]

    let twoColumns = 
            Html.div [ 
                prop.classes ["container"; "is-primary"; "is-fluid"]
                prop.children [
                    Html.div [
                        prop.className "columns" 
                        prop.children [
                            Html.div [
                                prop.classes ["column"; "is-one-quarter"; "has-background-primary"]
                                prop.children [
                                    menu
                                ]
                            ]
                            Html.div [
                                prop.classes ["column"; "is-three-quarters"; "has-background-info"; "content"; "is-medium"]
                                prop.children [
                                    Html.div [
                                        prop.classes ["title";"is-2"]
                                        prop.text title
                                    ]
                                    Html.div [
                                        prop.dangerouslySetInnerHTML html
                                    ]
                                ]
                            ]
                        ]     
                    ]
                ]
            ]

    let render =
        [
            Html.header [
                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://cdn.jsdelivr.net/npm/bulma@1.0.0/css/bulma.min.css"
                ]
            ]  
            navbar
            twoColumns
            footer
        ]
        |> Render.htmlView