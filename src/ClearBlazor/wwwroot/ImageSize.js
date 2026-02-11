window.GetImageSize = (id) => {
    var img = document.getElementById(id);
    if (!img)
        return;
    const originalWidth = img.naturalWidth;
    const originalHeight = img.naturalHeight;
    return { ImageWidth: originalWidth, ImageHeight: originalHeight }   
}
