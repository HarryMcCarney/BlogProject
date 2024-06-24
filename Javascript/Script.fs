



open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop

let expandHamburger() =
    let burger = document.getElementById("navbar-burger")
    let menu = document.getElementById("navbarBasicExample")

    burger.addEventListener("click", fun _ ->
        burger.classList.toggle("is-active") 
        |> ignore
        menu.classList.toggle("is-active") 
        |> ignore 
    )

(*
let addTagFilters() =

    let nodes = document.getElementsByClassName("tag")
    let tags = seq { for i in 0 .. nodes.length - 1 -> nodes.[i] }
    let postNodes = document.getElementsByClassName("post-card")
    let posts = seq { for i in 0 .. postNodes.length - 1 -> postNodes.[i] }

    tags
    |> Seq.iter(fun t -> 
        t.addEventListener("click", fun _ ->



            t.classList.toggle("is-active") 
            |> ignore
        )
    )
    
  *)  
      

[<Emit("window.execScripts = $0")>]
let exportexecScripts (greet: obj) = jsNative

let execScripts() =
    expandHamburger()

exportexecScripts execScripts


