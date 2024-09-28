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

    ScrollIntoView: (scrollViewerId, elementId, alignment) => {
        let scrollViewer = document.getElementById(scrollViewerId);
        let element = document.getElementById(elementId);
        if (!scrollViewer || !element)
            return;
        if (alignment == 2) {
            scrollViewer.scrollTop = element.offsetTop - scrollViewer.offsetTop -
                scrollViewer.clientHeight / 2 + element.clientHeight / 2;
        }
        else if (alignment == 3)
            scrollViewer.scrollTop = element.offsetTop - scrollViewer.offsetTop -
                                     scrollViewer.clientHeight + element.clientHeight;
        else
            scrollViewer.scrollTop = element.offsetTop - scrollViewer.offsetTop;
    },

    SetScrollTop: (elementId, scrollTop) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.scrollTop = scrollTop;
        }
    },

    SetScrollLeft: (elementId, scrollLeft) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.scrollTop(scrollLeft);
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

    AtScrollEnd: (scrollViewerId, elementId) => {
        let scrollViewer = document.getElementById(scrollViewerId);
        let element = document.getElementById(elementId);
        if (!scrollViewer || !element)
            return false;
        if (scrollViewer.scrollTop == element.offsetTop - scrollViewer.offsetTop -
                                      scrollViewer.clientHeight + element.clientHeight )
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
