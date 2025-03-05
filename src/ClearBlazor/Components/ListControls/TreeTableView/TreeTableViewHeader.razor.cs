using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class TreeTableViewHeader<TItem> where TItem : TreeItem<TItem>
    {
        /// <summary>
        /// Indicates how a list of items is Virtualized.
        /// </summary>
        [Parameter]
        public VirtualizeMode VirtualizeMode { get; set; } = VirtualizeMode.None;

        [Parameter]
        public string HeaderId { get; set; } = string.Empty;

        /// <summary>
        /// The spacing between the rows.
        /// </summary>
        [Parameter]
        public int RowSpacing { get; set; } = 5;

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

        /// <summary>
        /// The height to be used for each row.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// </summary>
        [Parameter]
        public int RowHeight { get; set; } = 30;

        [Parameter]
        public List<TableColumn<TItem>> Columns { get; set; } = new List<TableColumn<TItem>>();

        private TreeTableView<TItem>? _parent = null;
        private bool _doRender = false;

        public void Refresh()
        {
            _doRender = true;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<TreeTableView<TItem>>(Parent);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Pagination:
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Virtualize:
                    _doRender = true;
                    break;
            }
            await base.SetParametersAsync(parameters);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            _doRender = false;
        }
        protected override bool ShouldRender()
        {
            return _doRender;
        }

        private string GetFullHeaderStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = "background-color:white;  z-index:1;";
            css += $"display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr; " +
                   $"grid-area: 1 / 1 /span 1 / span {Columns.Count}; ";
            if (StickyHeader)
            {
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                    case VirtualizeMode.Pagination:
                        css += "position:sticky; top:0px; ";
                        break;
                    case VirtualizeMode.InfiniteScroll:
                        break;
                    case VirtualizeMode.Virtualize:
                        css += $"position:relative; top:{_parent._scrollTop}px; ";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return css;
        }

        private string GetHeaderStyle(int column)
        {
            string css = string.Empty;

            string justify = "start";
            switch (Columns[column - 1].HorizontalHeaderAlignment)
            {
                case Alignment.Stretch:
                    justify = "stretch";
                    break;
                case Alignment.Start:
                    justify = "start";
                    break;
                case Alignment.Center:
                    justify = "center";
                    break;
                case Alignment.End:
                    justify = "end";
                    break;
            }
            css += $"display:grid; " +
                   $"padding:{RowSpacing / 2}px {ColumnSpacing / 2}px {RowSpacing / 2}px {ColumnSpacing / 2}px;" +
                   $"grid-area: 1 / {column} /span 1 /span 1; justify-self: stretch;" +
                   $"align-self:center; ";
            return css;
        }

        private string GetHorizontalGridLineStyle(int row, int columnCount)
        {
            string css = string.Empty;
            css += $"align-self:start; border-width:1px 0 0 0; border-style:solid; z-index:1; " +
                   $"grid-area: 2 / 1 / span 1 / span {columnCount}; " +
                   $"border-color: {ThemeManager.CurrentColorScheme.GrayLight.Value}; ";
            return css;
        }

        private string GetVerticalGridLineStyle(int column)
        {
            string css = $"justify-self:start; z-index:1; border-width:0 0 0 1px; " +
                         $"border-style:solid; margin:0 0 0 -1px; "+
                         $"grid-area: 1 / {column} / span 1 / span 1; " +
                         $"border-color: {ThemeManager.CurrentColorScheme.GrayLight.Value}; ";
            return css;
        }
    }
}