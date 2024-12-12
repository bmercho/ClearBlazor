using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ClearBlazorInternal;
using System.ComponentModel.Design;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class ListView1<TItem> : ListBase<TItem>
        where TItem : ListItem
    {
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
        /// Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and 
        /// it takes some time to load the data.
        /// </summary>
        [Parameter]
        public bool ShowLoadingSpinner { get; set; } = false;

        /// <summary>
        /// Approximately the number of rows that will fit in the ScrollViewer.
        /// Adjust this until this number at least fills a page.
        /// Should be too large rather that to small.
        /// Only used if VirtualizationMode is InfiniteScroll or Pagination.
        /// </summary>
        [Parameter]
        public int PageSize { get; set; } = 10;

        private bool _initializing = true;
        private string _scrollViewerId = Guid.NewGuid().ToString();
        //private ScrollViewer _scrollViewer = null!;
        private string _baseRowId = Guid.NewGuid().ToString();
        private double _componentHeight = 0;
        private double _componentWidth = 0;

        // Used when VirtualizeMode is Virtualize
        private double _height = 0;
        private double _totalHeight = 0;
        private double _scrollViewerHeight = 0;
        private double _scrollTop = 0;
        private bool _loadingUp = false;
        private bool _loadingDown = false;
        private double _prevScrollTop = 0;
        private bool _firstPage = true;
        // For VirtualizeMode InfiniteScroll
        // Pages are zero based. Initially just one page is loaded(page 0) and when that page is scrolled to the end
        // another page is loaded (first page is kept).
        // When that page is scrolled to the bottom the top page is removed and a new page loaded.
        // From then on there are always two pages rendered.

        // Used when VirtualizeMode is InfiniteScroll
        private int _firstRenderedPageNum = 0;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private double _averageRowHeight = 0;
        // Used when VirtualizeMode is Pagination
        private int _currentPageNum = 1;
        private int _numPages = 0;

        internal double _itemWidth = 0;

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
                case VirtualizeMode.InfiniteScrollReverse:
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
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop",
                                                    _scrollViewerId, 0);
                    await GetFirstTwoPagesAsync();
                    StateHasChanged();
                    break;
                case VirtualizeMode.InfiniteScrollReverse:
                    await GetFirstTwoPagesAsync();
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop",
                                                    _scrollViewerId, _totalHeight);
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

        public async Task Scroll(int value)
        {
            var scrollTop = await JSRuntime.InvokeAsync<double>("window.scrollbar.GetScrollTop", 
                                                                _scrollViewerId);

            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", 
                                             _scrollViewerId, scrollTop + value);
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
                    await GetCurrentPagesAsync();
                    StateHasChanged();
                    break;
                case VirtualizeMode.Pagination:
                    await GotoPage(_currentPageNum);
                    StateHasChanged();
                    break;
            }
        }

        /// <summary>
        /// Indicates that a row has been added to the top of the list.
        /// Ony used if VirtualizationMode is InfiniteScroll or InfiniteScrollReverse
        /// </summary>
        /// <param name="listItemId"></param>
        /// <returns></returns>
        public async Task RowAdded(Guid listItemId)
        {
            if (VirtualizeMode != VirtualizeMode.InfiniteScroll &&
                VirtualizeMode != VirtualizeMode.InfiniteScrollReverse)
                return;

            RowSizes.Add(listItemId.ToString(), (_averageRowHeight, 0));


            RowIds.Add(RowIds.Count, RowIds[RowIds.Count - 1]);

            for (int i = RowIds.Count - 2; i >= 0; i--)
            {
                var rowId = RowIds[i];
                var nextRowId = RowIds[i + 1];
                RowSizes[nextRowId] = RowSizes[rowId];
            }

            RowIds[0] = listItemId.ToString();
            CalculateTops();
            await Refresh();
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
                case VirtualizeMode.InfiniteScrollReverse:
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
                        await GetFirstTwoPagesAsync();
                        break;
                    case VirtualizeMode.InfiniteScrollReverse:
                        await GetFirstTwoPagesAsync();
                        break;
                    case VirtualizeMode.Pagination:
                        await GetFirstPageAsync();
                        break;
                }
            SetSelectedItems();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                List<string> elementIds = new List<string>() { Id, _scrollViewerId };

                _resizeObserverId = await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds);


                foreach (var row in RowSizes)
                {
                    if (row.Value.RowHeight == 0)
                        await ResizeObserverService.Service.ObserveElement(_resizeObserverId,
                                                                           row.Key);
                }

                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents",
                                            _scrollViewerId,
                                            DotNetObjectReference.Create(this)); 
            }

            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                case VirtualizeMode.InfiniteScroll:
                case VirtualizeMode.InfiniteScrollReverse:
                    break;
                case VirtualizeMode.Pagination:
                    break;
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
                        if (scrollState.ScrollTop > _totalHeight - _scrollViewerHeight)
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
                            await GetNextPageDataAsync(scrollState.ScrollTop);
                            StateHasChanged();
                        }
                        else
                        {
                            await CheckForNewRows(scrollState.ScrollTop);
                        }
                        break;
                    case VirtualizeMode.InfiniteScrollReverse:
                        if (scrollState.ScrollHeight+scrollState.ScrollTop < scrollState.ClientHeight+5)
                        {
                            await GetNextPageDataAsync(scrollState.ScrollTop);
                            StateHasChanged();
                        }
                        else
                        {
                            await CheckForNewRows(scrollState.ScrollTop);
                        }
                        break;
                    case VirtualizeMode.Pagination:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<bool> GetFirstPageAsync()
        {
            _skipItems = 0;
            _items = await GetItems(0, PageSize);
            if (_items.Count == 0)
                return false;
            return true;
        }

        private async Task<bool> GetFirstTwoPagesAsync()
        {
            _skipItems = 0;
            _firstRenderedPageNum = 0;
            _items = await GetItems(_skipItems, PageSize * 2);
            if (_items.Count == 0)
                return false;
            _height = 0;

            foreach (var item in _items)
                AddRow(item.ListItemId.ToString(), item.ItemIndex);

            return true;
        }

        private async Task GetCurrentPagesAsync()
        {
            _items = await GetItems(_skipItems, PageSize * 2);
        }

        private async Task<bool> GetNextPageDataAsync(double scrollTop)
        {
            if (_loadItemsCts != null && (_loadingUp || _loadingDown))
                await _loadItemsCts.CancelAsync();

            await _semaphoreSlim.WaitAsync();
            try
            {
                _loadingDown = true;
                if (ShowLoadingSpinner)
                    StateHasChanged();

                var newItems = await GetItems(RowSizes.Count, PageSize);
                if (newItems.Count > 0)
                {
                    // Check if at end;
                    if (_items.Count == 0 || _items[_items.Count - 1].ItemIndex == newItems[newItems.Count - 1].ItemIndex)
                        return false;

                    foreach (var item in _items.GetRange(PageSize, PageSize))
                        if (_resizeObserverId != null)
                            await ResizeObserverService.Service.UnobserveElement(_resizeObserverId,
                                                                                 item.ListItemId.ToString());
                    _items = _items.GetRange(PageSize, PageSize);
                    _items.AddRange(newItems);
                    var rowId = RowIds[RowSizes.Count - PageSize];
                    var row = RowSizes[rowId];
                    _height = row.Top;

                    _skipItems = RowSizes.Count + PageSize;
                    _firstRenderedPageNum++;

                    foreach (var item in newItems)
                        AddRow(item.ListItemId.ToString(), item.ItemIndex);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
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

        internal void AddRow(string id, int index)
        {
            if (!RowSizes.ContainsKey(id))
            {
                RowIds.Add(index, id);
                RowSizes.Add(id, (0, 0));
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
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop",
                                            _scrollViewerId, scrollTop);
        }

        protected override string UpdateStyle(string css)
        {
            return css + $"display: grid; ";
        }

        private string GetScrollViewerStyle()
        {
            string css = string.Empty;
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

            css += $"display:grid; height:{_componentHeight}px; " +
                   $"justify-self:stretch; overflow-x:hidden; " +
                   $"overflow-y:auto; {overscrollBehaviour}";

            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                case VirtualizeMode.Pagination:
                    break;
                case VirtualizeMode.InfiniteScroll:
                    css += "display: flex; flex-direction: column; ";
                    break;
                case VirtualizeMode.InfiniteScrollReverse:
                    css += "display: flex; flex-direction: column-reverse;";
                    // Do not delete the background color below. It somehow fixes a reverse
                    // infinite scrolling issue "
                    css += "background-color: #ffffffff; ";
                    break;
            }

            return css;
        }

        protected string GetContainerStyle()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Pagination:
                    return string.Empty;
                case VirtualizeMode.Virtualize:
                    return $"display:grid; position: relative;height: {_totalHeight}px; ";
                case VirtualizeMode.InfiniteScroll:
                    return $"height:{_totalHeight}px; min-height:{_totalHeight}px; ";
                case VirtualizeMode.InfiniteScrollReverse:
                    return $"height:{_totalHeight}px; min-height:{_totalHeight}px; " +
                           $"display: flex; flex-direction: column-reverse; ";
            }

            return string.Empty;
        }

        private string GetHeightDivStyle()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Pagination:
                case VirtualizeMode.Virtualize:
                    return string.Empty;
                case VirtualizeMode.InfiniteScroll:
                case VirtualizeMode.InfiniteScrollReverse:
                    return $"height:{_height}px; min-height:{_height}px; ";
            }

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
                        _componentWidth = observedSize.ElementWidth;
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
                else
                {
                    if (RowSizes.ContainsKey(observedSize.TargetId) && observedSize.ElementHeight > 0)
                    {
                        if (RowSizes[observedSize.TargetId].RowHeight != observedSize.ElementHeight)
                        {
                            RowSizes[observedSize.TargetId] = (observedSize.ElementHeight, 0);
                            CalculateTops();
                            changed = true;
                        }
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
                        await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);

                    }
                }
                if (VirtualizeMode == VirtualizeMode.None)
                {
                    if (_scrollViewerHeight > 0 && _initializing)
                    {
                        _initializing = false;
                        await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);
                    }
                }

                if (VirtualizeMode == VirtualizeMode.InfiniteScroll ||
                    VirtualizeMode == VirtualizeMode.InfiniteScrollReverse)
                    CalculateTops();

                //DisplayTops();
                StateHasChanged();
            }
        }

        private void CalculateTops()
        {
            double top = 0;
            for (var i = 0; i < RowSizes.Count; i++)
            {
                var rowId = RowIds[i];
                var row = RowSizes[rowId];
                RowSizes[rowId] = (row.RowHeight, top);
                top += row.RowHeight;
            }
            var num = RowSizes.Count;
            var rowId1 = RowIds[num - 1];
            _totalHeight = RowSizes[rowId1].Top + RowSizes[rowId1].RowHeight;
            _averageRowHeight = _totalHeight / num;
        }

        private void DisplayTops()
        {
            for (var i = 0; i < RowSizes.Count; i++)
            {
                var rowId = RowIds[i];
                var row = RowSizes[rowId];
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
                    _totalHeight = _totalNumItems * RowHeight;
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
            try
            {
                int numPages = (int)Math.Ceiling(RowSizes.Count / (double)PageSize);
                int page = 0;
                for (int i = 0; i < RowSizes.Count; i++)
                {
                    if (i == RowSizes.Count - 1)
                        return false;
                    var rowId = RowIds[i];
                    var nextRowId = RowIds[i + 1];
                    if (VirtualizeMode == VirtualizeMode.InfiniteScroll)
                    {
                        if (scrollTop >= RowSizes[rowId].Top && scrollTop <= RowSizes[nextRowId].Top)
                        {
                            if (page != _firstRenderedPageNum && page < numPages - 1)
                                return await LoadInfiniteScrollPageAsync(page);
                            else
                                break;
                        }
                    }
                    else
                    {
                        if (-scrollTop >= RowSizes[rowId].Top && -scrollTop <= RowSizes[nextRowId].Top)
                        {
                            if (page != _firstRenderedPageNum && page < numPages - 1)
                                return await LoadInfiniteScrollPageAsync(page);
                            else
                                break;
                        }
                    }
                    if (i % PageSize == PageSize - 1)
                        page++;

                }
            }
            catch (Exception ex)
            {
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

                _skipItems = page * PageSize;
                var items = await GetItems(_skipItems, PageSize * 2);

                if (items.Count > 0)
                {
                    _firstRenderedPageNum = page;
                    var rowId = RowIds[_skipItems];
                    var row = RowSizes[rowId];
                    _height = row.Top;
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