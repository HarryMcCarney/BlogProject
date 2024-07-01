namespace blog

module Home = 
    open Feliz.ViewEngine
    open Layout
    open Model
    open Feliz.ViewEngine.style
    open System
    open Feliz.ViewEngine.Styles
    open Feliz.ViewEngine.prop
    open Feliz
    
  


    let hoverStyle =
        prop.style [
            style.transform.scale 1.05
            style.transitionProperty "tramsform"
            style.transitionTimingFunction.easeInOut
            style.transitionDuration  (TimeSpan.FromSeconds(0.2))
        ]
      
      
      (*  
      
        style [
            css.hover [
                css.transform.scale(1.05)
                css.transitionProperty "transform"
                css.transitionDuration (TimeSpan.FromSeconds(0.2))
                css.transitionTimingFunction.easeInOut
            ]
        ]
    *)




    let buildPostCard post = 

        Html.div [
            prop.id post.FileName
            prop.classes ["card" ; "post-card"]
            hoverStyle 
            
            prop.children [
                Html.div [
                    prop.classes ["card-content"]
                    prop.children [
                        match post.Category with 
                                | Article ->  articleIcon
                                | Note -> noteIcon
                                | _ -> noteIcon
                        Html.a [
                            prop.classes  [ "is-family-secondary"; "is-size-4"]
                            prop.href (sprintf "/%s.html" post.FileName)
                            prop.text post.Title
                            prop.style [
                                style.fontWeight 400
                                style.color "black"
                            ]
                        ]
                    ]
                ]
                Html.footer [
                    prop.className "card-footer"
                    prop.children [
                        Html.p [
                            prop.classes [ "card-footer-item"; "is-size-7"]

                            prop.text ((post.Category |> string).ToUpper())
                        ]
                        bulletIcon
                        Html.p [
                            prop.classes [ "card-footer-item"; "is-size-7"]
                            prop.text 
                                (
                                match post.Category with 
                                | Article -> sprintf "PUBLISHED %s" (summarizeDate post.Updated)
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
                    style.paddingRight 8
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
            prop.className "container"
            prop.children [
                Html.div [
                    prop.classes ["title"; "is-size-1"; "mb-5"; "mt-6"]
                    prop.text "The Garden"
                    prop.style [
                        style.fontWeight 550
                    ]
                    
                ]
                
                Html.div [
                    prop.classes ["subtitle"; "is-size-2"; "is-family-secondary"; "mb-6"]
                    prop.text "A collection of essays, notes, and half-baked explorations I'm always tending to."
                    prop.style [
                        style.fontWeight 300
                    ]
                ]
            ]
        ]
        
    let renderGrid (posts : Post seq) = 
        let renderedPosts = getPostSummaries posts

        Html.div [
            prop.className "container"
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
            prop.className "container"
            prop.children [
                homeHeader
                renderGrid posts
            ]
        ]
        