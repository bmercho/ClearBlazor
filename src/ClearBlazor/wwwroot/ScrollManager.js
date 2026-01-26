window.scrollbar = {
    setScrollBarProperties: (width, height, borderRadius, backgroundColor, thumbColor, thumbHoverColor, thumbBorder) => {
        var r = document.querySelector(':root');
        r.style.setProperty('--scrollbarWidth', width);
        r.style.setProperty('--scrollbarHeight', height);
        r.style.setProperty('--scrollbarBorderRadius', borderRadius);
        r.style.setProperty('--scrollbarBg', backgroundColor);
        r.style.setProperty('--scrollbarThumb', thumbColor);
        r.style.setProperty('--scrollbarThumbHover', thumbHoverColor);
        r.style.setProperty('--scrollbarThumbBorder', thumbBorder);
    },


    scrollIntoView: (elementId, block) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({ behavior: "instant", block: block, inline: 'start' });
        }
    },

    ScrollIntoView: (scrollViewerId, elementId, headerHeight, alignment) => {
        let scrollViewer = document.getElementById(scrollViewerId);
        let element = document.getElementById(elementId);
        if (!scrollViewer || !element)
            return;
        console.log("ElementTop:" + element.offsetTop + " SVTop:" + scrollViewer.offsetTop +
            " ElementHt:" + element.offsetHeight + " SVHt:" + scrollViewer.offsetHeight +
            " HeaderHt:" + headerHeight);
        console.log("ElementClientTop:" + element.clientTop + " SVClientTop:" + scrollViewer.clientTop +
            " ElementClientHt:" + element.clientHeight + " SVClientHt:" + scrollViewer.clientHeight);
        if (alignment == 2) 
            scrollViewer.scrollTop = element.offsetTop - scrollViewer.offsetTop -
                scrollViewer.offsetHeight / 2 + element.offsetHeight / 2 - headerHeight / 2;
        else if (alignment == 3)
            scrollViewer.scrollTop = element.offsetTop - scrollViewer.offsetTop -
                scrollViewer.offsetHeight + element.offsetHeight;
        else
            scrollViewer.scrollTop = element.offsetTop - scrollViewer.offsetTop - headerHeight;
        console.log("ScrollTop:" + scrollViewer.scrollTop);
    },

    SetScrollTop: (elementId, scrollTop) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.scrollTop = scrollTop;
        }
    },

    GetScrollTop: (elementId) => {
        let element = document.getElementById(elementId);
        if (element) {
            return element.scrollTop;
        }
        return 0;
    },


    SetScrollLeft: (elementId, scrollLeft) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.scrollLeft = scrollLeft;
        }
    },

    GetScrollPosition: (elementId) => {
        let element = document.getElementById(elementId);
        if (element) {
            return {
                ScrollTop: element.scrollTop, ScrollLeft: element.scrollLeft,
                ScrollHeight: element.ScrollHeight, ScrollWidth: element.ScrollWidth
            }
        }
    },

    AtScrollEnd: (scrollViewerId) => {
        let scrollViewer = document.getElementById(scrollViewerId);
        if (!scrollViewer)
            return false;
        if (Math.abs(scrollViewer.scrollTop - scrollViewer.scrollHeight + scrollViewer.clientHeight) < 2)
            return true;
        return false;
    },

    AtScrollStart: (scrollViewerId) => {
        let scrollViewer = document.getElementById(scrollViewerId);
        if (!scrollViewer)
            return false;
        if (scrollViewer.scrollTop == 0)
            return true;
        return false;
    },

    ListenForScrollEvents: (elementId, dotnethelper) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.addEventListener("scroll", (e) => {
                dotnethelper.invokeMethodAsync('HandleScrollEvent',
                    {
                        ScrollTop: element.scrollTop, ScrollLeft: element.scrollLeft,
                        ScrollHeight: element.scrollHeight, ScrollWidth: element.scrollWidth,
                        ClientHeight: element.clientHeight, ClientWidth: element.clientWidth
                    })
            })
        }
    }

}
