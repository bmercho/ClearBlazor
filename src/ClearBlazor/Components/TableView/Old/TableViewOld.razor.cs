using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// TableView is a templated table component supporting virtualization and allowing multiple selections.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class TableViewOld<TItem> : ListBase<TItem>, IBorder, IBackground, IBoxShadow
                         where TItem : ListItem
    {
        /// <summary>
        /// The child content of this control. Contains the columns for that table.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// The height to be used for each item.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// </summary>
        [Parameter]
        public int RowHeight { get; set; } = 30;

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// Is is zero based.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);

        /// <summary>
        /// Indicates if the header row is to be shown or not.
        /// </summary>
        [Parameter]
        public bool ShowHeader { get; set; } = true;

        /// <summary>
        /// Defines the column layout for the table. Similar to the Grid Columns property.
        /// </summary>
        [Parameter]
        public string ColumnDefs { get; set; } = "";

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
        /// Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and 
        /// it takes some time to load the data.
        /// </summary>
        [Parameter]
        public bool ShowLoadingSpinner { get; set; } = false;

        /// <summary>
        /// Approximately the number of rows that will fit in the ScrollViewer.
        /// Adjust this until this number at least fills a page.
        /// Should be too large rather that to small.
        /// Not used if VirtualizationMode is None.
        /// </summary>
        [Parameter]
        public int PageSize { get; set; } = 10;

        private bool _initializing = true;
        private string _scrollViewerId = Guid.NewGuid().ToString();
        private string _headerId = Guid.NewGuid().ToString();
        private double _headerHeight = 0;
        private string _resizeObserverId = string.Empty;
        private string _baseRowId = Guid.NewGuid().ToString();
        private double _componentHeight = 0;

        // Used when VirtualizeMode is Virtualize
        private double _height = 0;
        private double _scrollViewerHeight = 0;
        internal ScrollState _scrollState = new();
        private bool _loadingUp = false;
        private bool _loadingDown = false;

        // For VirtualizeMode InfiniteScroll
        // Pages are zero based. Initially just one page is loaded(page 0) and when that page is scrolled to the end
        // another page is loaded (first page is kept).
        // When that page is scrolled to the bottom the top page is removed and a new page loaded.
        // From then on there are always two pages rendered.

        // Used when VirtualizeMode is InfiniteScroll
        private int _firstRenderedPageNum = 0;
        private double _maxScrollHeight = 0;
        private bool _hasHadData = false;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private List<double> _pageOffsets = new();
        private double _yOffset = 0;

        // Used when VirtualizeMode is Pagination
        private int _currentPageNum = 1;
        private int _numPages = 0;

        internal double _itemWidth = 0;
        internal int _skipItems = 0;
        internal int _takeItems = 0;

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
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    if (index == 0 && verticalAlignment == Alignment.Start)
                        await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId,
                                                        -_headerHeight);
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("window.scrollbar.ScrollIntoView", _scrollViewerId,
                                                        _baseRowId + index, (int)verticalAlignment);
                        if (ShowHeader && StickyHeader && verticalAlignment == Alignment.Start)
                        {
                            var scrollTop = await JSRuntime.InvokeAsync<double>("window.scrollbar.GetScrollTop",
                                                                        _scrollViewerId);
                            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId,
                                                            scrollTop - _headerHeight);
                        }
                    }
                    break;
                case VirtualizeMode.Virtualize:
                    await GotoVirtualIndex(index, verticalAlignment);
                    break;
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    _currentPageNum = (int)Math.Ceiling((double)(index + 1) / (double)PageSize);
                    await GotoPage(_currentPageNum);
                    break;
            }
        }

        /// <summary>
        /// Goto the start of the list
        /// </summary>
        public async Task GotoStart()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    await GotoIndex(0, Alignment.Start);
                    break;
                case VirtualizeMode.Virtualize:
                    await GotoIndex(0, Alignment.Start);
                    break;
                case VirtualizeMode.InfiniteScroll:
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId, 0);
                    await GetFirstPage();
                    StateHasChanged();
                    break;
                case VirtualizeMode.Pagination:
                    _currentPageNum = 1;
                    await GotoPage(_currentPageNum);
                    break;
            }
        }

        /// <summary>
        /// Goto the end of the list
        /// </summary>
        public async Task GotoEnd()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                    await GotoIndex(_totalNumItems - 1, Alignment.End);
                    break;
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    _currentPageNum = _numPages;
                    await GotoPage(_currentPageNum);
                    break;
            }
        }

        /// <summary>
        /// Returns the total number of pages. Used when VirtualizationMode is Pagination
        /// </summary>
        /// <returns></returns>
        public int NumPages()
        {
            return _numPages;
        }

        /// <summary>
        /// Return the current page number. Used when VirtualizationMode is Pagination
        /// </summary>
        /// <returns></returns>
        public int CurrentPageNum()
        {
            return _currentPageNum;
        }

        /// <summary>
        /// Loads the next page. Used when VirtualizationMode is Pagination
        /// </summary>
        public async Task NextPage()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    if (_currentPageNum < _numPages)
                    {
                        _currentPageNum++;
                        await GotoPage(_currentPageNum);
                    }
                    break;
            }
        }

        /// <summary>
        /// Loads the previous page. Used when VirtualizationMode is Pagination
        /// </summary>
        public async Task PrevPage()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    if (_currentPageNum > 1)
                    {
                        _currentPageNum--;
                        await GotoPage(_currentPageNum);
                    }
                    break;
            }
        }

        /// <summary>
        /// Goes to the given page number. Used when VirtualizationMode is Pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        public async Task GotoPage(int pageNumber)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    if (pageNumber < 1)
                        _currentPageNum = 1;
                    else if (pageNumber > _numPages)
                        _currentPageNum = _numPages;
                    else
                        _currentPageNum = pageNumber;
                    _items = await GetItems((_currentPageNum - 1) * PageSize, PageSize);
                    _numPages = (int)Math.Ceiling((double)_totalNumItems / (double)PageSize);
                    StateHasChanged();
                    break;
            }
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
                    if (await CalculateScrollItemsForVirtualize(_scrollState.ScrollTop))
                        StateHasChanged();
                    break;
                case VirtualizeMode.InfiniteScroll:
                    await GetCurrentPage();
                    StateHasChanged();
                    break;
                case VirtualizeMode.Pagination:
                    await GotoPage(_currentPageNum);
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
                    return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollEnd", _scrollViewerId);
                case VirtualizeMode.Pagination:
                    if (_currentPageNum == _numPages)
                        return true;
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
                    return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollStart", _scrollViewerId);
                case VirtualizeMode.Virtualize:
                    if (_scrollState.ScrollTop == 0)
                        return true;
                    break;
                case VirtualizeMode.Pagination:
                    if (_currentPageNum == 1)
                        return true;
                    break;
            }
            return false;

        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (_items.Count() == 0)
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        _items = await GetItems(0, int.MaxValue);
                        break;
                    case VirtualizeMode.Virtualize:
                        break;
                    case VirtualizeMode.InfiniteScroll:
                        break;
                    case VirtualizeMode.Pagination:
                        if (_items.Count() == 0)
                        {
                            _items = await GetItems(0, PageSize);
                            _numPages = (int)Math.Ceiling((double)_totalNumItems / (double)PageSize);
                        }
                        break;
                }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                List<string> elementIds = new List<string>() { Id, _headerId };

                if (VirtualizeMode == VirtualizeMode.Virtualize)
                    elementIds.Add(_scrollViewerId);
                _resizeObserverId = await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds);

                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewerId,
                                            DotNetObjectReference.Create(this));
            }

            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    if (firstRender)
                        await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);
                    break;
                case VirtualizeMode.Virtualize:
                    break;
                case VirtualizeMode.InfiniteScroll:
                    bool changed = false;

                    if (!_hasHadData && !_loadingUp && !_loadingDown)
                    {
                        _hasHadData = true;
                        if (!await GetFirstPage())
                            _hasHadData = false;
                        else
                            changed = true;
                    }
                    if (changed)
                        StateHasChanged();

                    break;
                case VirtualizeMode.Pagination:
                    break;
            }

        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    break;
                case VirtualizeMode.Virtualize:
                    if (await CalculateScrollItemsForVirtualize(scrollState.ScrollTop))
                    {
                        _scrollState = scrollState;
                        StateHasChanged();
                    }
                    break;
                case VirtualizeMode.InfiniteScroll:
                    if (scrollState.ScrollHeight > _maxScrollHeight)
                        _maxScrollHeight = scrollState.ScrollHeight;

                    if (Math.Ceiling(scrollState.ClientHeight + scrollState.ScrollTop) >= scrollState.ScrollHeight)
                    {
                        // Record page offset
                        _pageOffsets.Add(scrollState.ScrollHeight);

                        // If only one page loaded, now that we know the offset of that page, load the second page.
                        // From now on two pages will always be loaded.
                        if (_pageOffsets.Count == 2)
                            await GetSecondPage();
                        else
                            await GetNextPageData(_pageOffsets.Count - 2);
                        StateHasChanged();
                    }
                    else
                        await CalculateScrollItems(scrollState.ScrollTop);

                    break;
                case VirtualizeMode.Pagination:
                    break;
            }
        }

        private async Task<bool> GetFirstPage()
        {
            _items = await GetItems(0, PageSize);
            if (_items.Count == 0)
                return false;

            if (_pageOffsets.Count == 0)
                _pageOffsets.Add(0);
            _yOffset = 0;
            _firstRenderedPageNum = 0;
            return true;
        }

        private async Task GetSecondPage()
        {
            var newItems = await GetItems(PageSize, PageSize);
            _items.AddRange(newItems);
        }

        private async Task GetCurrentPage()
        {
            var newItems = await GetItems((_pageOffsets.Count - 2) * PageSize, PageSize * 2);
            _items = newItems;
        }

        private async Task<bool> GetNextPageData(int currentPageNum)
        {
            if (_loadingUp || _loadingDown)
                _loadItemsCts?.Cancel();

            await _semaphoreSlim.WaitAsync();
            try
            {
                _loadingDown = true;
                if (ShowLoadingSpinner)
                    StateHasChanged();
                List<TItem> newItems;
                bool loadTwoPages = currentPageNum != _firstRenderedPageNum + 1;
                if (loadTwoPages || _items.Count < PageSize * 2)
                    newItems = await GetItems(currentPageNum * PageSize, PageSize * 2);
                else
                    newItems = await GetItems((currentPageNum + 1) * PageSize, PageSize);

                if (newItems.Count > 0)
                {
                    _firstRenderedPageNum = currentPageNum;
                    _yOffset = _pageOffsets[_firstRenderedPageNum];

                    if (loadTwoPages || _items.Count < PageSize * 2)
                        _items = newItems;
                    else
                    {
                        _items = _items.GetRange(PageSize, PageSize);
                        _items.AddRange(newItems);
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                _loadingDown = false;
                _loadingUp = false;
                _semaphoreSlim.Release();
            }
        }

        private async Task GotoVirtualIndex(int index, Alignment verticalAlignment)
        {
            double scrollTop = 0;
            var maxItemsInContainer = _scrollViewerHeight / RowHeight;

            switch (verticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    _skipItems = index;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * RowHeight;
                    else
                        scrollTop = (_skipItems - maxItemsInContainer / 2 + 0.5) * RowHeight;
                    break;
                case Alignment.Start:
                    _skipItems = index;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    scrollTop = _skipItems * RowHeight;
                    break;
                case Alignment.End:
                    if (index < maxItemsInContainer)
                        _skipItems = 0;
                    else
                        _skipItems = (int)(index - maxItemsInContainer + 1);
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * RowHeight;
                    else
                        scrollTop = (index - maxItemsInContainer + 1) * RowHeight;
                    break;
            }
            _items = await GetItems(_skipItems, _takeItems);
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId, scrollTop);
        }

        protected override string UpdateStyle(string css)
        {
            return css + $"display: grid; ";
        }

        private string GetTransformStyle()
        {
            string css = $"display:grid; grid-template-columns: subgrid; " +
             $"grid-area: 1 / 1 /span {_items.Count} / span {Columns.Count}; ";

            if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
                css += $"transform: translateY({_yOffset}px);";

            return css;
        }

        private string GetHeightDivStyle()
        {
            string css = $"display:grid;  grid-template-columns: subgrid;align-content: flex-start; " +
                         $"grid-area: 3 / 1 /span {_items.Count} / span {Columns.Count};";

            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"position: relative; height:{_height}px";

            return css;
        }

        private string GetScrollViewerStyle()
        {
            string overscrollBehaviour = "overscroll-behavior-y:auto; ";
            switch (OverscrollBehaviour)
            {
                case OverscrollBehaviour.Auto:
                    overscrollBehaviour = "overscroll-behavior-y:auto; ";
                    break;
                case OverscrollBehaviour.Contain:
                    overscrollBehaviour = "overscroll-behavior-y:contain; ";
                    break;
                case OverscrollBehaviour.None:
                    overscrollBehaviour = "overscroll-behavior-y:none; ";
                    break;
            }

            return $"overflow-x:auto; overflow-y:auto; scrollbar-gutter:stable; {overscrollBehaviour}";
        }

        protected string GetFirstRowStyle()
        {
            string css = "display:grid;";
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"height: {_skipItems * RowHeight}px; " +
                       $"grid-area: 2 / 1 /span 1 / span {Columns.Count}; ";
            return css;
        }

        protected string GetLastRowStyle()
        {
            string css = "display:grid;";
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"height: {(_totalNumItems - _skipItems - _takeItems) * RowHeight}px; " +
                       $"grid-area: {_items.Count + 1} / 1 /span 1 / span {Columns.Count}; ";
            return css;
        }

        protected string GetContainerStyle()
        {
            string css = $"display:grid; grid-template-columns: subgrid; " +
                         $"grid-area: 2 / 1 /span {_items.Count} / span {Columns.Count}; ";
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"position: relative;height: {_height}px; ";
            else if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
            {
                css += $"height:{_maxScrollHeight}px";
                css += $"transform: translateY({_yOffset}px);";
            }

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
            string css = "background-color:inherit;  z-index:1;";
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
            string css = string.Empty;
            var marginTop = -RowSpacing / 2;
            if (row == 2)
                marginTop = RowSpacing / 2;
            css += $"align-self:start; border-width:1px 0 0 0; border-style:solid; margin:0px 0 0 0; z-index:1; " +
                   $"grid-area: {row} / 1 / span 1 / span {columnCount}; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";

            if (StickyHeader)
                css += $"position:sticky; top:{_headerHeight}px; ";
            return css;
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
                if (observedSize.TargetId == Id)
                {
                    if (observedSize.ElementHeight > 0 && _componentHeight != observedSize.ElementHeight)
                    {
                        _componentHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
                if (observedSize.TargetId == _headerId)
                {
                    if (observedSize.ElementHeight > 0 && _headerHeight != observedSize.ElementHeight)
                    {
                        _headerHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _scrollViewerId)
                {
                    if (observedSize.ElementHeight > 0 && _scrollViewerHeight != observedSize.ElementHeight)
                    {
                        _scrollViewerHeight = observedSize.ElementHeight;
                        _itemWidth = observedSize.ElementWidth;
                        changed = true;
                    }
                }
                if (changed)
                {
                    if (VirtualizeMode == VirtualizeMode.Virtualize)
                    {
                        if (_scrollViewerHeight > 0 && _initializing)
                        {
                            _initializing = false;
                            await CalculateScrollItemsForVirtualize(_scrollState.ScrollTop);
                        }
                    }
                    StateHasChanged();
                }


            }
        }

        private async Task<bool> CalculateScrollItemsForVirtualize(double scrollTop)
        {
            var skipItems = (int)(scrollTop / RowHeight);
            var takeItems = (int)Math.Ceiling((double)(scrollTop + _scrollViewerHeight) / RowHeight) - skipItems;
            if (skipItems != _skipItems || takeItems != _takeItems || _items.Count == 0)
            {
                if (Math.Abs(skipItems - _skipItems) > takeItems / 3 ||
                    Math.Abs(takeItems - _takeItems) > takeItems / 3 ||
                    _items.Count == 0)
                {
                    _skipItems = skipItems;
                    _takeItems = takeItems;
                    _items = await GetItems(_skipItems, _takeItems*3);
                    _height = _totalNumItems * RowHeight;
                    return true;
                }
            }
            return false;
        }

        private async Task CalculateScrollItems(double scrollTop)
        {
            for (int page = 0; page < _pageOffsets.Count - 1; page++)
            {
                double minOffset = _pageOffsets[page];
                double maxOffset = _pageOffsets[page + 1];
                if (scrollTop >= minOffset && scrollTop < maxOffset)
                {
                    if (page != _firstRenderedPageNum)
                    {
                        if (_loadingUp || _loadingDown)
                            _loadItemsCts?.Cancel();
                        await _semaphoreSlim.WaitAsync();
                        try
                        {
                            if (page < _firstRenderedPageNum)
                                _loadingUp = true;
                            else
                                _loadingDown = true;

                            if (ShowLoadingSpinner)
                                StateHasChanged();

                            _firstRenderedPageNum = page;
                            _yOffset = _pageOffsets[page];

                            _items = await GetItems(page * 10, PageSize * 2);
                        }
                        finally
                        {
                            _loadingDown = false;
                            _loadingUp = false;
                            _semaphoreSlim.Release();
                            StateHasChanged();
                        }
                    }
                    return;
                }
            }
        }

    }
}