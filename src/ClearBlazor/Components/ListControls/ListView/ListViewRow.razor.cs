using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ClearBlazorInternal;
namespace ClearBlazor
{
    public partial class ListViewRow<TItem> : ListRowBase<TItem>
           where TItem : ListItem
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        /// The RowId of this row. It consists of base guid as a string plus the index of the row
        /// </summary>
        [Parameter]
        public string RowId { get; set; } = string.Empty;

        /// <summary>
        /// The index of just the rendered rows.
        /// </summary>
        [Parameter]
        public int Index { get; set; }

        private ListView<TItem>? _parent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<ListView<TItem>>(Parent);
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
                }
            }
            DoRender = false;
        }

        protected override bool ShouldRender()
        {
            return DoRender;
        }

        protected async Task OnMouseEnter()
        {
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

        protected async Task OnMouseLeave()
        {
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
            await _parent.HandleRowSelection(RowData, RowData.ItemIndex, ctrlDown, shiftDown);
        }

        private string GetRowId()
        {
            if (_parent == null)
                return string.Empty;

            if (_parent.VirtualizeMode == VirtualizeMode.None)
                return RowId;
            return RowData.ListItemId.ToString();
        }

        protected string GetContentStyle()
        {
            if (_parent == null)
                return "display:grid;";

            var css = "display:grid;";
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize && _parent.RowHeight > 0)
                css += $"position:absolute; height: {_parent.RowHeight}px; width: {_parent._itemWidth}px; " +
                       $"top: {(_parent._skipItems + Index) * _parent.RowHeight}px;";
            if (MouseOver)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SurfaceContainerHighest.SetAlpha(.8).Value}; ";

            if (RowData.IsSelected)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SecondaryContainer.Value}; ";

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