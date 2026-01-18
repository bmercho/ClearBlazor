window.GetComputedColumnSizes = (gridId) => {
    var grid = document.getElementById(gridId);
    if (!grid)
        return [];

    // Get the computed style of the grid element
    const computedStyle = window.getComputedStyle(grid);

    // Get the value of the 'grid-template-columns' property
    const columnTemplate = computedStyle.getPropertyValue("grid-template-columns");

    // The returned value is a string of pixel values (e.g., "100px 200px 150px")
    // Split the string by spaces to get an array of size strings
    const columnSizeStrings = columnTemplate.split(" ");

    // Map the string array to a number array (parseFloat handles "px" suffix)
    const columnSizes = columnSizeStrings.map(parseFloat);

    return columnSizes;
}
window.GetComputedRowSizes = (gridId) => {
    var grid = document.getElementById(gridId);
    if (!grid)
        return [];

    // Get the computed style of the grid element
    const computedStyle = window.getComputedStyle(grid);

    // Get the value of the 'grid-template-rows' property
    const rowTemplate = computedStyle.getPropertyValue("grid-template-rows");

    // The returned value is a string of pixel values (e.g., "100px 200px 150px")
    // Split the string by spaces to get an array of size strings
    const rowSizeStrings = rowTemplate.split(" ");

    // Map the string array to a number array (parseFloat handles "px" suffix)
    const rowSizes = rowSizeStrings.map(parseFloat);

    return rowSizes;
}
