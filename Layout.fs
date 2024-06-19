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
(*
    let footer =

        Html.div [
            prop.classes ["footer"]
            prop.style [
                style.display.flex
                style.alignItems.center
                style.justifyContent.spaceBetween
            ]
            prop.children [
                Html.div [
                    prop.classes ["content has-text-centered"]
                    prop.children [
                        Html.p [
                            prop.text "Some blog" 
                            prop.style [
                                style.fontWeight.bold
                            ]
                            Html.a [
                                prop.text "© Hack and Craft 2023"
                                prop.href "https://hackandcraft.com/"
                                prop.target "_blank"
                            ]
                        ]
                        
                    ]
                ]
            ]
        ]
*)
    let footer = 
        Html.footer [
            prop.className "footer"
            prop.children [
                Html.div [
                    prop.className "content has-text-centered"
                    prop.children [
                        Html.p [
                            Html.strong "Bulma"
                            Html.text " by "
                            Html.a [
                                prop.href "https://jgthms.com"
                                prop.text "Jeremy Thomas"
                            ]
                            Html.text ". The source code is licensed "
                            Html.a [
                                prop.href "http://opensource.org/licenses/mit-license.php"
                                prop.text "MIT"
                            ]
                            Html.text ". The website content is licensed "
                            Html.a [
                                prop.href "http://creativecommons.org/licenses/by-nc-sa/4.0/"
                                prop.text "CC BY NC SA 4.0"
                            ]
                            Html.text "."
                        ]
                    ]
                ]
            ]
        ]
    


    let menu = 
        Html.aside [
            prop.className "menu"
            prop.children [
                Html.p [
                    prop.className "menu-label"
                    prop.text "General"
                ]
                Html.ul [
                    prop.className "menu-list"
                    prop.children [
                        Html.li [ Html.a [ prop.text "Dashboard" ] ]
                        Html.li [ Html.a [ prop.text "Customers" ] ]
                    ]
                ]
                Html.p [
                    prop.className "menu-label"
                    prop.text "Administration"
                ]
                Html.ul [
                    prop.className "menu-list"
                    prop.children [
                        Html.li [ Html.a [ prop.text "Team Settings" ] ]
                        Html.li [
                            Html.a [
                                prop.className "is-active"
                                prop.text "Manage Your Team"
                            ]
                            Html.ul [
                                Html.li [ Html.a [ prop.text "Members" ] ]
                                Html.li [ Html.a [ prop.text "Plugins" ] ]
                                Html.li [ Html.a [ prop.text "Add a member" ] ]
                            ]
                        ]
                        Html.li [ Html.a [ prop.text "Invitations" ] ]
                        Html.li [ Html.a [ prop.text "Cloud Storage Environment Settings" ] ]
                        Html.li [ Html.a [ prop.text "Authentication" ] ]
                    ]
                ]
                Html.p [
                    prop.className "menu-label"
                    prop.text "Transactions"
                ]
                Html.ul [
                    prop.className "menu-list"
                    prop.children [
                        Html.li [ Html.a [ prop.text "Payments" ] ]
                        Html.li [ Html.a [ prop.text "Transfers" ] ]
                        Html.li [ Html.a [ prop.text "Balance" ] ]
                    ]
                ]
            ]
        ]