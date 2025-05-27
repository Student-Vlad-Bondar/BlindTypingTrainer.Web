document.addEventListener("DOMContentLoaded", () => {
    const keyboardConfig = {
        layout: localStorage.getItem('keyboard-layout') || 'qwerty',
        showFingers: localStorage.getItem('keyboard-fingers') !== 'false',
        highlightKeys: true
    };
    
    const keyboardContainer = document.getElementById('keyboard-container');
    const inputBox = document.getElementById('inputBox');

    const qwertyLayout = [
        ['`', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '=', 'Backspace'],
        ['Tab', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']', '\\'],
        ['Caps', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', '\'', 'Enter'],
        ['Shift', 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/', 'Shift'],
        ['Ctrl', 'Win', 'Alt', 'Space', 'Alt', 'Menu', 'Ctrl']
    ];
    
    const fingerMap = {
        '`': 'pinky-left', '1': 'pinky-left', '2': 'ring-left', '3': 'middle-left', '4': 'index-left',
        '5': 'index-left', '6': 'index-right', '7': 'index-right', '8': 'middle-right', '9': 'ring-right', 
        '0': 'pinky-right', '-': 'pinky-right', '=': 'pinky-right',
        'q': 'pinky-left', 'w': 'ring-left', 'e': 'middle-left', 'r': 'index-left',
        't': 'index-left', 'y': 'index-right', 'u': 'index-right', 'i': 'middle-right', 
        'o': 'ring-right', 'p': 'pinky-right', '[': 'pinky-right', ']': 'pinky-right', '\\': 'pinky-right',
        'a': 'pinky-left', 's': 'ring-left', 'd': 'middle-left', 'f': 'index-left',
        'g': 'index-left', 'h': 'index-right', 'j': 'index-right', 'k': 'middle-right', 
        'l': 'ring-right', ';': 'pinky-right', '\'': 'pinky-right',
        'z': 'pinky-left', 'x': 'ring-left', 'c': 'middle-left', 'v': 'index-left',
        'b': 'index-left', 'n': 'index-right', 'm': 'index-right', ',': 'middle-right', 
        '.': 'ring-right', '/': 'pinky-right',
        'Space': 'thumb'
    };

    function createKeyboard() {
        const keyboard = document.createElement('div');
        keyboard.className = 'visual-keyboard';
        
        qwertyLayout.forEach(row => {
            const keyRow = document.createElement('div');
            keyRow.className = 'keyboard-row';
            
            row.forEach(key => {
                const keyElement = document.createElement('div');
                keyElement.className = 'key';
                keyElement.dataset.key = key.toLowerCase();

                if (keyboardConfig.showFingers && fingerMap[key]) {
                    keyElement.classList.add(fingerMap[key]);
                }
                
                if (key.length > 1) {
                    keyElement.classList.add('special-key');
                    if (key === 'Space') {
                        keyElement.classList.add('space-key');
                    }
                }
                
                keyElement.textContent = key;
                keyRow.appendChild(keyElement);
            });
            
            keyboard.appendChild(keyRow);
        });
        
        keyboardContainer.innerHTML = '';
        keyboardContainer.appendChild(keyboard);
    }
    
    function setupKeyHighlighting() {
        if (!inputBox) return;
        
        document.addEventListener('keydown', (e) => {
            const key = e.key.toLowerCase();
            const keyElement = document.querySelector(`.key[data-key="${key}"]`);
            
            if (e.key === ' ') {
                document.querySelector('.key[data-key="space"]')?.classList.add('active');
            }
            
            if (keyElement) {
                keyElement.classList.add('active');
            }
        });
        
        document.addEventListener('keyup', (e) => {
            const key = e.key.toLowerCase();
            const keyElement = document.querySelector(`.key[data-key="${key}"]`);
            
            if (e.key === ' ') {
                document.querySelector('.key[data-key="space"]')?.classList.remove('active');
            }
            
            if (keyElement) {
                keyElement.classList.remove('active');
            }
        });
        
        const stageTextEl = document.getElementById("stageText");
        if (stageTextEl && inputBox) {
            inputBox.addEventListener('input', () => {
                document.querySelectorAll('.key.next-key').forEach(k => 
                    k.classList.remove('next-key'));
                    
                const val = inputBox.value;
                const spans = stageTextEl.querySelectorAll('span');
                
                if (val.length < spans.length) {
                    const nextChar = spans[val.length].textContent.toLowerCase();
                    const nextKeyElement = document.querySelector(`.key[data-key="${nextChar}"]`);
                    
                    if (nextKeyElement) {
                        nextKeyElement.classList.add('next-key');
                    }
                }
            });
        }
    }
    
    if (keyboardContainer) {
        createKeyboard();
        setupKeyHighlighting();
        
        const settingsContainer = document.createElement('div');
        settingsContainer.className = 'keyboard-settings';
        
        const fingerToggle = document.createElement('button');
        fingerToggle.className = 'btn btn-sm ' + 
            (keyboardConfig.showFingers ? 'btn-primary' : 'btn-outline-primary');
        fingerToggle.textContent = keyboardConfig.showFingers ? 'Сховати пальці' : 'Показати пальці';
        fingerToggle.onclick = () => {
            keyboardConfig.showFingers = !keyboardConfig.showFingers;
            localStorage.setItem('keyboard-fingers', keyboardConfig.showFingers);
            createKeyboard();
            setupKeyHighlighting();
            fingerToggle.textContent = keyboardConfig.showFingers ? 'Сховати пальці' : 'Показати пальці';
            fingerToggle.className = 'btn btn-sm ' + 
                (keyboardConfig.showFingers ? 'btn-primary' : 'btn-outline-primary');
        };
        
        settingsContainer.appendChild(fingerToggle);
        keyboardContainer.parentNode.insertBefore(settingsContainer, keyboardContainer.nextSibling);
    }
});
