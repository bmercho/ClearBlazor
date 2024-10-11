using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class ListViewItem<TItem> : ClearComponentBase, IDisposable
           where TItem : ListItem
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        [Parameter]
        public required TItem RowData { get; set; }

        [Parameter]
        public int RowIndex { get; set; }

        [Parameter]
        public int Index { get; set; }

        [Parameter]
        public string RowId { get; set; } = string.Empty;


        private bool _mouseOver = false;
        private ListView<TItem>? _parent;
        private bool _doRender = true;

        public void Refresh()
        {
            _doRender = true;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<ListView<TItem>>(Parent);
            if (_parent != null)
                _parent.AddListItem(this);
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
                        break;
                }
            await base.SetParametersAsync(parameters);

        }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
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
        protected string GetContentStyle()
        {
            if (_parent == null)
                return "display:grid;";

            var css = "display:grid;";
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize && _parent._itemHeight > 0)
                css += $"position:absolute; height: {_parent._itemHeight}px; width: {_parent._itemWidth}px; " +
                       $"top: {(_parent._skipItems + Index) * _parent._itemHeight}px;";
            if (_mouseOver)
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            if (RowData.IsSelected)
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; ";

            return css;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_parent != null)
                _parent.RemoveListItem(this);
        }
    }
}