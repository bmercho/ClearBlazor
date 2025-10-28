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
    if (_keys == null || _keys.includes(event.code)) {
        event.preventDefault();
        _dotNetInstance.invokeMethodAsync('KeyDown', event.keyCode, event.code, event.shiftKey, event.ctrlKey, event.altKey);
    }
}

const keyUpHandler = function (event) {
    if (_keys == null || _keys.includes(event.code)) {
        event.preventDefault();
        _dotNetInstance.invokeMethodAsync('KeyUp', event.keyCode, event.code, event.shiftKey, event.ctrlKey, event.altKey);
    }
}


