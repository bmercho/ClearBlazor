//document.onmousemove = function (e) {
//    var x = e.pageX;
//    var y = e.pageY;
//    console.log("X is " + x + " and Y is " + y);
//};

var cssId = 'ClearBlazorCss'; 
if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = '_content/ClearBlazor/ClearBlazor.css';
    link.media = 'all';
    head.appendChild(link);
}

window.clearBlazor = {
    numberInput: {
        initialize: (elementId, isFloat, allowNegativeNumbers, numberDecimalSeparator) => {
            let numberEl = document.getElementById(elementId);

            numberEl?.addEventListener('keydown', function (event) {
                let validChars = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                    "Backspace", "Enter", "ArrowLeft", "ArrowRight", "ArrowUp", "ArrowDown", "Home", "End"];
                let num = document.getElementById(elementId);
                if (isFloat) {
                    if (!num.value.includes('.') && !num.value.includes(numberDecimalSeparator)) {
                        validChars.push("."); // allow '.' for non integer types
                        validChars.push(numberDecimalSeparator); // allow ',' for specific culture
                    }
                }

                if (allowNegativeNumbers && num.value.length == 0) {
                    validChars.push("-"); // allow '-'
                }

                let validControlChars = ["c", "v"];
                if ((event.ctrlKey == false && !validChars.includes(event.key)) ||
                    (event.ctrlKey == true && !validControlChars.includes(event.key))) {
                    event.preventDefault();
                }
            });
            numberEl?.addEventListener('beforeinput', function (event) {
                if (event.inputType === 'insertFromPaste' || event.inputType === 'insertFromDrop') {

                    let num = document.getElementById(elementId);
                    let hasDot = num.value.includes('.');

                    if (!allowNegativeNumbers) {
                        // restrict 'e', 'E', '+', '-'
                        if (isFloat && /[\e\E\+\-]/gi.test(event.data)) {
                            event.preventDefault();
                        }
                        // restrict 'e', 'E', '.', '+', '-'
                        else if (isFloat && hasDot && /[\e\E\.\+\-]/gi.test(event.data)) {
                            event.preventDefault();
                        }
                        // restrict 'e', 'E', '.', '+', '-'
                        else if (!isFloat && /[\e\E\.\+\-]/gi.test(event.data)) {
                            event.preventDefault();
                        }
                    }
                    // restrict 'e', 'E', '+'
                    else if (isFloat && /[\e\E\+]/gi.test(event.data)) {
                        event.preventDefault();
                    }
                    // restrict 'e', 'E', '.', '+'
                    else if (isFloat && hasDot && /[\e\E\.\+]/gi.test(event.data)) {
                        event.preventDefault();
                    }
                    // restrict 'e', 'E', '.', '+'
                    else if (!isFloat && /[\e\E\.\+]/gi.test(event.data)) {
                        event.preventDefault();
                    }

                }
            });
        },
        setValue: (elementId, value) => {
            document.getElementById(elementId).value = value;
        }
    },
    textInput: {
        initialize: (elementId, maxLength) => {
            let textEl = document.getElementById(elementId);

            textEl?.addEventListener('keydown', function (event) {
                let num = document.getElementById(elementId);
                if (maxLength != 0 && num.value.length >= maxLength)
                    event.preventDefault();
            });
        },
        setValue: (elementId, value) => {
            document.getElementById(elementId).value = value;
        }
    },
    popup: {
        initialize: (dotNetInstance) => {
            document.addEventListener('mousedown', function (event) {
                if (dotNetInstance != null) {
                    dotNetInstance.invokeMethodAsync('MouseDown');
                }
            });
        }
    }
}
