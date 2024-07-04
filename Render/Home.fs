namespace blog

module Home = 
    open Feliz.ViewEngine
    open Layout
    open Model
    open Feliz.ViewEngine.style

    let buildPostCard post = 
        Html.div [
            prop.id post.FileName
            prop.classes ["card" ;  "post-card"; "my-card"; "is-clickable"]
            prop.style [
                style.backgroundColor "#F6F5F1"
            ]
            prop.children [
                Html.div [
                    prop.classes ["card-content"; "p-4"]
                    prop.children [
                        match post.Category with 
                        | Note -> noteIcon
                        | Essay -> EssayIcon
                        | _ -> failwith "unknown post category"

                        Html.a [
                            prop.classes  ["is-family-secondary"; "is-size-4"; "ml-3"]
                            prop.href (sprintf "/%s.html" post.FileName)
                            prop.text post.Title
                            prop.style [
                                style.fontWeight 400
                            ]
                        ]
                    ]
                ]
                Html.footer [
                    prop.className "card-footer"
                    prop.style [
                        style.color "Black"
                    ]
                    prop.children [
                        Html.p [
                            prop.classes ["p-1"; "card-footer-item"; "is-size-7"; "has-text-right"]

                            prop.text ((post.Category |> string).ToUpper())
                        ]
                    
                        Html.p [
                            prop.classes ["p-1"; "card-footer-item"; "is-size-7"; "has-text-left"]
                            prop.text 
                                (
                                match post.Category with 
                                | Essay -> sprintf "PUBLISHED %s" (summarizeDate post.Updated)
                                | Note -> sprintf "UPDATED %s" (summarizeDate post.Updated)
                                | _ -> sprintf "UPDATED %s" (summarizeDate post.Updated)
                                )
                        ]
                    ]
                ]
            ]
        ]



    let buildTagList (posts : Post seq) = 

        let tagList = 
            posts
            |> Seq.map(fun p -> p.Tags)
            |> Seq.concat
            |> Set.ofSeq
            |> Set.toSeq
            |> Seq.map(fun t ->
                Html.span [
                    prop.id t
                    prop.classes ["tag"; "is-hoverable"; "is-rounded"; "is-size-6"]
                    prop.text t
                ]
                )

        let topicLabel = 
            Html.span [
                prop.text "TOPICS"
                prop.style [
                    borderStyle.solid
                    style.borderRightWidth 2 
                    style.borderBottomWidth 0
                    style.borderTopWidth 0
                    style.borderLeftWidth 0
                    style.borderRightColor "#00d1b2"
                    style.paddingRight 20
                    style.marginRight 10
                ]
            ]

        Html.div [
            prop.classes ["mb-6"]
            prop.children  (tagList |> Seq.append [topicLabel])
        ]

    let getPostSummaries posts =
        posts 
        |> Seq.map(fun p -> 
            buildPostCard p
        )

    let getColumnPosts posts colNo totCols = 
        posts
        |> Seq.mapi(fun i p -> i,p)
        |> Seq.filter(fun (i, _ ) -> 
            [colNo..totCols..1000] |> Seq.contains  (i + 1)
            )
        |> Seq.map snd

    let homeHeader = 
        Html.div [
            prop.classes [ "container";]
            prop.children [
                Html.div [
                    prop.classes ["title"; "is-size-1"; "mb-5"; "mt-6"]
                    prop.text "What is Lorem Ipsum?"
                    prop.style [
                        style.fontWeight 550
                    ]
                    
                ]
                
                Html.div [
                    prop.classes ["subtitle"; "is-size-2"; "is-family-secondary"; "mb-6"]
                    prop.text "Lorem Ipsum is simply dummy text of the printing and typesetting industry."
                    prop.style [
                        style.fontWeight 300
                    ]
                ]
            ]
        ]
        
    let renderGrid (posts : Post seq) = 
        let renderedPosts = getPostSummaries posts

        Html.div [
            prop.classes [ "container";]
            prop.children [
                Html.div [
                    prop.children  (buildTagList posts)
                ]
                Html.div [
                    prop.className "columns is-multiline"
                    prop.children [
                        Html.div [
                            prop.className "column is-one-third"
                            prop.children (getColumnPosts renderedPosts 1 3)
                        ]
                        Html.div [
                            prop.className "column is-one-third"
                            prop.children (getColumnPosts renderedPosts 2 3)
                        ]
                        Html.div [
                            prop.className "column is-one-third"
                            prop.children (getColumnPosts renderedPosts 3 3)
                        ]       
                    ]
                ]     // Add more card columns here based on the structure
            ]
        ]
               
    let renderHomePage posts = 
        Html.div [
            prop.classes [ "container";]
            prop.children [
                homeHeader
                renderGrid posts
            ]
        ]
        