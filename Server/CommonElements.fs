namespace blog

open Feliz.StaticHtml
open System

module Layout = 

    let summarizeDate (inputDate: DateTime) =
        let now = DateTime.UtcNow
        let span = now - inputDate
        
        if span.TotalHours < 24.0 then
            sprintf "%.0f HOURS AGO" span.TotalHours
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
            Attr.classes ["icon"; "is-medium";"is-primary"]
            Html.i [
                Attr.classes ["fa-regular"; "fa-circle"; "is-primary"]
                Attr.style "color:#00d1b2"
                ]
        ]
            
    let essayIcon = 
        Html.span [
            Attr.classes ["icon"; "is-medium";"is-primary"]
            Html.i [
                Attr.classes ["fa-solid"; "fa-circle"; "is-primary"]
                Attr.style "color:#00d1b2"
                ]
        ]    
    
    let talkIcon = 
        Html.span [
            Attr.classes ["icon"; "is-medium";"is-primary"]
            Html.i [
                Attr.classes ["fa-sharp"; "fa-solid"; "fa-square";  "is-primary"]
                Attr.style "color:#00d1b2"
                ]
        ]    

    let navbar =
        Html.nav [
            Attr.classes ["navbar";"is-fluid"; "has-text-dark"; "has-background-light"]
            Attr.role "navigation"
            Attr.ariaLabel "main navigation"

            Html.div [
                Attr.className "navbar-brand"
                Html.a [
                    Attr.id "navbar-burger"
                    Attr.classes ["navbar-burger"; "is-text"; "has-text-dark";]
                    Attr.role "button"
                    Attr.ariaLabel "menu"
                    Attr.ariaExpanded false
                    Attr.custom ("data-target", "navbarBasicExample")
                    
                    Html.span [ Attr.ariaHidden true ]
                    Html.span [ Attr.ariaHidden true ]
                    Html.span [ Attr.ariaHidden true ]
                    Html.span [ Attr.ariaHidden true ]
                ]
            ]
                
            Html.div [
                Attr.id "navbarBasicExample"
                Attr.classes ["navbar-menu"; "navbar-end"]
                Html.a [
                    Attr.classes ["navbar-item";]
                    Html.text "About"
                    Attr.href "about.html"
                ]

            ]
        ]
            
    let customLinkStyle = 
        Attr.style "color:#363636; text-decoration:underline; font-weight:bold; transition-duration:0.3s; transition-property:color; transition-timing-function:ease"
    
    let customLinkHoverStyle = 
        Attr.style "color:#363636"

    let footer = 
        Html.footer [
            Attr.classes [ "footer"; "has-text-dark"; "has-background-light"]
            Html.div [
                Attr.classes ["content"; "has-text-centered"]
                Html.p [
                    Fragment [
                        Html.strong "Content"
                        Html.ins " by "
                        Html.a [
                            customLinkStyle
                            Attr.href "https://defcon.social/@HarryMcCarney"
                            Html.text "Harry McCarney"
                        ]
                        Html.ins [
                            Html.text ". The source code is licensed "
                            Attr.style "text-decoration: none"]
                        Html.a [
                            Attr.href "http://opensource.org/licenses/mit-license.php"
                            Html.text "MIT"
                        ]
                        Html.ins [
                            Html.text ". The website content is licensed "
                            Attr.style "text-decoration: none"]
                        Html.a [
                            Attr.href "http://creativecommons.org/licenses/by-nc-sa/4.0/"
                            Html.text "CC BY NC SA 4.0."
                        ]
                        
                    ]
                ]
            ]
        ]
      

    