
export function expandHamburger() {
    const burger = document.getElementById("navbar-burger");
    const menu = document.getElementById("navbarBasicExample");
    burger.addEventListener("click", (_arg) => {
        burger.classList.toggle("is-active");
        menu.classList.toggle("is-active");
    });
}

export function execScripts() {
    expandHamburger();
}

window.execScripts = (() => {
    execScripts();
});

