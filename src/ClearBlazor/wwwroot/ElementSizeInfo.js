window.GetElementSizeInfo = (element) => {
    var elementRect = element.getBoundingClientRect();
    var parentRect = element.parentNode.getBoundingClientRect();
    return {
        ElementX: elementRect.x, ElementY: elementRect.y, ElementWidth: elementRect.width, ElementHeight: elementRect.height
    }
}

window.GetElementSizeInfoById = (id) => {
    var element = document.getElementById(id);
    if (!element)
        return;
    var elementRect = element.getBoundingClientRect();
    return {
        ElementX: elementRect.x, ElementY: elementRect.y, ElementWidth: elementRect.width, ElementHeight: elementRect.height
    }
}

window.GetParentElementSizeInfoById = (id) => {
    var element = document.getElementById(id);
    if (!element)
        return;
    var parentNode = element.parentNode;
    if (!parentNode)
        return;
    var parentRect = parentNode.getBoundingClientRect();
    return {
        ElementX: parentRect.x, ElementY: parentRect.y, ElementWidth: parentRect.width, ElementHeight: parentRect.height
    }
}
