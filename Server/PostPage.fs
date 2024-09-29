namespace blog

module Post = 
    open Feliz.StaticHtml
    open Model
    open Layout


    let EssayTags (post: Post) =  
        post.Tags
        |> Seq.map(fun t ->
            Html.span [
                Attr.classes ["tag";"is-medium"]
                Html.text t
            ]
        )

    let tagsAndDate (post: Post) = 
            Html.div [
                Attr.classes ["columns";"mb-6"]
                Html.div [
                    Attr.classes ["column"; "is-two-thirds";"pr-2"; "has-text-dark"]
                    Fragment  (EssayTags post |> Seq.toList) 
                ]
                
                Html.div [
                    Attr.classes ["column"; "is-one-third"; "is-size-7"; "has-text-right" ; "has-text-dark"]
                    Attr.style "font-weight:350"
                    Html.p [
                        Html.text 
                            (
                            match post.Category with 
                            | Essay -> sprintf "CREATED %s" (summarizeDate post.Created)
                            | Note -> sprintf "CREATED %s" (summarizeDate post.Created)
                            | _ -> sprintf "CREATED %s" (summarizeDate post.Created)
                            )
                        if post.Created <> post.Updated then 
                            Html.p [
                                Html.text 
                                    (
                                    match post.Category with 
                                    | Essay -> sprintf "UPDATED %s" (summarizeDate post.Updated)
                                    | Note -> sprintf "UPDATED %s" (summarizeDate post.Updated)
                                    | _ -> sprintf "UPDATED %s" (summarizeDate post.Updated)
                                    )
                            ]   
                    ]
                ]
            ]


    let renderPost (post: Post) =
        Html.section [
            Attr.classes ["container"; "content"; "is-medium"; "has-text-dark"]
            Attr.style "max-width: 800";
            Html.nav [
                Attr.classes ["level"; "mt-6"]
                Html.div [
                    Attr.classes ["level-left"]
                    Html.div [
                        Attr.classes ["level-item"]
                        match post.Category with 
                        | Note -> noteIcon
                        | Essay -> essayIcon
                        | Talk -> talkIcon
                        | _ -> failwith "unknown post category"           
                        Html.p [
                            Attr.classes ["ml-2"; "has-text-dark"]
                            Html.text 
                                (
                                match post.Category with 
                                | Essay -> "Essay"
                                | Note -> "Note"
                                | Talk -> "Talk"
                                | _ -> ""
                                )
                        ]
                    ]
                ]
            ]
            Html.div [
                Attr.classes ["title"; "is-size-1"; "is-size-2-mobile"; "mb-5"; "has-text-dark"]
                Html.text post.Title
            ]
            Html.div [
                Attr.classes ["subtitle"; "is-size-3"; "is-size-4-mobile";"is-family-secondary"; "mb-6"; "has-text-dark"]
                Attr.style "font-weight: 350"
                Html.text post.Summary
            ]
            tagsAndDate post
            Html.div [
                Html.text post.Content
            ]
        ]


        
        
    
     
