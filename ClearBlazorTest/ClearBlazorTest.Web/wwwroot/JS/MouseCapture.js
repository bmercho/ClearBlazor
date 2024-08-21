window.ReleaseMouseCapture = (elementId, mouseButton) => {
    let element = document.getElementById(elementId);
    if (element) {
        element.releasePointerCapture(mouseButton);
    }
}

    window.CaptureMouse = (elementId, mouseButton) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.setPointerCapture(mouseButton);
        }
}