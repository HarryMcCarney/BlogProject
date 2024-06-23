
namespace blog
module JS =

    open Browser.Dom

    let expandHamburger() =
        let burger = document.getElementById(".navbar-burger")
        let menu = document.getElementById(".navbar-menu")

        burger.addEventListener("click", fun _ ->
            burger.toggleAttribute("is-active") 
            |> ignore
            menu.toggleAttribute("is-active") 
            |> ignore 
        )