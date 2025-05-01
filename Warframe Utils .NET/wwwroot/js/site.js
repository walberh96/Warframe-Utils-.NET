// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const themeToggle = document.getElementById("theme-toggle");
const userPref = localStorage.getItem("theme");
const systemPrefDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

function applyTheme(theme) {
    const body = document.body;
    const navbar = document.getElementById("main-navbar");

    body.classList.add("transition");

    if (theme === "dark") {
        body.classList.add("dark");
        navbar.classList.remove("navbar-light", "bg-white");
        navbar.classList.add("navbar-dark", "bg-dark");
    } else {
        body.classList.remove("dark");
        navbar.classList.remove("navbar-dark", "bg-dark");
        navbar.classList.add("navbar-light", "bg-white");
    }
}

function currentTheme() {
    return document.body.classList.contains("dark") ? "dark" : "light";
}

// Aplica el tema en base a preferencia o sistema
if (userPref) {
    applyTheme(userPref);
} else {
    applyTheme(systemPrefDark ? "dark" : "light");
}

// Cambiar tema al hacer click
themeToggle.addEventListener("click", () => {
    const isDark = currentTheme() === "dark";
    const newTheme = isDark ? "light" : "dark";
    applyTheme(newTheme);
    localStorage.setItem("theme", newTheme);
});


