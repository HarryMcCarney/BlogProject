namespace blog

module Post = 
    open Feliz.ViewEngine
    open Model
    open Layout

    let renderPost (post: Post) = 

        let articleTags =  
            post.Tags
            |> Seq.map(fun t ->
                Html.span [
                    prop.classes ["tag";"is-medium"]
                    prop.text t
                ]
            )

        Html.div [ 
            prop.classes ["container"; "content"; "is-medium"]
            prop.style [
                style.backgroundColor "#F3F3F3"
            ]
            prop.children [
                Html.p [
                    prop.text 
                        (
                        match post.Category with 
                        | Article -> "Article"
                        | Note -> "Note"
                        | _ -> ""
                        )
                ]

                Html.div [
                    prop.classes ["title";"is-2"]
                    prop.text post.Title
                ]
                Html.div [
                    prop.classes ["subtitle"]
                    prop.text post.Summary
                ]
                
                Html.div [
                    prop.classes ["columns"]
                    prop.children [
                        Html.div [
                            prop.classes ["column"; "is-one-half"]
                            prop.children articleTags
                        ]
                        Html.div [
                            prop.classes ["column"; "is-one-half"; "is-right"]
                            prop.children [
                                Html.p [
                                    prop.text 
                                        (
                                        match post.Category with 
                                        | Article -> sprintf "Created %s" (summarizeDate post.Created)
                                        | Note -> sprintf "Created %s" (summarizeDate post.Created)
                                        | _ -> sprintf "Created %s" (summarizeDate post.Created)
                                        )
                                ]
                                if post.Created <> post.Updated then 
                                    Html.p [
                                        prop.text 
                                            (
                                            match post.Category with 
                                            | Article -> sprintf "Updated %s" (summarizeDate post.Updated)
                                            | Note -> sprintf "Updated %s" (summarizeDate post.Updated)
                                            | _ -> sprintf "Updated %s" (summarizeDate post.Updated)
                                            )
                                    ]
                            ]
                        ]
                    ]
                ]

                Html.div [
                    prop.dangerouslySetInnerHTML post.Content
                ]
            ]
        ]
        
            
