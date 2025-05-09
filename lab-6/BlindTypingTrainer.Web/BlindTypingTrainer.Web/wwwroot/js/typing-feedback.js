// wwwroot/js/typing-feedback.js

document.addEventListener("DOMContentLoaded", () => {
    const stageTextEl = document.getElementById("stageText");
    const input = document.getElementById("inputBox");
    const stageNumEl = document.getElementById("stageNum");
    const timerEl = document.getElementById("timer");
    const corrEl = document.getElementById("correct");
    const errEl = document.getElementById("errors");
    const finishBtn = document.getElementById("finish");
    const finishForm = document.getElementById("finishForm");
    const summaryEl = document.getElementById("summary");
    const totalTimeEl = document.getElementById("totalTime");
    const totalErrorsEl = document.getElementById("totalErrors");
    const wpmEl = document.getElementById("wpm");
    const accuracyEl = document.getElementById("accuracy");

    const stages = JSON.parse(document.getElementById("stagesData").value);

    let current = 0;
    let stageStart = null;
    let timerInterval = null;

    let globalStart = null;
    let totalElapsed = 0;       // загальний час в мс
    let totalErrors = 0;        // загальні помилки
    let totalTyped = 0;         // загальна кількість введених символів
    let totalCorrect = 0;       // загальна кількість правильних символів

    function pad(n) { return String(n).padStart(2, '0'); }

    function startTimer() {
        if (!globalStart) globalStart = Date.now();
        stageStart = Date.now();
        timerInterval = setInterval(() => {
            const delta = Date.now() - stageStart;
            timerEl.textContent = `${pad(Math.floor(delta / 60000))}:${pad(Math.floor((delta % 60000) / 1000))}`;
        }, 200);
    }

    function stopTimer() {
        clearInterval(timerInterval);
        totalElapsed += Date.now() - stageStart;
    }

    function renderStage(i) {
        const text = stages[i];
        stageTextEl.innerHTML = text.split('').map(ch => `<span>${ch}</span>`).join('');
        input.value = '';
        current = i;
        stageNumEl.textContent = i + 1;
        timerEl.textContent = '00:00';
        corrEl.textContent = '0';
        errEl.textContent = '0';
        finishBtn.style.display = 'none';
        stageStart = null;
        if (timerInterval) clearInterval(timerInterval);
        input.disabled = false;
        input.focus();
    }

    input.addEventListener("keydown", e => {
        const pos = input.value.length;
        const expected = stages[current][pos];

        if (e.key.length === 1 && !stageStart) {
            startTimer();
        }

        if (e.key !== 'Backspace' && e.key.length === 1) {
            totalTyped++;
            if (e.key !== expected) {
                totalErrors++;
                errEl.textContent = totalErrors;
            }
        }
    });

    input.addEventListener("input", () => {
        const spans = stageTextEl.querySelectorAll('span');
        const val = input.value;
        let correctCount = 0;

        spans.forEach((span, idx) => {
            const ch = val[idx] || '';
            if (ch === span.textContent) {
                span.classList.add('correct');
                span.classList.remove('wrong');
                correctCount++;
            } else {
                span.classList.add('wrong');
                span.classList.remove('correct');
            }
        });

        corrEl.textContent = correctCount;
        totalCorrect += (val.length === spans.length && correctCount === spans.length) ? correctCount : 0;

        if (correctCount === spans.length) {
            stopTimer();
            if (current + 1 < stages.length) {
                renderStage(current + 1);
            } else {
                // Всі етапи пройдені — показуємо підсумки
                input.disabled = true;
                finishBtn.style.display = 'inline-block';
                showSummary();
            }
        }
    });

    finishBtn.addEventListener('click', () => {
        finishForm.submit();
    });

    function showSummary() {
        // Загальний час mm:ss
        const mm = pad(Math.floor(totalElapsed / 60000));
        const ss = pad(Math.floor((totalElapsed % 60000) / 1000));
        totalTimeEl.textContent = `${mm}:${ss}`;

        totalErrorsEl.textContent = totalErrors;

        // WPM = (totalTyped/5) / (totalElapsed_minutes)
        const minutes = totalElapsed / 60000;
        const wpm = minutes > 0 ? Math.round((totalTyped / 5) / minutes) : 0;
        wpmEl.textContent = wpm;

        // Точність в %
        const accuracy = totalTyped > 0
            ? Math.round((totalCorrect / totalTyped) * 100)
            : 0;
        accuracyEl.textContent = `${accuracy}%`;

        summaryEl.style.display = 'block';
    }

    if (stages.length) renderStage(0);
});
