namespace blog

module Post = 
    open Feliz.ViewEngine
    open Model


    let renderPost (post: Post) = 

        Html.div [ 
            prop.classes ["container"; "content"; "is-medium"]
            prop.style [
                style.backgroundColor "#F3F3F3"
            ]
            prop.children [
                Html.section [
                    prop.className "section"
                    prop.children [
                        Html.div [
                            prop.classes ["title";"is-2"]
                            prop.text post.Title
                        ]
                    ]
                ]
                Html.div [
                    prop.dangerouslySetInnerHTML post.Content
                ]
            ]
        ]
            
    