// wwwroot/js/site.js

document.addEventListener("DOMContentLoaded", function () {
    // Для всіх <input>, крім type="email" і type="password"
    document.querySelectorAll('input:not([type="email"]):not([type="password"])')
        .forEach(function (el) {
            el.setAttribute("autocomplete", "off");
        });

    // Для всіх <textarea>
    document.querySelectorAll("textarea")
        .forEach(function (el) {
            el.setAttribute("autocomplete", "off");
        });
});