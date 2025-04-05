using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ClearBlazorInternal;

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
        /// The height to be used for each row.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// </summary>
        [Parameter]
        public int RowHeight { get; set; } = 30;

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// Not available when VirtualizationMode is InfiniteScroll
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment)? InitialIndex { get; set; } = null;

        /// <summary>
        /// The spacing between the rows.
        /// </summary>
        [Parameter]
        public int RowSpacing { get; set; } = 5;

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
        /// Not used if VirtualizationMode is None or Virtualized.
        /// </summary>
        [Parameter]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Indicates if the header row is to be shown or not.
        /// </summary>
        [Parameter]
        public bool ShowHeader { get; set; } = true;

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

        internal int _columnSpacing = 5;
        internal bool _stickyHeader = true;

        private bool _initializing = true;
        //private string _scrollViewerId = Guid.NewGuid().ToString();
        private ScrollViewer _scrollViewer = null!;
        private Grid _grid = null!;
        private double _gridWidth = 0;
        private string _headerId = Guid.NewGuid().ToString();
        internal double _headerHeight = 0;
        private string _baseRowId = Guid.NewGuid().ToString();
        private double _componentHeight = 0;
        private double _componentWidth = 0;
        private TableViewHeader<TItem> _header = null!;

        // Used when VirtualizeMode is Virtualize
        private double _height = 0;
        private double _scrollViewerHeight = 0;
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
        internal double _yOffset = 0;

        // Used when VirtualizeMode is Pagination
        private int _currentPageNum = 1;
        private int _numPages = 0;

        internal double _itemWidth = 0;

        private bool _inProgress = false;


        private List<TableColumn<TItem>> Columns { get; } = new List<TableColumn<TItem>>();
        private string _columnDefinitions = string.Empty;

        /// <summary>
        /// Goto the given index in the data. Not available if VirtualizationMode is InfiniteScroll.
        /// </summary>
        /// <param name="index">Index to goto. The index is zero based.</param>
        /// <param name="verticalAlignment">Where the index should be aligned in the scroll viewer.</param>
        /// <returns></returns>
        public async Task GotoIndex(int index, Alignment verticalAlignment)
        {
            switch (VirtualizeMode)
            {

                case VirtualizeMode.None:
                    string id = _baseRowId + index;

                    int headerHeight = 0;
                    if (ShowHeader && (StickyHeader || index == 0))
                        headerHeight = ShowHeader ? (int)_headerHeight - 1 : 0;

                    await JSRuntime.InvokeVoidAsync("window.scrollbar.ScrollIntoView", _scrollViewer.Id,
                                                    id, headerHeight, (int)verticalAlignment);
                    break;
                case VirtualizeMode.Virtualize:
                    await GotoVirtualIndex(index, verticalAlignment);
                    break;
                case VirtualizeMode.InfiniteScroll:
                case VirtualizeMode.InfiniteScrollReverse:
                    break;
                case VirtualizeMode.Pagination:
                    _currentPageNum = (int)Math.Ceiling((double)(index + 1) / PageSize);
                    await GotoPage(_currentPageNum);
                    await Refresh();
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
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, 0);
                    await GetFirstPageAsync();
                    StateHasChanged();
                    break;
                case VirtualizeMode.InfiniteScrollReverse:
                    break;
                case VirtualizeMode.Pagination:
                    _currentPageNum = 1;
                    await GotoPage(_currentPageNum);
                    break;
            }
        }

        /// <summary>
        /// Goto the end of the list. Not available if VirtualizationMode is InfiniteScroll.
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
                case VirtualizeMode.InfiniteScrollReverse:
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
                case VirtualizeMode.InfiniteScrollReverse:
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
        /// Scrolls the content by a given amount.
        /// </summary>
        /// <param name="value">Specifies the number of pixels to scroll the content vertically.</param>
        public async Task Scroll(int value)
        {
            var scrollTop = await JSRuntime.InvokeAsync<double>("window.scrollbar.GetScrollTop", _scrollViewer.Id);

            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, scrollTop + value);
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
                case VirtualizeMode.InfiniteScrollReverse:
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
                case VirtualizeMode.InfiniteScrollReverse:
                    break;
                case VirtualizeMode.Pagination:
                    if (pageNumber < 1)
                        _currentPageNum = 1;
                    //else if (pageNumber > _numPages)
                    //    _currentPageNum = _numPages;
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
                case VirtualizeMode.InfiniteScrollReverse:
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
        /// Fully refreshes the list 
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAll()
        {
            _maxScrollHeight = 0;
            _pageOffsets.Clear();
            RowSizes.Clear();
            RowIds.Clear();
            ListRows.Clear();
            await Refresh();
            RefreshAllRows();
            _header.Refresh();
        }

        /// <summary>
        /// Resets the component to its initial state. Mainly used for testing.
        /// </summary>
        /// <returns></returns>
        public async Task ResetComponent()
        {
            await GotoStart();
            _items.Clear();
            RowSizes.Clear();
            RowIds.Clear();
            ListRows.Clear();
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
                case VirtualizeMode.InfiniteScrollReverse:
                    return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollEnd", _scrollViewer.Id);
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
                case VirtualizeMode.InfiniteScrollReverse:
                    return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollStart", _scrollViewer.Id);
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
            _columnSpacing = ColumnSpacing;
            _horizontalGridLines = HorizontalGridLines;
            _verticalGridLines = VerticalGridLines;
            _stickyHeader = StickyHeader;

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
                    case VirtualizeMode.InfiniteScrollReverse:
                        break;
                    case VirtualizeMode.Pagination:
                        if (_items.Count() == 0 && !_inProgress)
                        {
                            _inProgress = true;
                            _items = await GetItems(0, PageSize);
                            _numPages = (int)Math.Ceiling((double)_totalNumItems / (double)PageSize);
                            _inProgress = false;
                        }
                        break;
                }
            _rowHeight = RowHeight;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                List<string> elementIds = new List<string>() { Id, _scrollViewer.Id,
                                                               _headerId};
                if (_grid != null)
                    elementIds.Add(_grid.Id);

                _resizeObserverId = await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds);

                if (VirtualizeMode == VirtualizeMode.InfiniteScroll ||
                    VirtualizeMode == VirtualizeMode.InfiniteScrollReverse)
                {
                    foreach (var row in RowIds)
                    {
                        if (RowSizes[row.Value].RowHeight == -1)
                            await ResizeObserverService.Service.ObserveElement(_resizeObserverId,
                                                       row.Key.ToString());
                    }
                }

                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewer.Id,
                                            DotNetObjectReference.Create(this));
            }

            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    break;
                case VirtualizeMode.Virtualize:
                    break;
                case VirtualizeMode.InfiniteScroll:
                case VirtualizeMode.InfiniteScrollReverse:
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
        private async Task ScrollCallBack(ScrollState scrollState)
        {
            try
            {
                var top = scrollState.ScrollTop;
                if (_scrollTop == top)
                    return;

                _scrollTop = top;
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        break;
                    case VirtualizeMode.Virtualize:
                        await CheckForNewRows(_scrollTop, false);
                        _header.Refresh();
                        break;
                    case VirtualizeMode.InfiniteScroll:
                    case VirtualizeMode.InfiniteScrollReverse:
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
                        _header.Refresh();
                        break;
                    case VirtualizeMode.Pagination:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            try
            {
                var top = scrollState.ScrollTop;
                if (_scrollTop == top)
                    return;

                _scrollTop = top;
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        break;
                    case VirtualizeMode.Virtualize:
                        await CheckForNewRows(_scrollTop, false);
                        _header.Refresh();
                        break;
                    case VirtualizeMode.InfiniteScroll:
                    case VirtualizeMode.InfiniteScrollReverse:
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
                        _header.Refresh();
                        break;
                    case VirtualizeMode.Pagination:
                        break;
                }
            }
            catch (Exception)
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
                    newItems = await GetItems((currentPageNum - 1) * PageSize, numToGet);
                }
                else
                {
                    numToGet = PageSize;
                    newItems = await GetItems((currentPageNum + 1) * PageSize, numToGet);
                }

                if (newItems.Count > 0)
                {
                    // Check if at end;
                    if (_items.Count == 0 || _items[_items.Count - 1].ItemIndex == newItems[newItems.Count - 1].ItemIndex)
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
            catch (Exception)
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
            if (scrollHeight + _scrollViewerHeight > _maxScrollHeight)
                _maxScrollHeight = scrollHeight + _scrollViewerHeight;
        }

        private async Task GotoVirtualIndex(int index, Alignment verticalAlignment)
        {
            double scrollTop = 0;
            var maxItemsInContainer = _scrollViewerHeight / (_rowHeight + RowSpacing);
            int headerHeight = 0;
            if (ShowHeader && (StickyHeader || index == 0))
                headerHeight = ShowHeader ? (int)_headerHeight : 0;

            switch (verticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    _skipItems = index;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * (_rowHeight + RowSpacing);
                    else
                        scrollTop = (_skipItems + 1 - maxItemsInContainer / 2) *
                                    (_rowHeight + RowSpacing);
                    break;
                case Alignment.Start:
                    if (ShowHeader && !StickyHeader)
                        _skipItems = index + 1;
                    else
                        _skipItems = index;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    scrollTop = _skipItems * (_rowHeight + RowSpacing);
                    break;
                case Alignment.End:
                    if (index < maxItemsInContainer)
                        _skipItems = 0;
                    else
                        _skipItems = (int)(index - maxItemsInContainer + 1);
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * (_rowHeight + RowSpacing);
                    else
                        if (ShowHeader)
                        scrollTop = (index - maxItemsInContainer + 2) * (_rowHeight + RowSpacing);
                    else
                        scrollTop = (index - maxItemsInContainer + 1) * (_rowHeight + RowSpacing);
                    break;
            }
            _items = await GetItems(_skipItems, _takeItems);
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, scrollTop);
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; ";
            if (BackgroundColor == null)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SurfaceContainerLow.Value}; ";
            return css;
        }

        private string GetTransformStyle()
        {
            int header = ShowHeader ? 1 : 0;
            string css = $"display:grid;  grid-template-columns: subgrid;align-content: flex-start; " +
                         $"grid-area: 1 / 1 /span {_items.Count + header} / span {Columns.Count};";

            if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
                css += $"transform: translateY({_yOffset}px);";
            else if (VirtualizeMode == VirtualizeMode.InfiniteScrollReverse)
                css += $"transform: translateY({_yOffset}px);";

            return css;
        }

        private string GetSpacerStyle()
        {
            int header = ShowHeader ? 1 : 0;
            double ht = (_skipItems + header) * (_rowHeight + RowSpacing);
            string css = $"display:grid;  grid-template-columns: subgrid; " +
                         $"grid-area: 1 / 1 /span {1 + header} / span {Columns.Count}; ";
            if (VirtualizeMode == VirtualizeMode.Virtualize)
            {
                //css += $"transform: translateY({(_skipItems + header) * (_rowHeight + RowSpacing)}px);";
                //css += $"position:relative; top:{(_skipItems + header) * (_rowHeight + RowSpacing)}px; ";

            }
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"min-height:{(_skipItems + header) * (_rowHeight + RowSpacing)}px; ";
            else
                css += $"min-height:0px; ";

            return css;
        }

        private string GetHeightDivStyle()
        {
            int header = ShowHeader ? 1 : 0;
            string css = $"display:grid;  grid-template-columns: subgrid;align-content: flex-start; " +
                         $"grid-area: 1 / 1 /span {_items.Count + header} / span {Columns.Count};";

            if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
                css += $"height:{_maxScrollHeight}px";
            else if (VirtualizeMode == VirtualizeMode.InfiniteScrollReverse)
                css += $"height:{_maxScrollHeight}px";

            return css;
        }

        protected string GetContainerStyle()
        {
            int header = ShowHeader ? 1 : 0;
            string css = $"display:grid; " +
                         $"width:{_gridWidth - ThemeManager.CurrentTheme.GetScrollBarProperties().width}px; " +
                         $" grid-template-columns: subgrid;align-content: flex-start; " +
             $"grid-area: 1 / 1 /span {_items.Count + header} / span {Columns.Count};";

            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"display:grid; position: relative;height: {_height}px";

            return css;
        }

        private string GetVerticalGridLineStyle(int column)
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.InfiniteScroll:
                case VirtualizeMode.InfiniteScrollReverse:
                case VirtualizeMode.Pagination:
                    return $"display:grid; z-index:1; width:1px; " +
                           $"border-width:0 0 0 1px; border-style:solid; " +
                           $"grid-area: 1 / {column} / span {_items.Count + 1} / span 1; " +
                           $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
                case VirtualizeMode.Virtualize:
                    return $"display:grid; z-index:1;width:1px; " +
                           $"border-width:0 0 0 1px; border-style:solid; " +
                           $"grid-area: 2 / {column} / span {_takeItems - 1} / span 1; " +
                           $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
            }
            return string.Empty;
        }

        private ScrollMode GetHorizontalScrollMode()
        {
            if (HorizontalScrollbar)
                return ScrollMode.Auto;
            else
                return ScrollMode.Disabled;
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
                        _componentWidth = observedSize.ElementWidth;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _scrollViewer.Id)
                {
                    if (observedSize.ElementHeight > 0 && _scrollViewerHeight != observedSize.ElementHeight)
                    {
                        _scrollViewerHeight = observedSize.ElementHeight;
                        _itemWidth = observedSize.ElementWidth;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _grid.Id)
                {
                    if (observedSize.ElementHeight > 0 && _gridWidth != observedSize.ElementHeight)
                    {
                        _gridWidth = observedSize.ElementWidth;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _headerId)
                {
                    if (observedSize.ElementHeight > 0 && _headerHeight != observedSize.ElementHeight)
                    {
                        _headerHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
                else
                {
                    if (RowSizes.ContainsKey(observedSize.TargetId) && observedSize.ElementHeight > 0)
                    {
                        RowSizes[observedSize.TargetId] = (observedSize.ElementHeight, 0);
                        changed = true;
                    }
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
                        if (InitialIndex != null)
                        {
                            int? index = InitialIndex?.index;
                            Alignment? alignment = InitialIndex?.verticalAlignment;
                            if (index != null && alignment != null)
                                await GotoIndex((int)index, (Alignment)alignment);
                        }
                    }
                }
                else
                {
                    if (_scrollViewerHeight > 0 && _initializing)
                    {
                        _initializing = false;
                        if (InitialIndex != null)
                        {
                            int? index = InitialIndex?.index;
                            Alignment? alignment = InitialIndex?.verticalAlignment;
                            if (index != null && alignment != null)
                                await GotoIndex((int)index, (Alignment)alignment);
                        }
                    }
                }
                StateHasChanged();
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
                case VirtualizeMode.InfiniteScrollReverse:
                    return await CheckForNewInfiniteScrollRowsAsync(scrollTop);
            }
            return false;
        }

        private async Task<bool> CheckForNewVirtualizationRows(double scrollTop, bool reload)
        {
            var skipItems = (int)Math.Floor(scrollTop / (_rowHeight + RowSpacing)) - 1;
            if (skipItems < 0)
                skipItems = 0;

            var takeItems = (int)Math.Ceiling(_scrollViewerHeight / (_rowHeight + RowSpacing)) + 2;

            if ((reload || skipItems != _skipItems || takeItems != _takeItems ||
                _items.Count == 0) && takeItems > 0)
            {
                if (_loadingUp || _loadingDown)
                {
                    _loadItemsCts?.Cancel();
                }
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
                    if (ShowHeader)
                        _height = _totalNumItems * (_rowHeight + RowSpacing) + _headerHeight;
                    else
                        _height = _totalNumItems * (_rowHeight + RowSpacing);
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