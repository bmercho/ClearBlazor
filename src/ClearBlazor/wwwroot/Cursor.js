(function () {
    if (window.Cursor) return;

    let _previousCursor = null;

    function setCursor(cursor) {
        try {
            if (_previousCursor === null) _previousCursor = document.body.style.cursor || "";
            document.body.style.cursor = cursor || "";
        } catch { /* ignore */ }
    }

    function resetCursor() {
        try {
            document.body.style.cursor = _previousCursor || "";
        } catch { /* ignore */ }
        _previousCursor = null;
    }

    // Expose API (case-insensitive helpers for C# typos)
    window.Cursor = {
        setCursor: setCursor,
        resetCursor: resetCursor
    };
})();