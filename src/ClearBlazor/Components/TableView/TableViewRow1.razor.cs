using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ClearBlazorInternal;
namespace ClearBlazor
{
    public partial class TableViewRow1<TItem> : ListRowBase<TItem>, IDisposable
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


        [Parameter]
        public List<TableColumn1<TItem>> Columns { get; set; } = new List<TableColumn1<TItem>>();

        private TableView1<TItem>? _parent = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<TableView1<TItem>>(Parent);
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
                                _doRender = true;
                        break;
                    case VirtualizeMode.Virtualize:
                    case VirtualizeMode.InfiniteScroll:
                        _doRender = true;
                        break;
                    case VirtualizeMode.Pagination:
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

        protected async Task OnMouseEnter()
        {
            if (_parent == null)
                return;
            if (_parent.HoverHighlight)
            {
                _mouseOver = true;
                await Task.CompletedTask;
                _doRender = true;
                StateHasChanged();
            }
        }

        protected async Task OnMouseLeave()
        {
            if (_parent == null)
                return;
            if (_parent.HoverHighlight)
            {
                _mouseOver = false;
                await Task.CompletedTask;
                _doRender = true;
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
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                         $"grid-column: 1 / span {Columns.Count}; grid-row: 1/ span 1;";
                css += $"justify-self:start; position:relative; " +
                       $"top:{(_parent._skipItems + Index + header) * (_parent._rowHeight + RowSpacing)}px;" +
                       $" height: {(_parent._rowHeight + RowSpacing)}px;";
            }
            else
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                             $"grid-area: {Index + 1 + header} / 1 /span 1 / span {Columns.Count}; ";
            
            if (_mouseOver)
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            if (RowData.IsSelected)
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; ";

            return css;
        }

        private string GetRowStyle(int row, int column)
        {
            if (_parent == null)
                return string.Empty;

            return $"display:grid; grid-column: {column} /span 1; justify-self: stretch;" +
                   $"padding:0px 0px 0px {ColumnSpacing / 2}px;";

        }

        private string GetContainerDivStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = "display:grid; grid-template-columns: 1fr auto;";
            if (_parent.VirtualizeMode != VirtualizeMode.Virtualize)
                css += $"grid-template-rows: {RowSpacing / 2}px 1fr {RowSpacing / 2}px; ";
            else
                css += $"grid-template-rows: 0px 1fr 0px; ";

            return css;
        }

        private string GetContentDivStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = "display:grid; grid-row: 2 / span 1; ";
            return css;
        }

        private string[] GetLines(string? content)
        {
            return content == null ? Array.Empty<string>() : content.Split('\r');
        }

        private string GetHorizontalGridLineStyle(int row, int columnCount)
        {
            if (_parent == null)
                return string.Empty;

            string css = string.Empty;
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize)
            {
                css += $"z-order:1; align-self:start; border-width:1px 0 0 0; border-style:solid;" +
                       $"display:grid; grid-template-columns: subgrid; " +
                       $"grid-area: 2 / 1 /span 1 / span {Columns.Count};  " +
                       $"border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
                css += $"justify-self:start; position:relative; " +
                       $"top:{(_parent._skipItems + Index) * (_parent._rowHeight + RowSpacing)}px;";

            }
            else
            {
                css += $"z-order:1; align-self:start; border-width:1px 0 0 0; border-style:solid;" +
                       $"grid-area: {row} / 1 / span 1 / span {columnCount}; " +
                       $"border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
            }
            return css;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_parent != null)
                _parent.RemoveListRow(this);
        }
    }
}