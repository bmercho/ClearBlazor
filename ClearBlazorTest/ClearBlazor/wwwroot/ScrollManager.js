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

    ListenForScrollEvents: (elementId, dotnethelper) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.addEventListener("scroll", (e) => {
                dotnethelper.invokeMethodAsync('HandleScrollEvent',
                    {
                        ScrollTop: element.scrollTop, ScrollLeft: element.scrollLeft,
                        ScrollHeight: element.scrollHeight, ScrollWidth: element.scrollWidth
                    })
            })
        }
    }

}
