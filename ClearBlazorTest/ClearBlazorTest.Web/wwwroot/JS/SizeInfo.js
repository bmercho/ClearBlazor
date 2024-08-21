window.getSizeInfo = (element) => {
    var elementRect = element.getBoundingClientRect();
    var parentRect = element.parentNode.getBoundingClientRect();
    return {
        WindowWidth: window.innerWidth, WindowHeight: window.innerHeight,
        ParentX: parentRect.x, ParentY: parentRect.y, ParentWidth: parentRect.width, ParentHeight: parentRect.height,
        ElementX: elementRect.x, ElementY: elementRect.y, ElementWidth: elementRect.width, ElementHeight: elementRect.height
    }
}

window.GetSizeInfo = (id) => {
    var element = document.getElementById(id);
    if (!element)
        return;
    var elementRect = element.getBoundingClientRect();
    var parentRect = element.parentNode.getBoundingClientRect();
    return {
        WindowWidth: window.innerWidth, WindowHeight: window.innerHeight,
        ParentX: parentRect.x, ParentY: parentRect.y, ParentWidth: parentRect.width, ParentHeight: parentRect.height,
        ElementX: elementRect.x, ElementY: elementRect.y, ElementWidth: elementRect.width, ElementHeight: elementRect.height
    }
}
