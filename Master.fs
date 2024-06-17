namespace blog

open Feliz.ViewEngine 
open RulesForDistributedAgile
open System.IO
open FSharp.Formatting.Markdown


module Master =

        let article = File.ReadAllText "testing.md"
        let parsed = Markdown.Parse(article)
        let html = Markdown.ToHtml(parsed)

        let twoColumns = 
                Html.div [ 
                    prop.classes ["container"; "is-primary"; "is-widescreen"]
                    prop.children [
                        Html.div [
                            prop.className "columns" 
                            prop.children [
                                Html.div [
                                    prop.classes ["column"; "is-one-quarter"; "has-background-primary"]
                                ]
                                Html.div [
                                    prop.classes ["column"; "is-three-quarters"; "has-background-info"]

                                    prop.dangerouslySetInnerHTML html
                                ]
                            ]     
                        ]
                    ]
                ]

        let render =
            [
                Html.header [
                    Html.title "Harrys Blog "
                    Html.link [
                        prop.rel "stylesheet"
                        prop.href "https://cdn.jsdelivr.net/npm/bulma@1.0.0/css/bulma.min.css"
                    ]
                    Html.link [
                        prop.rel "stylesheet"
                        prop.href "styles.css"
                    ]
                ]  
                Html.div [ 
                    prop.className "title" 
                    prop.text "This is my very simple blog" 
                ]
                twoColumns
            ]
            |> Render.htmlView