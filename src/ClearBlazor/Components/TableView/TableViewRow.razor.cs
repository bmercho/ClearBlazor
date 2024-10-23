using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class TableViewRow<TItem> : ListRowBase<TItem>, IDisposable
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

        [Parameter]
        public List<TableColumn<TItem>> Columns { get; set; } = new List<TableColumn<TItem>>();

        private bool _mouseOver = false;
        private TableView<TItem>? _parent = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<TableView<TItem>>(Parent);
            if (_parent != null)
                _parent.AddListRow(this);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (_parent != null)
                switch (_parent.VirtualizeMode)
                {
                    case VirtualizeMode.None:
                            parameters.TryGetValue<TItem>(nameof(RowData), out var rowData);
                            if (rowData != null)
                                if (RowData == null || rowData.Id != RowData.Id)
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
            await _parent.HandleRowSelection(this, ctrlDown, shiftDown);
        }

        private string GetFullRowStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = string.Empty;


            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize)
            {
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                         $"grid-column: 1 / span {Columns.Count}; ";
                css += $"justify-self:start; position:relative; " +
                       $"top:{_parent._scrollTop}px; height: {_parent.RowHeight}px;";
                //$"top:{Index * _parent.RowHeight - 1 + _parent._scrollState.ScrollTop}px; height: {_parent.RowHeight}px;";
            }
            else
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                             $"grid-area: {2 + Index} / 1 /span 1 / span {Columns.Count}; ";


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

            string justify = "start";
            switch (Columns[column - 1].ContentAlignment)
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
            return $"display:grid; height:{_parent.RowHeight} grid-column: {column} /span 1; justify-self: {justify};" +
                   $"padding:{RowSpacing / 2}px {ColumnSpacing / 2}px {RowSpacing / 2}px {ColumnSpacing / 2}px;";

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
                css += $"align-self:start; border-width:1px 0 0 0; border-style:solid;" +
                       $"grid-area: 2 / 1 /span 1 / span {Columns.Count};  border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
                css += $"justify-self:start; position:relative; top:{Index * _parent.RowHeight}px;";

            }
            else
            {
                css += $"align-self:start; border-width:1px 0 0 0; border-style:solid;" +
                       $"grid-area: {row} / 1 / span 1 / span {columnCount}; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
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