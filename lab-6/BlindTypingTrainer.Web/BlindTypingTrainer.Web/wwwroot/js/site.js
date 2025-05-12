// wwwroot/js/site.js

document.addEventListener("DOMContentLoaded", function () {
    // Для всех <input>, кроме type="email" и type="password"
    document.querySelectorAll('input:not([type="email"]):not([type="password"])')
        .forEach(function (el) {
            el.setAttribute("autocomplete", "off");
        });

    // Для всех <textarea>
    document.querySelectorAll("textarea")
        .forEach(function (el) {
            el.setAttribute("autocomplete", "off");
        });
});