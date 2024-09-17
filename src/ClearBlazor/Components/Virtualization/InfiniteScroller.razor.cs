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
        public required RenderFragment<TItem>? ChildContent { get; set; }

        /// <summary>
        ///  The items to be displayed in the list.
        /// </summary>
        //[Parameter]
        //public IEnumerable<TItem> Items { get; set; } = new List<TItem>();

        [Parameter]
        public ItemsProviderRequestDelegate<TItem>? ItemsProvider { get; set; }

        [Parameter]
        public RenderFragment? LoadingTemplate { get; set; }

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

        private bool _loading = false;
        private string _firstMarkerId = Guid.NewGuid().ToString();
        private string _middleMarkerId = Guid.NewGuid().ToString();
        private string _lastMarkerId = Guid.NewGuid().ToString();
        private DotNetObjectReference<InfiniteScroller<TItem>>? _currentComponentReference;
        private CancellationTokenSource? _loadItemsCts;
        private IJSObjectReference? _instance;
        private int _startDataIndex = 0;
        private double _offset = 0;
        private bool _atStart = true;
        private bool _atEnd = false;
        private double _pageSize;
        private List<double> _pageSizes = new();
        private int _pageNum = 0;

        private List<TItem> _items { get; set; } = new List<TItem>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _currentComponentReference = DotNetObjectReference.Create(this);
            }

            if (_items.Count > 0 && _instance == null)
                _instance = await JSRuntime.InvokeAsync<IJSObjectReference>("window.InfiniteScrolling.Initialize",
                                    _scrollViewer.Id, _firstMarkerId, _middleMarkerId,
                                    _lastMarkerId, _currentComponentReference);

            if (_items.Count == 0)
            {
                _items = await GetItems(0, PageSize * 2);

                if (_items.Count > 0)
                    StateHasChanged();
            }
        }

        [JSInvokable]
        public async Task MarkerHit(int marker)
        {
            if (_loading)
                return;

            switch (marker)
            {
                case 0:
                    if (!_atStart)
                    {
                        await HandleFirstMarker();
                    }
                    return;
                case 1:
                    if (!_atEnd)
                        await HandleLastMarker();
                    return;
            }
        }

        [JSInvokable]
        public void VirtualPageSize(double pageSize)
        {
            _pageSize = pageSize;
        }

        private async Task HandleFirstMarker()
        {
            _loading = true;
            try
            {
                var index = _startDataIndex - PageSize;

                List<TItem> newItems = new();
                if (index >= 0)  
                    newItems = await GetItems(index, PageSize);

                if (newItems.Count > 0)
                {
                    _atEnd = false;
                    _offset -= _pageSizes[_pageNum-1];
                    _pageNum--;

                    var lastPage = _items.GetRange(0, PageSize);
                    _items.Clear();
                    _items.AddRange(newItems);
                    _items.AddRange(lastPage);
                    _startDataIndex -= newItems.Count;
                    if (_startDataIndex == 0)
                        _atStart = true;
                }
                else
                    _atStart = true;

                StateHasChanged();
                await _instance.InvokeVoidAsync("onNewItems");
            }
            finally
            {
                _loading = false;
            }
        }

        private async Task HandleLastMarker()
        {
            _loading = true;
            try
            {
                var index = _startDataIndex + _items.Count;
                var newItems = await GetItems(index, PageSize);

                if (newItems.Count > 0)
                {
                    if (_pageSizes.Count == _pageNum)
                        _pageSizes.Add(_pageSize);
                    _pageNum++;

                    _atStart = false;
                    _offset += _pageSize;

                    _items = _items.GetRange(PageSize, PageSize);
                    _items.AddRange(newItems);
                    _startDataIndex += newItems.Count;
                }
                else
                    _atEnd = true;

                StateHasChanged();
                await _instance.InvokeVoidAsync("onNewItems");
            }
            finally
            {
                _loading = false;
            }
        }

        private async Task<List<TItem>> GetItems(int startIndex, int count)
        {
            _loadItemsCts ??= new CancellationTokenSource();
            try
            {
                if (startIndex < 0)
                    startIndex = 0;
                var newItems = await ItemsProvider(new ItemsProviderRequest(startIndex, count, _loadItemsCts.Token));

                return newItems.ToList();
            }
            catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
            {
            }
            return new List<TItem>();
        }


        private string GetIndicatorStyle()
        {
            return "position:absolute; height:1px;flex-shrink:0";
        }

        private string GetTransformStyle()
        {
            return $"transform: translateY({_offset}px); padding-top:{0}px;";

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