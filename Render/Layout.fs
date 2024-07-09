namespace blog

open Feliz.ViewEngine 
open System

module Layout = 

    let summarizeDate (inputDate: DateTime) =
        let now = DateTime.UtcNow
        let span = now - inputDate
        
        if span.TotalHours < 24.0 then
            sprintf "%.0f HOURS AGP" span.TotalHours
        elif span.TotalDays < 30.0 then
            sprintf "%.0f DAYS AGO" span.TotalDays
        elif span.TotalDays < 365.0 then
            let months = Math.Round(span.TotalDays / 30.0)
            sprintf "ABOUT %.0f MONTHS AGO" months
        else
            let years = Math.Round(span.TotalDays / 365.0)
            sprintf "ABOUT %.0f YEARS AGO" years

    
    let noteIcon = 
        Html.span [
                prop.classes ["icon"; "is-medium";"is-primary"]
                prop.children [
                    Html.i [
                        prop.classes ["fa-regular"; "fa-circle"; "is-primary"]
                        prop.style [
                            style.color "#00d1b2"
                        ]
                    ]
                
                ]
            ]
    
    let essayIcon = 
        Html.span [
                prop.classes ["icon"; "is-medium"]
                prop.children [
                    Html.i [
                        prop.classes ["fa-solid"; "fa-circle"; "is-primary"]
                        prop.style [
                            style.color "#00d1b2"
                        ]
                    ]
                ]
            ]
    
    let talkIcon = 
        Html.span [
                prop.classes ["icon"; "is-medium"]
                prop.children [
                    Html.i [
                        prop.classes ["fa-sharp"; "fa-solid"; "fa-square"; "is-primary"]
                        prop.style [
                            style.color "#00d1b2"
                        ]
                    ]
                ]
            ]
     

    let navbar =
        Html.nav [
            prop.classes ["navbar";"is-fluid"; "has-text-dark"; "has-background-light"]
            prop.role "navigation"
            prop.ariaLabel "main navigation"

            prop.children [
                Html.div [
                    prop.className "navbar-brand"
                    prop.children [
                        Html.a [
                            prop.id "navbar-burger"
                            prop.classes ["navbar-burger"; "is-text"; "has-text-dark";]

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
            prop.classes [ "footer"; "has-text-dark"; "has-background-light"]
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
    


    