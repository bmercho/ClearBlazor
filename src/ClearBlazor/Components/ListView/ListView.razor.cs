using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class ListView<TItem> : ClearComponentBase, IBorder,
                                                     IBackground, IBoxShadow
        where TItem : ListItem
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        ///  The items to be displayed in the list. If this is not null DataProvider is used.
        ///  If DataProvider is also not null then Items takes precedence.
        /// </summary>
        [Parameter]
        public IEnumerable<TItem>? Items { get; set; }

        /// <summary>
        /// Defines the data provider used to get pages of data from where ever. eg database
        /// Used if Items is null.
        /// </summary>
        [Parameter]
        public DataProviderRequestDelegate<TItem>? DataProvider { get; set; }

        /// <summary>
        /// The currently selected items. (when in Multiselect mode)
        /// </summary>
        [Parameter]
        public List<TItem> SelectedItems { get; set; } = new();

        /// <summary>
        /// The currently selected item. (when in single select mode)
        /// </summary>
        [Parameter]
        public TItem? SelectedItem { get; set; } = default;

        /// <summary>
        /// Event that is raised when the SelectedItems is changed.(when in multi select mode)
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem>> SelectedItemsChanged { get; set; }

        /// <summary>
        /// Event that is raised when the SelectedItem is changed.(when in single select mode)
        /// </summary>
        [Parameter]
        public EventCallback<TItem> SelectedItemChanged { get; set; }

        /// <summary>
        /// The selection mode of this control. One of None, Single, SimpleMulti or Multi.
        /// </summary>
        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.None;

        /// <summary>
        /// If true, when in single selection mode, allows the selection to be toggled.
        /// </summary>
        [Parameter]
        public bool AllowSelectionToggle { get; set; } = false;

        /// <summary>
        /// If true highlights the item when it is hovered over.
        /// </summary>
        [Parameter]
        public bool HoverHighlight { get; set; } = true;

        /// <summary>
        /// The height to be used for each item.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// In this case it is optional and if not present the height is obtained from the first item.
        /// </summary>
        [Parameter]
        public int? ItemHeight { get; set; }

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);

        /// <summary>
        /// Indicates how a list of items is Virtualized.
        /// </summary>
        [Parameter]
        public VirtualizeMode VirtualizeMode { get; set; } = VirtualizeMode.None;

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


        /// <summary>
        /// Defines what happens when the boundary of a scrolling area is reached in the vertical direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour OverscrollBehaviour { get; set; } = OverscrollBehaviour.None;

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        // IBoxShadow

        /// <summary>
        /// See <a href="IBoxShadowApi">IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; }

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        private int _totalNumItems = 0;
        private bool _initializing = true;
        private ScrollViewer _scrollViewer = null!;
        private string _scrollViewerId = Guid.NewGuid().ToString();
        private CancellationTokenSource? _loadItemsCts;
        private string _baseRowId = Guid.NewGuid().ToString();
        private double _componentHeight = 0;

        // Used when VirtualizeMode is Virtualize
        private string _firstItemId = string.Empty;
        private double _height = 0;
        private double _scrollViewerHeight = 0;
        private string _resizeObserverId = string.Empty;
        private ScrollState _scrollState = new();
        private int _lastSelectedRow = 0;
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

        // Used for selection and highlighting
        private TItem? _highlightedItem = default;
        private bool _mouseOver = false;

        internal double _itemHeight = 0;
        internal double _itemWidth = 0;
        internal int _skipItems = 0;
        internal int _takeItems = 0;

        private List<TItem> _items { get; set; } = new List<TItem>();

        private Dictionary<Guid, ListViewItem<TItem>> _listItems = new();


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
                    _currentPageNum = (int)Math.Ceiling((double)(index+1) / (double)PageSize);
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
                    _items = await GetItems((_currentPageNum-1) * PageSize, PageSize);
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
                    await CalculateScrollItems(false);
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
        }

        /// <summary>
        /// Refresh an item in the list when it has been updated. (only re-renders the given item)
        /// </summary>
        /// <returns></returns>
        public void Refresh(TItem item)
        {
            if (_listItems.ContainsKey(item.Id))
                _listItems[item.Id].Refresh();
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

        /// <summary>
        /// Removes all selections.
        /// </summary>
        /// <returns></returns>
        public async Task RemoveAllSelections()
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsSelected = false;
                Refresh(SelectedItem);
                SelectedItem = null;
                await NotifySelection();
            }

            foreach(var selection in SelectedItems)
            {
                selection.IsSelected = false;
                Refresh(selection);
            }
            SelectedItems = new();
            await NotifySelections();
        }


        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                _firstItemId = _baseRowId + "0";
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
                        if (ItemHeight != null)
                            _itemHeight = ItemHeight.Value;

                        if (_items.Count() == 0)
                            _items = await GetItems(0, 1);
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
                    if (ItemHeight == null)
                        elementIds.Add(_firstItemId);
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
                    _scrollState = scrollState;
                    await CalculateScrollItems(false);
                    StateHasChanged();
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

        internal void AddListItem(ListViewItem<TItem> listItem)
        {
            if (_listItems.ContainsKey(listItem.RowData.Id))
                return;

            _listItems.Add(listItem.RowData.Id, listItem);
        }

        internal void RemoveListItem(ListViewItem<TItem> listItem)
        {
            if (!_listItems.ContainsKey(listItem.RowData.Id))
                return;

            _listItems.Remove(listItem.RowData.Id);
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
            var maxItemsInContainer = _scrollViewerHeight / _itemHeight;

            switch (verticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    _skipItems = index;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * _itemHeight;
                    else
                        scrollTop = (_skipItems - maxItemsInContainer / 2 + 0.5) * _itemHeight;
                    break;
                case Alignment.Start:
                    _skipItems = index;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    scrollTop = _skipItems * _itemHeight;
                    break;
                case Alignment.End:
                    if (index < maxItemsInContainer)
                        _skipItems = 0;
                    else
                        _skipItems = (int)(index - maxItemsInContainer + 1);
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * _itemHeight;
                    else
                        scrollTop = (index - maxItemsInContainer + 1) * _itemHeight;
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

        internal async Task HandleRowSelection(ListViewItem<TItem> selectedRow, bool ctrlDown, bool shiftDown)
        {
            switch (SelectionMode)
            {
                case SelectionMode.None:
                    return;
                case SelectionMode.Single:
                    if (HandleSingleSelect(selectedRow.RowData, AllowSelectionToggle))
                        await NotifySelection();
                    break;
                case SelectionMode.SimpleMulti:
                    if (HandleSimpleMultiSelect(selectedRow.RowData))
                        await NotifySelections();
                    break;
                case SelectionMode.Multi:
                    if (await HandleMultiSelect(selectedRow.RowData, selectedRow.RowIndex,
                                                ctrlDown, shiftDown))
                        await NotifySelections();
                    break;
            }

        }

        private bool HandleSingleSelect(TItem item, bool allowSelectionToggle)
        {
            if (SelectedItem != null && SelectedItem.Equals(item))
            {
                if (allowSelectionToggle)
                {
                    item.IsSelected = false;
                    Refresh(item);

                    SelectedItem = default;
                    return true;
                }
                return false;
            }
            else
            {
                if (SelectedItem != null)
                {
                    SelectedItem.IsSelected = false;
                    Refresh(SelectedItem);
                }
                SelectedItem = item;
                SelectedItem.IsSelected = true;
                var l = _listItems.Keys.ToList();
                Refresh(SelectedItem);

                return true;
            }
        }

        private bool HandleSimpleMultiSelect(TItem item)
        {
            if (AlreadySelected(item))
            {
                item.IsSelected = false;
                Refresh(item);
                SelectedItems.Remove(item);
            }
            else
            {
                item.IsSelected = true;
                Refresh(item);
                SelectedItems.Add(item);
            }
            return true;
        }

        private async Task<bool> HandleMultiSelect(TItem item,
                                                  int itemIndex,
                                                  bool ctrlDown,
                                                  bool shiftDown)
        {
            bool alreadySelected = AlreadySelected(item);

            if (!ctrlDown && !shiftDown)
            {
                _lastSelectedRow = itemIndex;
                if (!alreadySelected || SelectedItems.Count > 0)
                {
                    foreach (var item1 in SelectedItems)
                    {
                        item1.IsSelected = false;
                        Refresh(item1);
                    }
                    SelectedItems.Clear();
                    item.IsSelected = true;
                    Refresh(item);
                    SelectedItems.Add(item);
                    return true;
                }
                else
                    return false;
            }
            else if (ctrlDown && !shiftDown)
            {
                if (alreadySelected)
                {
                    item.IsSelected = false;
                    Refresh(item);
                    SelectedItems.Remove(item);
                }
                else
                {
                    item.IsSelected = true;
                    Refresh(item);
                    SelectedItems.Add(item);
                }
                _lastSelectedRow = itemIndex;
                return true;
            }
            else
            {
                if (!ctrlDown)
                {
                    foreach (var item1 in SelectedItems)
                    {
                        item1.IsSelected = false;
                        Refresh(item);
                    }
                    SelectedItems.Clear();
                }

                var range = await GetSelections(_lastSelectedRow, itemIndex);
                if (range != null && range.Count > 0)
                {
                    foreach (var item1 in range)
                    {
                        bool selected = AlreadySelected(item1);
                        if (!selected)
                        {
                            item1.IsSelected = true;
                            Refresh(item1);
                            SelectedItems.Add(item1);
                        }
                    }
                }
                return true;
            }
        }

        private async Task<List<TItem>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex - firstIndex + 1);
            else
                return await GetItems(secondIndex, firstIndex - secondIndex + 1);
        }

        private bool AlreadySelected(TItem item)
        {
            return SelectedItems.FirstOrDefault(s => s == item) != null;
        }

        internal async Task NotifySelection()
        {
            await SelectedItemChanged.InvokeAsync(SelectedItem);
            StateHasChanged();
        }

        internal async Task NotifySelections()
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
            StateHasChanged();
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
                else if (observedSize.TargetId == _firstItemId)
                {
                    if (observedSize.ElementHeight > 0 && _itemHeight != observedSize.ElementHeight)
                    {
                        _itemHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
                if (changed)
                {
                    if (VirtualizeMode == VirtualizeMode.Virtualize)
                    {
                        if (_scrollViewerHeight > 0 && _itemHeight > 0 && _initializing)
                        {
                            _initializing = false;
                            await CalculateScrollItems(true);
                        }
                    }
                    StateHasChanged();
                }
            }
        }

        private async Task CalculateScrollItems(bool initial)
        {
            if (initial)
            {
                _height = _totalNumItems * _itemHeight;
                await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);
            }
            else
            {
                _skipItems = (int)(_scrollState.ScrollTop / _itemHeight);
                _takeItems = (int)Math.Ceiling((double)(_scrollState.ScrollTop + _scrollViewerHeight) / _itemHeight) - _skipItems;
                _items = await GetItems(_skipItems, _takeItems);
                _height = _totalNumItems * _itemHeight;
            }
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

        private async Task<List<TItem>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();

                if (startIndex + count > _totalNumItems)
                    count = _totalNumItems - startIndex;

                return Items.ToList().GetRange(startIndex, count).Select((item, index) =>
                { item.Index = startIndex + index; return item; }).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts = new CancellationTokenSource();

                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.Select((item, index) =>
                    { item.Index = startIndex + index; return item; }).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                    _loadItemsCts = null;
                }
            }
            return new List<TItem>();
        }
    }
}