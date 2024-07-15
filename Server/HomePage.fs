namespace blog

module Home = 
    open Feliz.ViewEngine
    open Layout
    open Model
    open Feliz.ViewEngine.style

    let getCategoryDropDown (posts: Post seq) = 

        let dropdownItems = 

            let all =  
                Html.a [
                    prop.classes ["dropdown-item"]
                    prop.id (sprintf "dropdown_all")
                    prop.children [
                        Html.span [
                            prop.text "All Types"
                        ]
                    ]
                ]

            posts
            |> Seq.countBy(fun p -> p.Category)
            |> Seq.distinct
            |> Seq.map(fun (c, i) -> 
                Html.a [
                    prop.classes ["dropdown-item"]
                    prop.id (sprintf "dropdown_%s" (c.ToString()))
                    prop.children [
                        match c with 
                        | Essay -> essayIcon
                        | Note -> noteIcon
                        | Talk -> talkIcon
                        | Draft -> failwith "published a draft"
                        Html.span [
                            prop.text (sprintf "%s (%i)" (c.ToString()) i)
                        ]

                    ]
                ]
            )
            |> Seq.append  (seq {all})
               

        Html.div [
            prop.classes ["dropdown";]
            prop.id "category_dropdown"
            prop.children [
                Html.div [
                    prop.classes ["dropdown-trigger"]
                    prop.children [
                        Html.button [
                            prop.id "dropdown_button"
                            prop.classes ["button"]
                            prop.ariaHasPopup true
                            prop.ariaControls "dropdown-menu"
                            prop.children [
                                Html.span [
                                    prop.id "dropdown_button_text"
                                    prop.text "All Types"
                                ]
                                Html.span [
                                    prop.classes ["icon"; "is-small"]
                                    prop.children [
                                        Html.i [
                                            prop.classes ["fas"; "fa-angle-down"]
                                            prop.ariaHidden true
                                            prop.style [
                                                style.color "#00d1b2"
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
                Html.div [
                    prop.classes ["dropdown-menu"]
                    prop.id "dropdown-menu"
                    prop.role "menu"
                    prop.children [
                        Html.div [
                            prop.classes ["dropdown-content"; "has-text-left"]
                            prop.children dropdownItems
                        ]
                    ]
                ]
            ]
        ]

    let buildPostCard post = 
        Html.div [
            prop.id post.FileName
            prop.classes ["card";  "post-card"; "my-card"; "is-clickable"; "has-background-light"]
  
            prop.children [
                match post.MainImage with 
                | Some i -> 
                    Html.div [
                        prop.classes ["card-image"]
                        prop.children [
                            Html.figure [
                                prop.classes ["image"; "is4by3"]
                                prop.children [
                                    Html.img [
                                        prop.src i
                                    ]
                                ]
                            ]
                        ]
                    ]
                | _ -> ()

                Html.div [
                    prop.classes ["card-content"; "p-4"]
                    prop.children [
                        match post.Category with 
                        | Note -> noteIcon
                        | Essay -> essayIcon
                        | Talk -> talkIcon
                        | _ -> failwith "unknown post category"

                        Html.a [
                            prop.classes  ["is-family-secondary"; "is-size-4"; "ml-3"; "has-text-dark"]
                            prop.href (sprintf "/%s.html" post.FileName)
                            prop.text post.Title
                            prop.style [
                                style.fontWeight 400
                            ]
                        ]
                    ]
                ]
                Html.footer [
                    prop.classes [ "card-footer"; "has-text-dark"]
                    prop.children [
                        Html.p [
                            prop.classes ["p-2"; "card-footer-item"; "is-size-7"; "has-text-right"]

                            prop.text ((post.Category |> string).ToUpper())
                        ]
                    
                        Html.p [
                            prop.classes ["p-2"; "card-footer-item"; "is-size-7"; "has-text-left"]
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
        |> Seq.sortBy(fun p -> p.Title)
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
                    prop.classes ["is-size-1"; "mb-5"; "mt-6"; "has-text-dark"; "has-text-weight-bold"]
                    prop.text "Notes, Essays and Talks"  
                ]
                
                Html.div [
                    prop.classes ["is-size-2"; "is-family-secondary"; "mb-6"; "has-text-dark"; "has-text-weight-light"]
                    prop.text "A collection of ideas in varying stages of bakedness."
                ]
            ]
        ]
        
    let renderGrid (posts : Post seq) = 
        let renderedPosts = getPostSummaries posts

        Html.div [
            prop.classes [ "container";]
            prop.children [
                Html.div [
                    prop.classes ["columns"]
                    prop.children [
                        Html.div [
                            prop.classes ["column"; "is-four-fifths"]
                            prop.children (buildTagList posts)   
                        ] 
                        Html.div [
                            prop.classes ["column"; "is-one-fifth" ;"has-text-right"]
                            prop.children (getCategoryDropDown posts)  
                        ] 
                    ]                        
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
        