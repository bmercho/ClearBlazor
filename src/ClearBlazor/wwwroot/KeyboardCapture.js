var _dotNetInstance = null;
var _keys = null;

window.DisableKeyboardCapture = () => {
    document.removeEventListener('keydown', keyDownHandler);
    document.removeEventListener('keyup', keyUpHandler);
}


window.EnableKeyboardCapture = (dotNetInstance, keys) => {
    _dotNetInstance = dotNetInstance;
    _keys = keys;
    document.addEventListener('keydown', keyDownHandler);
    document.addEventListener('keyup', keyUpHandler);
}

const keyDownHandler = function (event) {
    if (_keys.includes(event.key)) {
        event.preventDefault();
        _dotNetInstance.invokeMethodAsync('KeyDown', event.key, event.shiftKey, event.ctrlKey, event.altKey);
    }
}

const keyUpHandler = function (event) {
    if (_keys.includes(event.key)) {
        event.preventDefault();
        _dotNetInstance.invokeMethodAsync('KeyUp', event.key, event.shiftKey, event.ctrlKey, event.altKey);
    }
}


