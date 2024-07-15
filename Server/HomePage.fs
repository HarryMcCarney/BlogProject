namespace blog

module Home = 
    open Feliz.StaticHtml
    open Layout
    open Model

    let getCategoryDropDown (posts: Post seq) = 

        let dropdownItems = 
            let all =  
                Html.a [
                    Attr.classes ["dropdown-item"]
                    Attr.id (sprintf "dropdown_all")
                    Html.span "All Types"  
                ]

            posts
            |> Seq.countBy(fun p -> p.Category)
            |> Seq.distinct
            |> Seq.map(fun (c, i) -> 
                Html.a [
                    Attr.classes ["dropdown-item"]
                    Attr.id (sprintf "dropdown_%s" (c.ToString()))
                    match c with 
                    | Essay -> essayIcon
                    | Note -> noteIcon
                    | Talk -> talkIcon
                    | Draft -> failwith "published a draft"
                    Html.span (sprintf "%s (%i)" (c.ToString()) i)
                ]
            )
            |> Seq.append  (seq {all})
            |> Seq.toList
               
        Html.div [
            Attr.classes ["dropdown";]
            Attr.id "category_dropdown"
            Html.div [
                Attr.classes ["dropdown-trigger"]
                Html.button [
                    Attr.id "dropdown_button"
                    Attr.classes ["button"]
                    Attr.ariaHasPopup true
                    Attr.ariaControls "dropdown-menu"
                    Html.span [
                        Attr.id "dropdown_button_text"
                        Html.text "All Types"
                    ]
                    Html.span [
                        Attr.classes ["icon"; "is-small"]
                        Html.i [
                            Attr.classes ["fas"; "fa-angle-down"]
                            Attr.ariaHidden true
                            Attr.style "color:#00d1b2"
                            ]
                    ]
                ]
                Html.div [
                    Attr.classes ["dropdown-menu"]
                    Attr.id "dropdown-menu"
                    Attr.role "menu"
                    Html.div [
                        Attr.classes ["dropdown-content"; "has-text-left"]
                        Fragment dropdownItems
                    ]
                ]
            ]
        ]
        

    let buildPostCard post = 
        Html.div [
            Attr.id post.FileName
            Attr.classes ["card";  "post-card"; "my-card"; "is-clickable"; "has-background-light"]
            match post.MainImage with 
            | Some i -> 
                Html.div [
                    Attr.classes ["card-image"]
                    Html.figure [
                        Attr.classes ["image"; "is4by3"]
                        Html.img [
                                Attr.src i
                            ]
                        ]
                    ]
            | _ -> ()

            Html.div [
                Attr.classes ["card-content"; "p-4"]
                match post.Category with 
                | Note -> noteIcon
                | Essay -> essayIcon
                | Talk -> talkIcon
                | _ -> failwith "unknown post category"

                Html.a [
                    Attr.classes  ["is-family-secondary"; "is-size-4"; "ml-3"; "has-text-dark"]
                    Attr.href (sprintf "/%s.html" post.FileName)
                    Html.text post.Title
                    Attr.style "font-weight: 400"
                    ]
                ]
            
            Html.footer [
                Attr.classes [ "card-footer"; "has-text-dark"]
                Html.p [
                    Attr.classes ["p-2"; "card-footer-item"; "is-size-7"; "has-text-right"]
                    Html.text ((post.Category |> string).ToUpper())
                ]
                Html.p [
                    Attr.classes ["p-2"; "card-footer-item"; "is-size-7"; "has-text-left"]
                    Html.text 
                        (match post.Category with 
                        | Essay -> sprintf "PUBLISHED %s" (summarizeDate post.Updated)
                        | Note -> sprintf "UPDATED %s" (summarizeDate post.Updated)
                        | _ -> sprintf "UPDATED %s" (summarizeDate post.Updated))
                    
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
                    Attr.id t
                    Attr.classes ["tag"; "is-hoverable"; "is-rounded"; "is-size-6"]
                    Html.text t
                ]
                )

        let topicLabel = 
            Html.span [
                Html.text "TOPICS"
                Attr.style """
                    border-style:solid;
                    border-right-width:2; 
                    border-bottom-width:0;
                    border-top-width:0;
                    border-left-width:0;
                    border-right-color:#00d1b2;
                    padding-right:20;
                    margin-right:10
                """
            ]

        Html.div [
            Attr.classes ["mb-6"]
            Fragment (tagList |> Seq.append [topicLabel] |> Seq.toList)
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
            Attr.classes [ "container";]
            Html.div [
                    Attr.classes ["is-size-1"; "mb-5"; "mt-6"; "has-text-dark"; "has-text-weight-bold"]
                    Html.text "Notes, Essays and Talks"  
                ]  
            Html.div [
                Attr.classes ["is-size-2"; "is-family-secondary"; "mb-6"; "has-text-dark"; "has-text-weight-light"]
                Html.text "A collection of ideas in varying stages of bakedness."
            ]
        ]
        
        
    let renderGrid (posts : Post seq) = 
        let renderedPosts = getPostSummaries posts

        Html.div [
            Attr.classes [ "container";]
            Html.div [
                Attr.classes ["columns"]
                Html.div [
                    Attr.classes ["column"; "is-four-fifths"]
                    (buildTagList posts) 
                ] 
                Html.div [
                    Attr.classes ["column"; "is-one-fifth" ;"has-text-right"]
                    (getCategoryDropDown posts)  
                ] 
                Html.div [
                    Attr.className "columns is-multiline"
                    Html.div [
                        Attr.className "column is-one-third"
                        Fragment (getColumnPosts renderedPosts 1 3 |> Seq.toList)
                    ]
                    Html.div [
                        Attr.className "column is-one-third"
                        Fragment (getColumnPosts renderedPosts 2 3 |> Seq.toList)
                    ]
                    Html.div [
                        Attr.className "column is-one-third"
                        Fragment (getColumnPosts renderedPosts 3 3 |> Seq.toList)
                    ]       
                ]
            ]     // Add more card columns here based on the structure
        ]
        
               
    let renderHomePage posts = 
        Html.div [
            Attr.classes [ "container";]
            homeHeader
            renderGrid posts
            
        ]
        