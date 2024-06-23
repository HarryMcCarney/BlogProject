



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


[<Emit("window.execScripts = $0")>]
let exportexecScripts (greet: obj) = jsNative

let execScripts() =
    expandHamburger()

exportexecScripts execScripts



(*
function expandHamburger() {
    const burger = document.getElementById("navbar-burger");
    const menu = document.getElementById("navbarBasicExample");
    burger.addEventListener("click", (_arg) => {
        burger.classList.toggle("is-active");
        menu.classList.toggle("is-active");
    });
}
*)