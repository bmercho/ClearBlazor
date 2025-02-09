window.ResizeCanvas = (id) => {
    var canvas = document.getElementById(id);

    canvas.style.width = '100%';
    canvas.style.height = '100%';
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
}
