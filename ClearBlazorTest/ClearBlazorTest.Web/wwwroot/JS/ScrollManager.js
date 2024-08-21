window.scrollbar = {
    setScrollBarProperties: (width, height, borderRadius, backgroundColour, thumbColour, thumbHoverColour, thumbBorder) => {
        var r = document.querySelector(':root');
        r.style.setProperty('--scrollbarWidth', width);
        r.style.setProperty('--scrollbarHeight', height);
        r.style.setProperty('--scrollbarBorderRadius', borderRadius);
        r.style.setProperty('--scrollbarBg', backgroundColour);
        r.style.setProperty('--scrollbarThumb', thumbColour);
        r.style.setProperty('--scrollbarThumbHover', thumbHoverColour);
        r.style.setProperty('--scrollbarThumbBorder', thumbBorder);
    },


    scrollIntoView: (elementId, block) => {
        let element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({ behavior: "instant", block: block, inline: 'start' });
        }
    }
}
