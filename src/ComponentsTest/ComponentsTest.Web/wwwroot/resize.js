var resizeJs = {}

var dotNetInstance = null;

resizeJs.setDotNetInstance = function (dotnethelper) {
    dotNetInstance = dotnethelper
    dotNetInstance.invokeMethodAsync('SetBrowserDimensions', window.innerHeight, window.innerWidth);
}

function reportWindowSize() {

    if (dotNetInstance != null) {
        dotNetInstance.invokeMethodAsync('SetBrowserDimensions', window.innerHeight, window.innerWidth);
    }
}

window.addEventListener('resize', reportWindowSize);

