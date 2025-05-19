// wwwroot/js/achievements.js
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".badge-card").forEach(card => {
        card.addEventListener("mouseover", () => {
            card.classList.add("glow");
        });
        card.addEventListener("mouseout", () => {
            card.classList.remove("glow");
        });
    });
});
