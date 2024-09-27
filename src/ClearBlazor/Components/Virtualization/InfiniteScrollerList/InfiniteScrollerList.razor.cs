using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Virtualizes a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// Use this component if the item height is variable or if the total number of items is unknown.
    /// Otherwise use the VirtualizedList component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class InfiniteScrollerList<TItem> : ClearComponentBase, IBorder, IBackground, IBoxShadow,IList<TItem>
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<(TItem item,int index)>? RowTemplate { get; set; }

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
        /// Gets or sets the index of the Items to be initially shown  in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) VisibleIndex { get; set; } = (0, Alignment.Start);

        /// <summary>
        /// The horizontal content alignment within the control.
        /// </summary>
        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Stretch;

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        // IBoxShadow

        /// <summary>
        /// See <a href=IBoxShadowApi>IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; }

        /// <summary>
        /// See <a href=IBackgroundApi>IBackground</a>
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

        private List<(TItem item,int index)> _items { get; set; } = new List<(TItem, int)>();

        /// <summary>
        /// Goes to the start of the list
        /// </summary>
        public async Task GotoStart()
        {
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, 0);
            await GetFirstPage();
            StateHasChanged();
        }
        public async Task<List<(TItem, int)>> GetSelections(int firstIndex, int secondIndex)
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
                List<(TItem,int)> newItems;
                bool loadTwoPages = currentPageNum != _firstRenderedPageNum + 1;
                if (loadTwoPages || _items.Count < PageSize * 2)
                    newItems = await GetItems(currentPageNum * PageSize, PageSize*2);
                else
                    newItems = await GetItems((currentPageNum + 1) * PageSize, PageSize);

                if (newItems.Count > 0)
                {
                    _firstRenderedPageNum = currentPageNum;
                    _yOffset = _pageOffsets[_firstRenderedPageNum];

                    if (loadTwoPages || _items.Count < PageSize*2)
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

        private async Task<List<(TItem,int)>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                return Items.ToList().GetRange(startIndex, count).Select((item, index) =>
                          (item, startIndex + index)).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts = new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count,
                                                                                _loadItemsCts.Token));
                    return result.Items.Select((item, index) => (item, startIndex + index)).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                    _loadItemsCts = null;
                }
            }

            return new List<(TItem, int)>();
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

        protected string GetContentStyle()
        {
            var css = "display:grid; margin-right:5px; ";
            switch (HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += $"justify-self:stretch;";
                    break;
                case Alignment.Start:
                    css += "justify-self:start; ";
                    break;
                case Alignment.Center:
                    css += "justify-self:center; ";
                    break;
                case Alignment.End:
                    css += "justify-self:end; ";
                    break;
            }
            return css;
        }
    }
}