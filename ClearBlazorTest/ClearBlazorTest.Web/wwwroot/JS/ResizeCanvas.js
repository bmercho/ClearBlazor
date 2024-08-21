window.ResizeCanvas = (id) => {
//    var canvas = document.querySelector('canvas');
    var canvas = document.getElementById(id);
    //canvas = document.getElementById(id);
//    canvas1 = document.getElementById('Canvas');

    canvas.style.width = '100%';
    canvas.style.height = '100%';
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
    //return { width: canvas.width, height: canvas.height };
}
