namespace blog

module Home = 
    open Feliz.ViewEngine
    open Layout
    open Model

    let buildPostCard post = 

        Html.div [
            prop.className "card"
            prop.children [
                Html.header [
                    prop.className "card-header"
                    prop.children [
                        Html.p [
                            prop.className "card-header-title"
                            prop.text post.Title
                        ]
                        Html.button [
                            prop.className "card-header-icon"
                            prop.ariaLabel "more options"
                            prop.children [
                                Html.span [
                                    prop.className "icon"
                                    prop.children [
                                        Html.i [
                                            prop.className "fas fa-angle-down"
                                            prop.ariaHidden true
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
                Html.div [
                    prop.className "card-content"
                    prop.children [
                        Html.div [
                            prop.className "content"
                            prop.children [
                                Html.text "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus nec iaculis mauris."
                                Html.a [
                                    prop.href "#"
                                    prop.text "@bulmaio"
                                ]
                                Html.text ". "
                                Html.a [
                                    prop.href "#"
                                    prop.text "#css"
                                ]
                                Html.text " "
                                Html.a [
                                    prop.href "#"
                                    prop.text "#responsive"
                                ]
                                Html.br []
                                Html.time [
                                    prop.dateTime "2016-1-1"
                                    prop.text "11:09 PM - 1 Jan 2016"
                                ]
                            ]
                        ]
                    ]
                ]
                Html.footer [
                    prop.className "card-footer"
                    prop.children [
                        Html.a [
                            prop.href "#"
                            prop.className "card-footer-item"
                            prop.text "Save"
                        ]
                        Html.a [
                            prop.href "#"
                            prop.className "card-footer-item"
                            prop.text "Edit"
                        ]
                        Html.a [
                            prop.href "#"
                            prop.className "card-footer-item"
                            prop.text "Delete"
                        ]
                    ]
                ]
            ]
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
                ]
                        // Add more card columns here based on the structure
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
        