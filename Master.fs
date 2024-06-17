namespace blog

open Feliz.ViewEngine 
open RulesForDistributedAgile

module Master =
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
                                    prop.children RulesForDistributedAgile.content
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
                ]  
                Html.div [ 
                    prop.className "title" 
                    prop.text "This is my very simple blog" 
                ]
                twoColumns
            ]
            |> Render.htmlView