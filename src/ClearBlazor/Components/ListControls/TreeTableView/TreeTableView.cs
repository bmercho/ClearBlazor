using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a tree table view that can display hierarchical data with customizable options for headers and grid
    /// lines.
    /// </summary>
    public class TreeTableView<TItem> : TreeViewBase<TItem>  
             where TItem : TreeItem<TItem>
    {
        /// <summary>
        /// Indicates if the header row is to be shown or not.
        /// </summary>
        [Parameter]
        public bool ShowHeader { get; set; } = true;

        /// <summary>
        /// The spacing between the columns.
        /// </summary>
        [Parameter]
        public int ColumnSpacing { get; set; } = 5;

        /// <summary>
        /// Indicates if horizontal grid lines are to be shown.
        /// </summary>
        [Parameter]
        public GridLines HorizontalGridLines { get; set; } = GridLines.None;

        /// <summary>
        /// Indicates if vertical grid lines are to be shown.
        /// </summary>
        [Parameter]
        public GridLines VerticalGridLines { get; set; } = GridLines.None;

        /// <summary>
        /// Indicates if the header row (if shown) is sticky. ie stays at top while other rows are scrolled.
        /// </summary>
        [Parameter]
        public bool StickyHeader { get; set; } = true;


        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _showHeader = ShowHeader;
            _columnSpacing = ColumnSpacing;
            _horizontalGridLines = HorizontalGridLines;
            _verticalGridLines = VerticalGridLines;
            _stickyHeader = StickyHeader;
        }

    }
}