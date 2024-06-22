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


    let renderHomePage posts = 
        let postSummaries = getPostSummaries posts
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
                            prop.children 
                                postSummaries
                        ]
                    ]
                ]     
            ]
        ]
        
