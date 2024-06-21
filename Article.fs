namespace blog

module Article = 
    open Feliz.ViewEngine
    open Layout

    let renderArticle (title: string) (htmlBody: string) = 
    
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
                                        prop.dangerouslySetInnerHTML htmlBody
                                    ]
                                ]
                            ]
                        ]     
                    ]
                ]
            ]