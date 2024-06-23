namespace blog

module Home = 
    open Feliz.ViewEngine
    open Layout
    open Model
    open System

    let summarizeDate (inputDate: DateTime) =
        let now = DateTime.UtcNow
        let span = now - inputDate
        
        if span.TotalHours < 24.0 then
            sprintf "%.0f hours ago" span.TotalHours
        elif span.TotalDays < 30.0 then
            sprintf "%.0f days ago" span.TotalDays
        elif span.TotalDays < 365.0 then
            let months = Math.Round(span.TotalDays / 30.0)
            sprintf "about %.0f months ago" months
        else
            let years = Math.Round(span.TotalDays / 365.0)
            sprintf "about %.0f years ago" years


    let icon = 
        Html.span [
            prop.classes ["icon"; "is-small"; "has-text-danger"]
            prop.children [
                Html.i [
                    prop.classes ["fas"; "fa-music"; "fa-fw"]
                ]
            ]
        ]

    let buildPostCard post = 

        Html.div [
            prop.className "card"
            prop.children [
                Html.div [
                    prop.classes ["card-content"]
                    prop.children [
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
                        Html.p [
                            prop.className "card-footer-item"
                            prop.text (summarizeDate post.Updated)
                            prop.children [
                                icon
                            ]
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
                    prop.children [icon]
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
        