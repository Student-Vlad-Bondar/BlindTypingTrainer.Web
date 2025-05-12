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
    let totalElapsed = 0;   // мілісекунди
    let totalErrors = 0;
    let totalTyped = 0;
    let totalCorrect = 0;

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
        stageTextEl.innerHTML = text
            .split('')
            .map(ch => `<span>${ch}</span>`)
            .join('');
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
        if (correctCount === spans.length) {
            totalCorrect += correctCount;
        }

        if (correctCount === spans.length) {
            stopTimer();
            if (current + 1 < stages.length) {
                renderStage(current + 1);
            } else {
                input.disabled = true;
                showSummary();
            }
        }
    });

    finishBtn.addEventListener('click', () => {
        // Заповнюємо сховані поля і відправляємо форму
        document.getElementById("correctInput").value = totalCorrect;
        document.getElementById("errorsInput").value = totalErrors;
        finishForm.submit();
    });

    function showSummary() {
        // Загальний таймер mm:ss
        const mm = pad(Math.floor(totalElapsed / 60000));
        const ss = pad(Math.floor((totalElapsed % 60000) / 1000));
        totalTimeEl.textContent = `${mm}:${ss}`;

        totalErrorsEl.textContent = totalErrors;

        // WPM = (typed/5) / minutes
        const minutes = totalElapsed / 60000;
        const wpmCalc = minutes > 0 ? Math.round((totalTyped / 5) / minutes) : 0;
        wpmEl.textContent = wpmCalc;

        // Точність %
        const accuracy = totalTyped > 0
            ? Math.round((totalCorrect / totalTyped) * 100)
            : 0;
        accuracyEl.textContent = `${accuracy}%`;

        summaryEl.style.display = 'block';
        finishBtn.style.display = 'inline-block';
    }

    if (stages.length) renderStage(0);
});
