using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// TableView is a templated table component supporting virtualization and allowing multiple selections.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class TableView<TItem> : ListBase<TItem>, IBorder, IBackground, IBoxShadow
                         where TItem : ListItem
    {
        /// <summary>
        /// The child content of this control. Contains the columns for that table.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// Is is zero based.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);

        [Parameter]
        public bool ShowHeader { get; set; } = true;

        [Parameter]
        public string ColumnDefs { get; set; } = "";

        [Parameter]
        public int RowSpacing { get; set; } = 5;

        [Parameter]
        public int ColumnSpacing { get; set; } = 5;

        [Parameter]
        public GridLines HorizontalGridLines { get; set; } = GridLines.None;

        [Parameter]
        public GridLines VerticalGridLines { get; set; } = GridLines.None;

        [Parameter]
        public bool StickyHeader { get; set; } = true;

        private ScrollViewer _scrollViewer = null!;
        private string _headerId = Guid.NewGuid().ToString();
        private double _headerHeight = 0;
        private string _resizeObserverId = string.Empty;
        private string _baseRowId = Guid.NewGuid().ToString();
        private List<TItem> _items { get; set; } = new List<TItem>();

        private List<TableColumn<TItem>> Columns { get; } = new List<TableColumn<TItem>>();

        /// <summary>
        /// Goto the given index in the data
        /// </summary>
        /// <param name="index">Index to goto. The index is zero based.</param>
        /// <param name="verticalAlignment">Where the index should be aligned in the scroll viewer.</param>
        /// <returns></returns>
        public async Task GotoIndex(int index, Alignment verticalAlignment)
        {
            if (index == 0 && verticalAlignment == Alignment.Start)
                await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id,
                                                -_headerHeight);
            else
            {
                await JSRuntime.InvokeVoidAsync("window.scrollbar.ScrollIntoView", _scrollViewer.Id,
                                                _baseRowId + index, (int)verticalAlignment);
                if (ShowHeader && StickyHeader && verticalAlignment == Alignment.Start)
                {
                    var scrollTop = await JSRuntime.InvokeAsync<double>("window.scrollbar.GetScrollTop",
                                                                _scrollViewer.Id);
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id,
                                                    scrollTop - _headerHeight);
                }
            }
        }

        /// <summary>
        /// Goto the start of the list
        /// </summary>
        public async Task GotoStart()
        {
            await GotoIndex(0, Alignment.Start);
        }

        /// <summary>
        /// Goto the end of the list
        /// </summary>
        public async Task GotoEnd()
        {
            await GotoIndex(_totalNumItems - 1, Alignment.End);
        }

        /// <summary>
        /// Refresh the list. Call this when items are added to or deleted from the data or if an item has changed.
        /// When VirtualizationMode is None a new object needs to be created with a new Id for 
        /// all items that need re-rendering. This ensures that only the changed items are re-rendered. 
        /// (otherwise it would be expensive)
        /// Other Virtualized modes re-render all items, which should not be expensive as they are virtualized.
        /// </summary>
        /// <returns></returns>
        public async Task Refresh(bool full = false)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    _items = await GetItems(0, int.MaxValue);
                    StateHasChanged();
                    break;
                case VirtualizeMode.Virtualize:
                    //await CalculateScrollItems(false);
                    StateHasChanged();
                    break;
                case VirtualizeMode.InfiniteScroll:
                    //await GetCurrentPage();
                    StateHasChanged();
                    break;
                case VirtualizeMode.Pagination:
                    //await GotoPage(_currentPageNum);
                    StateHasChanged();
                    break;
            }
            if (full)
            {
                RefreshAll();
            }
        }

        /// <summary>
        /// Returns true if the list has been scrolled to the end. 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AtEnd()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                case VirtualizeMode.InfiniteScroll:
                    return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollEnd", _scrollViewer.Id);
                case VirtualizeMode.Pagination:
//                    if (_currentPageNum == _numPages)
//                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the list is at the start. 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AtStart()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.InfiniteScroll:
                    return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollStart", _scrollViewer.Id);
                case VirtualizeMode.Virtualize:
                   // if (_scrollState.ScrollTop == 0)
                   //     return true;
                    break;
                case VirtualizeMode.Pagination:
                  //  if (_currentPageNum == 1)
                  //      return true;
                    break;
            }
            return false;

        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (_items.Count() == 0)
                _items = await GetItems(0, int.MaxValue);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
                _resizeObserverId = await ResizeObserverService.Service.
                                          AddResizeObserver(NotifyObservedSizes,
                                                            new List<string>() { _headerId });
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; ";
            return css;
        }


        protected override void AddChild(ClearComponentBase child)
        {
            TableColumn<TItem>? column = child as TableColumn<TItem>;
            if (column != null && !Columns.Contains(column))
            {
                Columns.Add(column);
                StateHasChanged();
            }
        }

        private string GetFullHeaderStyle()
        {
            string css = "background-color:lightblue; ";
            css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                   $"grid-area: 1 / 1 /span 1 / span {Columns.Count}; ";
            if (StickyHeader)
                css += "position:sticky; top:0px; ";
            return css;
        }

        private string GetHeaderStyle(int column)
        {
            string css = string.Empty;

            string justify = "start";
            switch (Columns[column - 1].HeaderAlignment)
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
            var marginTop = -RowSpacing / 2;
            if (row == 2)
                marginTop = RowSpacing / 2;
            return $"align-self:start; border-width:1px 0 0 0; border-style:solid; margin:0px 0 0 0; " +
                   $"grid-area: {row} / 1 / span 1 / span {columnCount}; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
        }

        private string GetVerticalGridLineStyle(int column, int rowCount, bool excludeHeader)
        {
            string exclude = excludeHeader == true ? "2" : "1";
            return $"justify-self:start; z-index:1; border-width:0 0 0 1px; border-style:solid; margin:0 0 0 0px; " +
                   $"grid-area: {exclude} / {column} / span {rowCount} / span 1; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
        }

        internal async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            bool changed = false;
            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _headerId)
                {
                    if (observedSize.ElementHeight > 0 && _headerHeight != observedSize.ElementHeight)
                    {
                        _headerHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
            }
            if (changed && _headerHeight > 0 )
            {
                StateHasChanged();
            }
            await Task.CompletedTask;
        }
    }
}