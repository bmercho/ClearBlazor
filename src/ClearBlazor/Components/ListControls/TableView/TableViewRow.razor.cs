using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ClearBlazor;

namespace ClearBlazorInternal
{
    public partial class TableViewRow<TItem> : ListRowBase<TItem>
           where TItem : ListItem
    {
        [Parameter]
        public int Index { get; set; }

        [Parameter]
        public string RowId { get; set; } = string.Empty;

        [Parameter]
        public int RowSpacing { get; set; } = 5;

        [Parameter]
        public int ColumnSpacing { get; set; } = 5;

        /// <summary>
        /// Indicates if vertical grid lines are to be shown.
        /// </summary>
        [Parameter]
        public GridLines VerticalGridLines { get; set; } = GridLines.None;

        /// <summary>
        /// Gets or sets the collection of columns to display in the table.
        /// </summary>
        [Parameter]
        public List<TableColumn<TItem>> Columns { get; set; } = new List<TableColumn<TItem>>();

        /// <summary>
        /// Used for a ListView
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? RowTemplate { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether users can reorder rows in the component.
        /// </summary>
        [Parameter]
        public bool AllowRowReordering { get; set; } = false;


        internal bool DragOver { get; set; } = false;
        internal static TableViewRow<TItem>? DragRow { get; set; } = null;

        private TableView<TItem>? _parent = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<TableView<TItem>>(Parent);
            if (_parent != null)
            {
                _parent.AddListRow(this);
                return;
            }
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (_parent != null)
                switch (_parent.VirtualizeMode)
                {
                    case VirtualizeMode.None:
                            parameters.TryGetValue<TItem>(nameof(RowData), out var rowData);
                            if (rowData != null)
                                if (RowData == null || rowData.ListItemId != RowData.ListItemId)
                                    DoRender = true;
                        break;
                    case VirtualizeMode.Virtualize:
                    case VirtualizeMode.InfiniteScroll:
                        DoRender = true;
                        break;
                    case VirtualizeMode.Pagination:
                        DoRender = true;
                        break;
                }
            await base.SetParametersAsync(parameters);

        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            DoRender = false;


        }
        protected override bool ShouldRender()
        {
            return DoRender;
        }

        internal override async Task OnPointerEnter(PointerEventArgs args)
        {
            await base.OnPointerEnter(args);
            if (_parent == null)
                return;
            if (_parent.HoverHighlight)
            {
                MouseOver = true;
                await Task.CompletedTask;
                DoRender = true;
                StateHasChanged();
            }
        }

        internal override async Task OnPointerLeave(PointerEventArgs args)
        {
            await base.OnPointerLeave(args);
            if (_parent == null)
                return;
            if (_parent.HoverHighlight)
            {
                MouseOver = false;
                await Task.CompletedTask;
                DoRender = true;
                StateHasChanged();
            }
        }

        protected async Task OnRowClicked(MouseEventArgs args)
        {
            if (_parent == null)
                return;

            bool ctrlDown = args.CtrlKey;
            bool shiftDown = args.ShiftKey;
            await _parent.HandleRowSelection(RowData, RowIndex, ctrlDown, shiftDown);
        }

        private string GetFullRowStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = string.Empty;

            int header = _parent.ShowHeader ? 1 : 0;
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize)
            {
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: auto 1fr auto;" +
                       $"grid-area: {Index + 2 + header} / 1 /span 1 / span {Columns.Count}; " +
                       $" height: {(_parent._rowHeight + RowSpacing)}px;";
            }
            else
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: auto 1fr auto;" +
                             $"grid-area: {Index + 1 + header} / 1 /span 1 / span {Columns.Count}; ";

            if (MouseOver)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SurfaceContainerHighest.SetAlpha(.8).Value}; ";

            if (RowData.IsSelected)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SecondaryContainer.Value}; ";
            return css;
        }

        private string GetRowStyle(int row, int column)
        {
            if (_parent == null)
                return string.Empty;

            return $"display:grid; grid-column: {column} /span 1; grid-row: 2 / span 1; justify-self: stretch; " +
                         $"grid-template-rows: {RowSpacing / 2}px 1fr {RowSpacing / 2}px; " +
                         $"padding:0px {ColumnSpacing / 2}px 0px {ColumnSpacing / 2}px;";
        }

        private string GetContentDivStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = "display:grid; grid-row: 2 / span 1; align-self:center; ";
            return css;
        }

        private string[] GetLines(string? content)
        {
            return content == null ? Array.Empty<string>() : content.Split('\r');
        }

        private string GetHorizontalGridLineStyle(int columnCount, bool forDrag)
        {
            if (_parent == null)
                return string.Empty;

            int header = _parent.ShowHeader ? 1 : 0;

            int borderWidth = forDrag ? 3 : 1;

            int row = 1;
            if (forDrag && DragRow != null && DragRow.Index < Index)
                row = 3;

            string css = string.Empty;
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize)
            {
                css += $"z-order:1; align-self:start; border-width:{borderWidth}px 0 0 0; border-style:solid;" +
                       $"display:grid; grid-template-columns: subgrid; " +
                       $"grid-area: {row} / 1 /span 1 / span {Columns.Count};  " +
                       $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
            }
            else
            {
                css += $"z-order:1; align-self:start; border-width:{borderWidth}px 0 0 0; border-style:solid;" +
                       $"grid-area: {row} / 1 / span 1 / span {columnCount}; " +
                       $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
            }
            return css;
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            if (_parent != null)
                _parent.RemoveListRow(this);
        }
    }
}