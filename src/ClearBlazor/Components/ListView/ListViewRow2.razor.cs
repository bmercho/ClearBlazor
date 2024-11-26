using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ClearBlazorInternal;
namespace ClearBlazor
{
    public partial class ListViewRow2<TItem> : ListRowBase<TItem>, IDisposable
           where TItem : ListItem
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        [Parameter]
        public int Index { get; set; }

        private ListView2<TItem>? _parent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<ListView2<TItem>>(Parent);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (_parent != null)
            {
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
            }
            await base.SetParametersAsync(parameters);

            if (_parent != null)
                _parent.AddListRow(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (_parent != null &&
                    (_parent.VirtualizeMode == VirtualizeMode.InfiniteScroll ||
                     _parent.VirtualizeMode == VirtualizeMode.InfiniteScrollReverse))
            {
                if (_parent._resizeObserverId != null)// && 
                    //_parent.RowSizes[RowData.ListItemId.ToString()].RowHeight == 0)
                {
                    await ResizeObserverService.Service.ObserveElement(_parent._resizeObserverId,
                                                                       RowData.ListItemId.ToString());
                    //Console.WriteLine($"Observe: Id:{RowData.ListItemId.ToString()} Row:{RowData.Index}");
                }
            }
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
        protected string GetContentStyle()
        {
            if (_parent == null)
                return "display:grid;";

            var css = "display:grid; height:100px; ";
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize && _parent.RowHeight > 0)
                css += $"position:absolute; height: {_parent.RowHeight}px; width: {_parent._itemWidth}px; " +
                       $"top: {(_parent._skipItems + Index) * _parent.RowHeight}px;";
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
                _parent.RemoveListRow(this);
        }
    }
}