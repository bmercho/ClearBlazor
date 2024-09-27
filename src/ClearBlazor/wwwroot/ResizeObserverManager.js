export class ResizeObserverManager {

    static ResizeObservers = {};

    static AddResizeObserver(observerId, dotNetRef, elementIds) {
        const obs = new ResizeObserver((entries) => {
            let results = [];

            for (const entry of entries) {
                results.push(new ResizeObserverResult(
                    entry.target.id,
                    entry.contentRect.x, entry.contentRect.y,
                    entry.contentRect.width, entry.contentRect.height));
            };
            const resultsJson = JSON.stringify(results)
            dotNetRef.invokeMethodAsync("NotifyObservedSizes", observerId, resultsJson);
        });

        this.ResizeObservers[observerId] = obs;

        for (let i = 0; i < elementIds.length; i++) {
            let element = document.getElementById(elementIds[i]);
            if (element != null)
                obs.observe(element);
        }
    }

    static RemoveResizeObserver(observerId) {
        if (!this.ResizeObservers[observerId]) return;
        this.ResizeObservers[observerId].disconnect();
        delete this.ResizeObservers[observerId];
    };
}
class ResizeObserverResult {
    constructor(targetId, elementX, elementY, elementWidth, elementHeight) {
        this.targetId = targetId;
        this.elementX = elementX;
        this.elementY = elementY;
        this.elementWidth = elementWidth;
        this.elementHeight = elementHeight;
    }
}

