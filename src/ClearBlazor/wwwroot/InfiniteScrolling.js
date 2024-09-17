window.InfiniteScrolling = {

    Initialize: (scrollViewerId, firstMarkerId, middleMarkerId, lastMarkerId, componentInstance) => {
        const firstMarker = 0;
        const lastMarker = 1;
        const scrollViewerElement = document.getElementById(scrollViewerId);
        var firstMarkerElement = document.getElementById(firstMarkerId);
        var middleMarkerElement = document.getElementById(middleMarkerId);
        var lastMarkerElement = document.getElementById(lastMarkerId);
        const firstOptions = {
            root: scrollViewerElement,
            rootMargin: '0px 0px 0px 0px',
            threshold: 0,
        };
        const lastOptions = {
            root: scrollViewerElement,
            rootMargin: '0px 0px 0px 0px',
            threshold: 0,
        };

        const firstObserver = new IntersectionObserver(async (entries) => {
            for (const entry of entries) {
                if (entry.isIntersecting) {
                    firstObserver.unobserve(firstMarkerElement);
                    await componentInstance.invokeMethodAsync("MarkerHit", firstMarker);
                }
            }
        }, firstOptions);

        const lastObserver = new IntersectionObserver(async (entries) => {
            for (const entry of entries) {
                if (entry.isIntersecting) {
                    lastObserver.unobserve(lastMarkerElement);
                    await componentInstance.invokeMethodAsync("MarkerHit", lastMarker);
                }
            }
        }, lastOptions);

        if (middleMarkerElement) {
            var pageSize = middleMarkerElement.offsetTop - firstMarkerElement.offsetTop;
            componentInstance.invokeMethod("VirtualPageSize", pageSize);
        }

        firstObserver.observe(firstMarkerElement);
        lastObserver.observe(lastMarkerElement);

        return {
            dispose: () => dispose(firstObserver),
            dispose: () => dispose(lastObserver),
            onNewItems: () => {
                firstObserver.unobserve(firstMarkerElement);
                lastObserver.unobserve(lastMarkerElement);

                firstMarkerElement = document.getElementById(firstMarkerId);
                if (middleMarkerElement) { 
                    middleMarkerElement = document.getElementById(middleMarkerId);
                }
                lastMarkerElement = document.getElementById(lastMarkerId);

                if (middleMarkerElement) {
                    var pageSize = middleMarkerElement.offsetTop - firstMarkerElement.offsetTop;
                    componentInstance.invokeMethod("VirtualPageSize", pageSize);
                }

                firstObserver.observe(firstMarkerElement);
                lastObserver.observe(lastMarkerElement);
            },
        };
    }
}

function dispose(observer) {
    observer.disconnect();
}
