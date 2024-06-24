namespace blog

open Feliz.ViewEngine 
open System

module Layout = 
    let navbar =
        Html.nav [
            prop.classes ["navbar";]
            prop.role "navigation"
            prop.ariaLabel "main navigation"
            prop.style [
                style.backgroundColor "#F3F3F3"
                ]
            prop.children [
                Html.div [
                    prop.className "navbar-brand"
                    prop.children [
                        Html.a [
                            prop.id "navbar-burger"
                            prop.className "navbar-burger"
                            prop.role "button"
                            prop.ariaLabel "menu"
                            prop.ariaExpanded false
                            prop.custom ("data-target", "navbarBasicExample")
                            prop.children [
                                Html.span [ prop.ariaHidden true ]
                                Html.span [ prop.ariaHidden true ]
                                Html.span [ prop.ariaHidden true ]
                                Html.span [ prop.ariaHidden true ]
                            ]
                        ]
                    ]
                ]
                Html.div [
                    prop.id "navbarBasicExample"
                    prop.classes ["navbar-menu"; "navbar-end"]
                    prop.children [
                        Html.a [
                            prop.classes ["navbar-item";]
                            prop.text "About"
                            prop.href "about.html"
                        ]

                    ]
                ]
            ]
        ]

    let customLinkStyle = [
        style.color "#363636"
        style.textDecoration.underline
        style.fontWeight.bold
        style.transitionDuration (TimeSpan.FromSeconds(0.3))
        style.transitionProperty "color"
        style.transitionTimingFunction.ease
        
    ]

    let customLinkHoverStyle = [
            style.color "#363636"
        ]
        
    let footer = 
        Html.footer [
            prop.className "footer"
            prop.style [
                style.backgroundColor "#F3F3F3"
                ]
            prop.children [
                Html.div [
                    prop.className "content has-text-centered"
                    prop.children [
                        Html.p [
                            Html.strong "Content"
                            Html.text " by "
                            Html.a [
                                prop.style customLinkStyle
                                prop.href "https://mastodon.sdf.org/@HarryMcCarnney"
                                prop.text "Harry McCarney"
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
    


    