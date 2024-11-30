window.StopPropagation = (elementId, eventName) => {
    var element = document.getElementById(elementId);
    if (!element)
        return;
    element.addEventListener(eventName, e => e.preventDefault(), { passive: false });
}
window.AllowPropagation = (elementId, eventName) => {
    var element = document.getElementById(elementId);
    if (!element)
        return;
    elemant.addEventListener(eventName, e => e.preventDefault(), { passive: true });
}
