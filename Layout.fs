namespace blog

open Feliz.ViewEngine 

module Layout = 
    let navbar =
        Html.nav [
            prop.classes ["navbar"; "is-fluid"; "is-primary"]
            prop.role "navigation"
            prop.ariaLabel "main navigation"
            prop.children [
                Html.a [
                prop.classes ["navbar-start"]
                prop.text "Home"
                prop.children [
                    Html.a [
                        prop.classes ["navbar-item"]
                        prop.text "Notes"
                    ]
                    Html.a [
                        prop.classes ["navbar-item"]
                        prop.text "Articles"
                    ]
                    Html.a [
                        prop.classes ["navbar-item";"navbar-end"]
                        prop.text "About"
                    ]

                ]
                ]

            ]
        ]

