var dotNetInstance = null;

window.resizeListener = function (dotnethelper) {
    dotNetInstance = dotnethelper
    dotNetInstance.invokeMethodAsync('NotifyBrowserDimensions', window.innerHeight, window.innerWidth);
}

function reportWindowSize() {

    if (dotNetInstance != null) {
        dotNetInstance.invokeMethodAsync('NotifyBrowserDimensions', window.innerHeight, window.innerWidth);
    }
}

window.addEventListener('resize', reportWindowSize);

