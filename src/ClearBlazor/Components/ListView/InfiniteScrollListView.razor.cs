using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a potentially infinite list of items (of type TItem), by loading more items when scrolled to the bottom. 
    /// Use this component if the item height is variable. 
    /// Otherwise use the ListView component.
    /// </summary>

    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class InfiniteScrollListView<TItem> : ClearComponentBase, IBorder, IBackground,
                                                       IBoxShadow, IList<TItem>
        where TItem : ListItem, IEquatable<TItem>
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        ///  The items to be displayed in the list. If this is null DataProvider is used.
        ///  If DataProvider and Items are not null then Items takes precedence.
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
        public List<TItem?> SelectedItems { get; set; } = new();

        /// <summary>
        /// The currently selected item. (when in single select mode)
        /// </summary>
        [Parameter]
        public TItem? SelectedItem { get; set; } = default;

        /// <summary>
        /// Event that is raised when the SelectedItems is changed.(when in multi select mode)
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem?>> SelectedItemsChanged { get; set; }

        /// <summary>
        /// Event that is raised when the SelectedItem is changed.(when in single select mode)
        /// </summary>
        [Parameter]
        public EventCallback<TItem?> SelectedItemChanged { get; set; }

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
        /// Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and 
        /// it takes some time to load the data.
        /// </summary>
        [Parameter]
        public bool ShowLoadingSpinner { get; set; } = false;

        /// <summary>
        /// Approximately the number of rows that will fit in the ScrollViewer.
        /// Adjust this until this number al least fills a page.
        /// Should be too large rather that to small.
        /// </summary>
        [Parameter]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Defines what happens when the boundary of a scrolling area is reached in the vertical direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour OverscrollBehaviour { get; set; } = OverscrollBehaviour.Auto;

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown  in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) VisibleIndex { get; set; } = (0, Alignment.Start);

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

        private ScrollViewer _scrollViewer = null!;
        private CancellationTokenSource? _loadItemsCts;
        private bool _loadingUp = false;
        private bool _loadingDown = false;
        private double _yOffset = 0;
        private List<double> _pageOffsets = new();

        // Pages are zero based. Initially just one page is loaded(page 0) and when that page is scrolled to the end
        // another page is loaded (first page is kept).
        // When that page is scrolled to the bottom the top page is removed and a new page loaded.
        // From then on there are always two pages rendered.

        private int _firstRenderedPageNum = 0;
        private double _maxScrollHeight = 0;
        private bool _hasHadData = false;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private TItem? _highlightedItem = default;
        private bool _mouseOver = false;
        private SelectionHelper<TItem> _selectionHelper = new();

        private List<TItem> _items { get; set; } = new List<TItem>();

        /// <summary>
        /// Goes to the start of the list
        /// </summary>
        public async Task GotoStart()
        {
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, 0);
            await GetFirstPage();
            StateHasChanged();
        }
        public async Task<List<TItem>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex - firstIndex + 1);
            else
                return await GetItems(secondIndex, firstIndex - secondIndex + 1);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewer.Id,
                                DotNetObjectReference.Create(this));


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
        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
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
        }

        private bool IsSelected(TItem item)
        {
            switch (SelectionMode)
            {
                case SelectionMode.None:
                    return false;
                case SelectionMode.Single:
                    if (SelectedItem != null && SelectedItem.Equals(item))
                        return true;
                    break;
                case SelectionMode.SimpleMulti:
                case SelectionMode.Multi:
                    foreach (TItem? item1 in SelectedItems)
                        if (item1 != null && item1.Equals(item))
                            return true;
                    break;
            }
            return false;
        }

        private bool IsHighlighted(TItem item)
        {
            return _mouseOver && item != null && item.Equals(_highlightedItem);
        }

        private void MouseEnter(TItem item)
        {
            _mouseOver = true;
            _highlightedItem = item;
            StateHasChanged();
        }

        private void MouseLeave()
        {
            _mouseOver = false;
            _highlightedItem = default;
            StateHasChanged();
        }
        private async Task ItemClicked(MouseEventArgs args, TItem item, int index)
        {
            bool ctrlDown = args.CtrlKey;
            bool shiftDown = args.ShiftKey;
            switch (SelectionMode)
            {
                case SelectionMode.None:
                    return;
                case SelectionMode.Single:
                    TItem? selectedItem = SelectedItem;
                    if (_selectionHelper.HandleSingleSelect(item, ref selectedItem, AllowSelectionToggle))
                    {
                        SelectedItem = selectedItem;
                        await SelectedItemChanged.InvokeAsync(SelectedItem);
                    }
                    break;
                case SelectionMode.SimpleMulti:
                    if (_selectionHelper.HandleSimpleMultiSelect(item, SelectedItems))
                    {
                        await SelectedItemsChanged.InvokeAsync(SelectedItems);
                    }
                    break;
                case SelectionMode.Multi:
                    if (await _selectionHelper.HandleMultiSelect(item, index, SelectedItems,
                                                                 this, ctrlDown, shiftDown))
                    {
                        await SelectedItemsChanged.InvokeAsync(SelectedItems);
                    }
                    break;
            }
            StateHasChanged();
        }

        private async Task<bool> GetFirstPage()
        {
            _items = await GetItems(0, PageSize);
            if (_items.Count == 0)
                return false;

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

        private async Task<bool> GetNextPageData(int currentPageNum)
        {
            if (_loadingUp || _loadingDown)
                _loadItemsCts?.Cancel();

            await semaphoreSlim.WaitAsync();
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
                semaphoreSlim.Release();
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
                        {
                            _loadItemsCts?.Cancel();

                        }
                        await semaphoreSlim.WaitAsync();
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
                            semaphoreSlim.Release();
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
                return Items.ToList().GetRange(startIndex, count).Select((item, index) =>
                       { item.Index = startIndex + index; return item; }).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts = new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
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


        private string GetTransformStyle()
        {
            return $"transform: translateY({_yOffset}px);";

        }

        private string GetHeightDivStyle()
        {
            return $"height:{_maxScrollHeight}px";
        }

        protected override string UpdateStyle(string css)
        {
            return css + $"display: grid; ";
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

            return $"display:grid; " +
                   $"justify-self:stretch; overflow-x:hidden; " +
                   $"overflow-y:auto; scrollbar-gutter:stable; {overscrollBehaviour}";
        }

        protected string GetContentStyle(TItem item)
        {
            var css = "display:grid;";
            if (HoverHighlight && IsHighlighted(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            if (IsSelected(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; ";

            return css;
        }
    }
}