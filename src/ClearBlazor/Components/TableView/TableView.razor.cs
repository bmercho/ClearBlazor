using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ClearBlazor
{
    /// <summary>
    /// TableView is a templated table component supporting virtualization and allowing multiple selections.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class TableView<TItem> : ListBase<TItem>
        where TItem : ListItem
    {
        /// <summary>
        /// The child content of this control. Contains the columns for that table.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        /// The height to be used for each row.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// </summary>
        [Parameter]
        public required int RowHeight { get; set; } = 30;

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);

        /// <summary>
        /// Indicates if the header row is to be shown or not.
        /// </summary>
        [Parameter]
        public bool ShowHeader { get; set; } = true;

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
        private string _baseRowId = Guid.NewGuid().ToString();
        private double _componentHeight = 0;

        // Used when VirtualizeMode is Virtualize
        private double _height = 0;
        private double _scrollViewerHeight = 0;
        private string _resizeObserverId = string.Empty;
        internal double _scrollTop = 0;
        private bool _loadingUp = false;
        private bool _loadingDown = false;
        private double _prevScrollTop = 0;

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
        private string _columnDefinitions = string.Empty;   

        /// <summary>
        /// Goto the given index in the data. Not used if VirtualizationMode is InfiniteScroll.
        /// </summary>
        /// <param name="index">Index to goto. The index is zero based.</param>
        /// <param name="verticalAlignment">Where the index should be aligned in the scroll viewer.</param>
        /// <returns></returns>
        public async Task GotoIndex(int index, Alignment verticalAlignment)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.ScrollIntoView", _scrollViewerId,
                                                    _baseRowId + index, (int)verticalAlignment);
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
                    await GetFirstPageAsync();
                    StateHasChanged();
                    break;
                case VirtualizeMode.Pagination:
                    _currentPageNum = 1;
                    await GotoPage(_currentPageNum);
                    break;
            }
        }

        /// <summary>
        /// Goto the end of the list. Not used if VirtualizationMode is InfiniteScroll.
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
        public async Task Refresh()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    _items = await GetItems(0, int.MaxValue);
                    StateHasChanged();
                    break;
                case VirtualizeMode.Virtualize:
                    if (await CheckForNewRows(_scrollTop, true))
                        StateHasChanged();
                    break;
                case VirtualizeMode.InfiniteScroll:
                    if (_pageOffsets.Count < 2)
                    { 
                        if (await GetFirstPageAsync())
                            _hasHadData = true;
                    }
                    else
                        await GetCurrentPageAsync();
                    StateHasChanged();
                    break;
                case VirtualizeMode.Pagination:
                    await GotoPage(_currentPageNum);
                    StateHasChanged();
                    break;
            }
        }

        /// <summary>
        /// Returns true if the list is at the end. 
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
                    if (_scrollTop == 0)
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
                        await CheckForNewRows(_scrollTop, true);
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
                List<string> elementIds = new List<string>() { Id };

                if (VirtualizeMode == VirtualizeMode.Virtualize)
                {
                    elementIds.Add(_scrollViewerId);
                }
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
                        if (!await GetFirstPageAsync())
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

        protected override void AddChild(ClearComponentBase child)
        {
            TableColumn<TItem>? column = child as TableColumn<TItem>;
            if (column != null && !Columns.Contains(column))
            {
                Columns.Add(column);
                _columnDefinitions = string.Join(", ", Columns.Select(c => c.ColumnDefinition));    
                StateHasChanged();
            }
        }
        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            try
            {
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        break;
                    case VirtualizeMode.Virtualize:
                        double top = scrollState.ScrollTop;
                        if (scrollState.ScrollTop > _height - _scrollViewerHeight)
                            top = _height - _scrollViewerHeight;
                        if (_scrollTop == top)
                            return;

                        _scrollTop = top;
                        if (await CheckForNewRows(_scrollTop, false))
                            StateHasChanged();
                        break;
                    case VirtualizeMode.InfiniteScroll:
                        if (Math.Ceiling(scrollState.ClientHeight + scrollState.ScrollTop) >= scrollState.ScrollHeight)
                        {
                            if (scrollState.ScrollHeight > _pageOffsets[_pageOffsets.Count - 1])
                            {
                                // If only one page loaded, now that we know the offset of that page, load the second page.
                                // From now on two pages will always be loaded.
                                if (_pageOffsets.Count == 1)
                                    await GetSecondPageAsync(scrollState.ScrollHeight);
                                else
                                    await GetNextPageDataAsync(_pageOffsets.Count - 1, 
                                                               scrollState.ScrollHeight, scrollState.ScrollTop);
                                StateHasChanged();
                            }
                            else
                               await CheckForNewRows(scrollState.ScrollTop);

                        }
                        else
                            await CheckForNewRows(scrollState.ScrollTop);

                        break;
                    case VirtualizeMode.Pagination:
                        break;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async Task<bool> GetFirstPageAsync()
        {
            _items = await GetItems(0, PageSize);
            if (_items.Count == 0)
                return false;

            if (_pageOffsets.Count == 0)
                RecordPageOffset(0);
            _yOffset = 0;
            _firstRenderedPageNum = 0;
            return true;
        }

        private async Task GetSecondPageAsync(double scrollHeight)
        {
            var newItems = await GetItems(PageSize, PageSize);
            var count = newItems.Count;
            if (count == 0)
                return;

            if (count == PageSize)
                RecordPageOffset(scrollHeight);

            _items.AddRange(newItems);
        }

        private async Task GetCurrentPageAsync()
        {
            List<TItem> newItems;
            if (_pageOffsets.Count == 0)
                newItems = await GetItems(0, PageSize);
            else
                newItems = await GetItems((_pageOffsets.Count - 2) * PageSize, PageSize * 2);
            _items = newItems;
        }

        private async Task<bool> GetNextPageDataAsync(int currentPageNum, double scrollHeight, double scrollTop)
        {
            if (_loadItemsCts != null && (_loadingUp || _loadingDown))
                await _loadItemsCts.CancelAsync();

            await _semaphoreSlim.WaitAsync();
            try
            {
                _loadingDown = true;
                if (ShowLoadingSpinner)
                    StateHasChanged();
                List<TItem> newItems;
                bool loadTwoPages = currentPageNum != _firstRenderedPageNum + 1;
                var numToGet = 0;
                if (loadTwoPages || _items.Count < PageSize * 2)
                {
                    numToGet = PageSize * 2;
                    newItems = await GetItems((currentPageNum-1) * PageSize, numToGet);
                }
                else
                {
                    numToGet = PageSize;
                    newItems = await GetItems((currentPageNum + 1) * PageSize, numToGet);
                }

                if (newItems.Count > 0)
                {
                    // Check if at end;
                    if (_items.Count == 0 || _items[_items.Count - 1].Index == newItems[newItems.Count - 1].Index)
                        return false;

                    RecordPageOffset(scrollHeight);

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
            catch(Exception ex)
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

        private void RecordPageOffset(double scrollHeight)
        {
            // Record page offset
            _pageOffsets.Add(scrollHeight);
            if (scrollHeight > _maxScrollHeight)
                _maxScrollHeight = scrollHeight;
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
            if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
                return $"transform: translateY({_yOffset}px);";
            else
                return string.Empty;

        }
        private string GetHeightDivStyle()
        {
            if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
                return $"height:{_maxScrollHeight}px";
            else
                return string.Empty;
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

            return $"display:grid; height:{_componentHeight}px; " +
                   $"justify-self:stretch; overflow-x:hidden; " +
                   $"overflow-y:auto; scrollbar-gutter:stable; {overscrollBehaviour}" +
                   $"grid-area: 1 / 1 / span 1 / span 1; ";
        }

        protected string GetContainerStyle()
        {
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                return $"display:grid; position: relative;height: {_height}px";
            else
                return string.Empty;
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
                            await CheckForNewRows(_scrollTop, true);
                        }
                    }
                    StateHasChanged();
                }
            }
        }

        private async Task<bool> CheckForNewRows(double scrollTop, bool reload = false)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Pagination:
                    break;
                case VirtualizeMode.Virtualize:
                    return await CheckForNewVirtualizationRows(scrollTop, reload);
                case VirtualizeMode.InfiniteScroll:
                    return await CheckForNewInfiniteScrollRowsAsync(scrollTop);
            }
            return false;
        }

        private async Task<bool> CheckForNewVirtualizationRows(double scrollTop, bool reload)
        {
            var skipItems = (int)(scrollTop / RowHeight);
            var takeItems = (int)Math.Ceiling((double)(scrollTop + _scrollViewerHeight) / RowHeight) - skipItems;

            if (reload || skipItems != _skipItems || takeItems != _takeItems || _items.Count == 0)
            {
                if (_loadingUp || _loadingDown)
                    _loadItemsCts?.Cancel();
                await _semaphoreSlim.WaitAsync();
                try
                {
                    if (_prevScrollTop < scrollTop)
                        _loadingDown = true;
                    else
                        _loadingUp = true;

                    if (ShowLoadingSpinner)
                        StateHasChanged();

                    _prevScrollTop = scrollTop;
                    _skipItems = skipItems;
                    _takeItems = takeItems;
                    _items = await GetItems(_skipItems, _takeItems);
                    _height = _totalNumItems * RowHeight;
                }
                finally
                {
                    _loadingDown = false;
                    _loadingUp = false;
                    _semaphoreSlim.Release();
                    StateHasChanged();
                }
                return true;
            }
            return false;
        }
        private async Task<bool> CheckForNewInfiniteScrollRowsAsync(double scrollTop)
        {
            for (int page = 0; page < _pageOffsets.Count - 1; page++)
            {
                double minOffset = _pageOffsets[page];
                double maxOffset = _pageOffsets[page + 1];
                if (scrollTop >= minOffset && scrollTop < maxOffset)
                {
                    if (page != _firstRenderedPageNum)
                        return await LoadInfiniteScrollPageAsync(page);
                    return false;
                }
            }
            return false;
        }

        private async Task<bool> LoadInfiniteScrollPageAsync(int page)
        {

            if (_loadingUp || _loadingDown)
                _loadItemsCts?.Cancel();
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (page == _firstRenderedPageNum)
                    return false;

                if (page < _firstRenderedPageNum)
                    _loadingUp = true;
                else
                    _loadingDown = true;

                if (ShowLoadingSpinner)
                    StateHasChanged();


                var items = await GetItems(page * PageSize, PageSize * 2);

                if (items.Count > 0)
                {
                    _firstRenderedPageNum = page;
                    _yOffset = _pageOffsets[page];
                    _items = items;
                }

                return true;
            }
            finally
            {
                _loadingDown = false;
                _loadingUp = false;
                _semaphoreSlim.Release();
                StateHasChanged();
            }

        }

    }
}