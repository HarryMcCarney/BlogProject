namespace blog

module Post = 
    open Feliz.ViewEngine
    open Model
    open Layout



    let EssayTags (post: Post) =  
        post.Tags
        |> Seq.map(fun t ->
            Html.span [
                prop.classes ["tag";"is-medium"]
                prop.text t
            ]
        )

    let tagsAndDate (post: Post) = 
            Html.div [
                prop.classes ["columns";"mb-6"]
                prop.children [
                    Html.div [
                        prop.classes ["column"; "is-two-thirds";"pr-2"]
                        prop.children  (EssayTags post) 
                        ]
                    
                    Html.div [
                        prop.classes ["column"; "is-one-third"; "is-size-6"; "has-text-right" ]
                        prop.style [
                            style.fontWeight 350
                        ]
                        prop.children [
                            Html.p [
                            prop.text 
                                (
                                match post.Category with 
                                | Essay -> sprintf "Created %s" (summarizeDate post.Created)
                                | Note -> sprintf "Created %s" (summarizeDate post.Created)
                                | _ -> sprintf "Created %s" (summarizeDate post.Created)
                                )
                            ]
                            if post.Created <> post.Updated then 
                                Html.p [
                                    prop.text 
                                        (
                                        match post.Category with 
                                        | Essay -> sprintf "Updated %s" (summarizeDate post.Updated)
                                        | Note -> sprintf "Updated %s" (summarizeDate post.Updated)
                                        | _ -> sprintf "Updated %s" (summarizeDate post.Updated)
                                        )
                                ]
                        ]
                    ]
                ]
            ]

    let renderPost (post: Post) =
        Html.div [
            prop.classes ["container"; "content"; "is-medium"]
            prop.style [
                style.backgroundColor "#F6F5F1"
                style.maxWidth 800;
            ]
            prop.children [
                Html.nav [
                    prop.classes ["level"; "mt-6"]
                    prop.children [
                        Html.div [
                            prop.classes ["level-left"]
                            prop.children [
                                Html.div [
                                    prop.classes ["level-item"]
                                    prop.children [   
                                        match post.Category with 
                                        | Note -> noteIcon
                                        | Essay -> EssayIcon
                                        | _ -> failwith "unknown post category"           
                                        Html.p [
                                            prop.classes ["ml-2"]
                                            prop.text 
                                                (
                                                match post.Category with 
                                                | Essay -> "Essay"
                                                | Note -> "Note"
                                                | _ -> ""
                                                )
                                        ]
                                    ]
                                ]
                   
                            ]
                        ]
                    ]
                ]

                Html.div [
                    prop.classes ["title"; "is-size-1"; "mb-5"]
                    prop.text post.Title
                ]
                Html.div [
                    prop.classes ["subtitle"; "is-size-3"; "is-family-secondary"; "mb-6"]
                    prop.style [
                        style.fontWeight 350
                    ]
                    prop.text post.Summary
                ]
                tagsAndDate post
                Html.div [
                    prop.dangerouslySetInnerHTML post.Content
                ]
            ]
        ]
        
    
     
