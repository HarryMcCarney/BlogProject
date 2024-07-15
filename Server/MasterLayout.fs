namespace blog

module MasterLayout =   
    open Feliz.StaticHtml
    open Layout

    let render content =
        [   
            Html.html [
                Attr.custom("data-theme", "light")

            ]
            Html.header [
                
                Html.metadata [
                    Attr.name "viewport"
                    Attr.content "width=device-width, initial-scale=1"
                ]

                Html.anchor [ // should be link not anchor
                    Attr.rel "stylesheet"
                    Attr.href "https://cdn.jsdelivr.net/npm/bulma@1.0.0/css/bulma.min.css"
                ]
                Html.anchor [
                    Attr.src "https://kit.fontawesome.com/fd17b6d7c8.js"
                    Attr.custom("crossOrigin", "anonymous")
                ]
                Html.anchor [
                    Attr.rel "stylesheet"
                    Attr.href "styles.css"
                ]

                Html.anchor [
                    Attr.rel "preconnect"
                    Attr.href "https://fonts.googleapis.com"
                ]

                Html.anchor [
                    Attr.rel "preconnect"
                    Attr.href "https://fonts.gstatic.com"
                ]

                Html.anchor [
                    Attr.rel "stylesheet"
                    Attr.href "https://fonts.googleapis.com/css2?family=Yrsa:ital,wght@0,300..700;1,300..700&display=swap"
                ]

                Html.anchor [
                    Attr.rel "stylesheet"
                    Attr.href "https://fonts.googleapis.com/css2?family=Merriweather:ital,wght@0,300;0,400;0,700;0,900;1,300;1,400;1,700;1,900&display=swap"
                ]

                Html.script [
                    Attr.src "Script.js"
                    Attr.type' "module"
                ]
            ]  

            Html.body [
                Attr.classes ["has-background-light"; "has-text-dark"]
                Attr.custom("onLoad", "execScripts()") 
                Attr.style """ 
                    overflow-x:hidden; width:100%; 
                    --family-serif:'Yrsa', serif; 
                    --family-secondary-serif: 'Merriweather', serif;
                    --family-primary: var(--family-serif);
                    --family-secondary: var(--family-secondary-serif);
                    var(--family-primary);
                    var(--family-secondary)
                    """

                Html.section [
                    Attr.classes ["container"; "is-fluid"]
                    navbar
                    content
                    footer
                ]
            ] 
        ]
        |> toString
  
   


 

        




    

    
    