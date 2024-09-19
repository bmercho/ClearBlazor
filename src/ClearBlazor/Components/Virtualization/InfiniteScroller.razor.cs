using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Virtualizes a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class InfiniteScroller<TItem> : ClearComponentBase, IBorder, IBackground, IBoxShadow
    {
        /// <summary>
        /// The child content of this control.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        ///  The items to be displayed in the list.
        /// </summary>
        //[Parameter]
        //public IEnumerable<TItem> Items { get; set; } = new List<TItem>();

        [Parameter, EditorRequired]
        public ItemsProviderRequestDelegate<TItem> ItemsProvider { get; set; } = null!;

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
        /// Gets or sets the index of the Items to be displayed in the centre of the visible area 
        /// (except if it near the start or end of list, where it wont be in the centre) 
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) VisibleIndex { get; set; } = (0, Alignment.Start);

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
        private ElementSizeInfo? _previousScrollViewerSizeInfo = null;
        private ElementSizeInfo? _scrollViewerSizeInfo = null;
        private CancellationTokenSource? _loadItemsCts;
        private bool _loadingUp = false;
        private bool _loadingDown = false;
        private bool _initialising = true;
        private double _yOffset = 0;
        private List<double> _pageOffsets = new();
        private double _paddingHeight = 0;
        // Initially just one page is loaded and when that page is scrolled to the end
        // another page is loaded (first page is kept).
        // When that page is scrolled to the bottom the top page is removed and a new page loaded.
        // From then on there are always two pages rendered.

        // _pageNum is the page index of the first page of the two. Its zero based.
        private int _pageNum = -1;
        private int _dataIndex = 0;
        private bool _atStart = true;
        private bool _atEnd = false;
        private double _maxScrollHeight = 0;
        private bool _pagingDown = true;

        private List<TItem> _items { get; set; } = new List<TItem>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            _scrollViewerSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", _scrollViewer.Id);

            bool changed = false;
            if (_previousScrollViewerSizeInfo == null ||
                !_previousScrollViewerSizeInfo.Equals(_scrollViewerSizeInfo))
            {
                changed = true;
                _previousScrollViewerSizeInfo = _scrollViewerSizeInfo;
            }

            if (_items.Count == 0 && !_loadingUp || _loadingDown)
            {
                if (await GetNextPageData())
                    changed = true;
            }

            if (_initialising)
            {
                _initialising = false;
                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewer.Id,
                                                DotNetObjectReference.Create(this));
                changed = true;
            }

            if (changed)
                StateHasChanged();
        }

        private async Task<List<TItem>> GetItems(int startIndex, int count)
        {
            _loadItemsCts ??= new CancellationTokenSource();
            try
            {
                if (startIndex < 0)
                    startIndex = 0;
                var newItems = await ItemsProvider(new ItemsProviderRequest(startIndex, count,
                                                                            _loadItemsCts.Token));

                return newItems.ToList();
            }
            catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
            {
            }
            return new List<TItem>();
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

                await GetNextPageData();
                StateHasChanged();
            }
            else
                await CalculateScrollItems(scrollState.ScrollTop);
        }

        private async Task<bool> GetNextPageData()
        {
            if (_loadingUp || _loadingDown)
                return false;
            try
            {
                _loadingDown = true;
                if (ShowLoadingSpinner)
                    StateHasChanged(); 
                var newItems = await GetItems(_dataIndex, PageSize);

                if (newItems.Count > 0)
                {

                    // Record first page offset
                    if (_pageOffsets.Count == 0)
                    {
                        _pageOffsets.Add(0);
                        _yOffset = 0;
                    }
                    else
                    {
                        _pageNum++;
                        _yOffset = _pageOffsets[_pageNum];
                    }
                    _atStart = false;
                    _dataIndex += newItems.Count;

                    // Initially only one page is loaded
                    // so in that case there is no need to extract second page
                    if (_items.Count > PageSize)
                        _items = _items.GetRange(PageSize, PageSize);
                    _items.AddRange(newItems);
                    return true;
                }
                else
                    _atEnd = true;
                return false;
            }
            finally
            {
                _loadingDown = false;
                _loadingUp = false;
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
                    if (page != _pageNum)
                    {
                        if (page < _pageNum)
                            _loadingUp = true;
                        else
                            _loadingDown = true;

                        if (ShowLoadingSpinner)
                            StateHasChanged();
                        try
                        {
                            _pageNum = page;
                            _yOffset = _pageOffsets[_pageNum];
                            _items = await GetItems(_pageNum * 10, PageSize * 2);
                        }
                        finally
                        {
                            _loadingDown = false;
                            _loadingUp = false;
                            StateHasChanged();
                        }
                    }
                    return;
                }
            }
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