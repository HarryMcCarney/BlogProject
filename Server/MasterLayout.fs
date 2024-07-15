namespace blog

module MasterLayout =   
    open Feliz.ViewEngine
    open Layout

    let render content =
        [   
            Html.html [
                prop.custom("data-theme", "light")

            ]
            Html.header [
                
                Html.meta [
                    prop.name "viewport"
                    prop.content "width=device-width, initial-scale=1"
                ]

                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://cdn.jsdelivr.net/npm/bulma@1.0.0/css/bulma.min.css"
                ]
                Html.script [
                    prop.src "https://kit.fontawesome.com/fd17b6d7c8.js"
                    prop.crossOrigin.anonymous
                ]
                Html.link [
                    prop.rel "stylesheet"
                    prop.href "styles.css"
                ]

                Html.link [
                    prop.rel "preconnect"
                    prop.href "https://fonts.googleapis.com"
                ]

                Html.link [
                    prop.rel "preconnect"
                    prop.href "https://fonts.gstatic.com"
                ]

                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://fonts.googleapis.com/css2?family=Yrsa:ital,wght@0,300..700;1,300..700&display=swap"
                ]

                Html.link [
                    prop.rel "stylesheet"
                    prop.href "https://fonts.googleapis.com/css2?family=Merriweather:ital,wght@0,300;0,400;0,700;0,900;1,300;1,400;1,700;1,900&display=swap"
                ]

                Html.script [
                    prop.src "Script.js"
                    prop.type' "module"
                ]
            ]  

            Html.body [
                prop.classes ["has-background-light"; "has-text-dark"]
                prop.custom("onLoad", "execScripts()") 
                prop.style [
                    style.overflowX.hidden
                    style.width (length.percent 100)
                    style.custom ("--family-serif", "'Yrsa', serif")
                    style.custom ("--family-secondary-serif", "'Merriweather', serif")
                    style.custom ("--family-primary", "var(--family-serif)")
                    style.custom ("--family-secondary", "var(--family-secondary-serif)")
                    style.fontFamily "var(--family-primary)"
                    style.fontFamily "var(--family-secondary)"
                ]
                prop.children [
                    Html.section [
                        prop.classes ["container"; "is-fluid"]
                        prop.children [
                            navbar
                            content
                            footer

                        ]
                    ]

                ]
            ]
        ]
        |> Render.htmlView
  
   


 

        




    

    
    