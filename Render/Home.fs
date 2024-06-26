namespace blog

module Home = 
    open Feliz.ViewEngine
    open Layout
    open Model
    open System



    let buildPostCard post = 

        Html.div [
            prop.id post.FileName
            prop.classes ["card" ; "post-card"]
            prop.children [
                Html.div [
                    prop.classes ["card-content"]
                    prop.children [
                        match post.Category with 
                                | Article ->  articleIcon
                                | Note -> noteIcon
                                | _ -> noteIcon
                        Html.a [
                            prop.className "subtitle"
                            prop.href (sprintf "/%s.html" post.FileName)
                            prop.text post.Title
                        ]
                    ]
                ]
                Html.footer [
                    prop.className "card-footer"
                    prop.children [
                        Html.p [
                            prop.className "card-footer-item"
                            prop.text (post.Category |> string)
                        ]
                        bulletIcon
                        Html.p [
                            prop.className "card-footer-item"
                            prop.text 
                                (
                                match post.Category with 
                                | Article -> sprintf "Published %s" (summarizeDate post.Updated)
                                | Note -> sprintf "Updated %s" (summarizeDate post.Updated)
                                | _ -> sprintf "Updated %s" (summarizeDate post.Updated)
                                )
                        ]
                    ]
                ]
            ]
        ]
        
    let buildTagList (posts : Post seq) = 
        posts
        |> Seq.map(fun p -> p.Tags)
        |> Seq.concat
        |> Set.ofSeq
        |> Set.toSeq
        |> Seq.map(fun t ->
            Html.span [
                prop.id t
                prop.classes ["tag"; "is-hoverable"; "is-medium"]
                prop.text t
            ]
        )

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
        Html.section [
            prop.className "section"
            prop.children [
                Html.h1 [
                    prop.className "title"
                    prop.text "The Garden"
                    
                ]
                
                Html.h2 [
                    prop.className "subtitle"
                    prop.text "A collection of essays, notes, and half-baked explorations I'm always tending to."
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
        