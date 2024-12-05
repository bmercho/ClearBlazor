window.SetStyleProperty = (id, property, value) => {
    var element = document.getElementById(id);
    if (element)
        element.style.setProperty(property, value);
}

window.SetStyleProperties = (id1, property1, value1, id2, property2, value2) => {
    var element = document.getElementById(id1);
    if (element)
        element.style.setProperty(property1, value1);
    element = document.getElementById(id2);
    if (element)
        element.style.setProperty(property2, value2);
}
