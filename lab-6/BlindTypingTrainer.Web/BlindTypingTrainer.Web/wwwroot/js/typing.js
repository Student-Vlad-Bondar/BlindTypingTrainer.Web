// wwwroot/js/typing.js

document.addEventListener("DOMContentLoaded", () => {
    const stageTextEl = document.getElementById("stageText");
    const input = document.getElementById("inputBox");
    const stageNumEl = document.getElementById("stageNum");
    const finishBtn = document.getElementById("finish");

    // отримуємо масив етапів, заповнений в Index.cshtml
    const stages = window['typingStages'] || [];
    let current = 0;

    function loadStage(i) {
        stageTextEl.textContent = stages[i];
        input.value = "";
        stageNumEl.textContent = i + 1;
        input.disabled = false;
        input.focus();
    }

    input.addEventListener("keydown", e => {
        const pos = input.value.length;
        const expected = stages[current][pos];
        if (e.key === "Backspace") return;
        if (e.key.length === 1 && e.key !== expected) {
            e.preventDefault();
        }
    });

    input.addEventListener("input", () => {
        const val = input.value;
        const text = stages[current];
        let html = "";
        for (let i = 0; i < text.length; i++) {
            const ch = val[i] || "";
            if (ch === text[i]) {
                html += `<span class="correct">${text[i]}</span>`;
            } else {
                html += `<span class="wrong">${text[i]}</span>`;
            }
        }
        stageTextEl.innerHTML = html;

        if (val.length === text.length && !html.includes('class="wrong"')) {
            current++;
            if (current < stages.length) {
                loadStage(current);
            } else {
                input.disabled = true;
                finishBtn.style.display = "inline-block";
            }
        }
    });

    finishBtn.addEventListener("click", () => {
        const form = document.createElement("form");
        form.method = "POST";
        form.action = "/Typing/Finish";
        form.append(Object.assign(document.createElement("input"), {
            type: "hidden", name: "sessionId", value: document.getElementById("sessionId").value
        }));
        document.body.append(form);
        form.submit();
    });

    // Старт першого етапу
    if (stages.length) loadStage(0);
});
