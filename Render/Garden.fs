namespace blog

module Garden = 
    open Feliz.ViewEngine

    let getGarden() = 

        Html.div [
            prop.className "container"
            prop.children [
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
              
                Html.div [
                    prop.className "columns is-multiline"
                    prop.children [
                        Html.div [
                            prop.className "column is-one-third"
                            prop.children [
                                Html.div [
                                    prop.className "card"
                                    prop.children [
                                        Html.div [
                                            prop.className "card-content"
                                            prop.children [
                                                Html.div [
                                                    prop.className "content"
                                                    prop.children [
                                                        Html.h3 [
                                                            prop.className "title is-4"
                                                            prop.text "Spinning Worlds, Seasickness, and Dealing with Vestibular Neuritis"
                                                        ]
                                                        Html.p [
                                                            prop.text "Note about 11 hours ago"
                                                        ]
                                                    ]
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                        Html.div [
                            prop.className "column is-one-third"
                            prop.children [
                                Html.div [
                                    prop.className "card"
                                    prop.children [
                                        Html.div [
                                            prop.className "card-content"
                                            prop.children [
                                                Html.div [
                                                    prop.className "content"
                                                    prop.children [
                                                        Html.h3 [
                                                            prop.className "title is-4"
                                                            prop.text "Speculative Calendar Events"
                                                        ]
                                                        Html.p [
                                                            prop.text "Note 2 months ago"
                                                        ]
                                                    ]
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                        // Add more card columns here based on the structure
                    ]
                ]
               
            ]
        ]