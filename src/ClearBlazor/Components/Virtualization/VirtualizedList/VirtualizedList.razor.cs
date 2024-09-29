using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Virtualizes a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// Use this component if the item heights are the same
    /// (or close enough to the same such that the ItemHeight could be supplied as a parameter) and the number of items are known.
    /// Otherwise use InfiniteScrollerList component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class VirtualizedList<TItem> : ClearComponentBase,IBorder,IBackground, IBoxShadow, IList<TItem>
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
        /// The height to be used for each item.
        /// This is optional in which case the height is obtained from the first item.
        /// </summary>
        [Parameter]
        public int? ItemHeight { get; set; }

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

        private int _totalNumItems = 0;
        private string _firstItemId = Guid.NewGuid().ToString();
        private double _containerHeight = -1;
        private int _visibleIndex = 0;
        private double _height = 0;
        private double _itemWidth = 0;
        private double _itemHeight = -1;
        private int _skipItems = 0;
        private int _takeItems = 0;
        private bool _initialising = true;
        private bool _initialScroll = true;
        private ScrollState _scrollState = new();
        private ScrollViewer _scrollViewer = null!;
        private CancellationTokenSource? _loadItemsCts;
        private string _resizeObserverId = string.Empty;

        private List<(TItem item,int index)> _items { get; set; } = new List<(TItem, int)>();

        public async Task<List<(TItem,int)>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex - firstIndex + 1);
            else
                return await GetItems(secondIndex, firstIndex - secondIndex + 1);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            _visibleIndex = VisibleIndex.index;
            if (ItemHeight != null)
                _itemHeight = ItemHeight.Value;

            if (_items.Count() == 0)
                _items = await GetItems(0, 1);
        }

        /// <summary>
        /// Goto the given index in the data
        /// </summary>
        /// <param name="index">Index to goto.</param>
        /// <param name="verticalAlignment">Where the index should be aligned in the scroll viewer.</param>
        /// <returns></returns>
        public async Task GotoIndex(int index, Alignment verticalAlignment)
        {
            // index is 1 based - convert to 0 based
            if (index > 0)
                _visibleIndex = index - 1;
            else
                _visibleIndex = 0;

            await GotoIndex(verticalAlignment);
        }

        /// <summary>
        /// Refresh the list. Call this when items are added to or deleted from the data or if an item has changed 
        /// </summary>
        /// <returns></returns>
        public async Task Refresh(bool gotoEnd)
        {
            await CalculateScrollItems(false);
            if (gotoEnd)
                await GotoIndex(_totalNumItems, Alignment.End);
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_items.Count() == 0)
                return;
            
            if (RowTemplate == null)
                return;


            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewer.Id,
                                DotNetObjectReference.Create(this));
                List<string> elementIds = new List<string>();
                if (ItemHeight == null)
                    elementIds = new List<string>() { _scrollViewer.Id, _firstItemId };
                else
                    elementIds = new List<string>() { _scrollViewer.Id };
                _resizeObserverId = await ResizeObserverService.Service.
                          AddResizeObserver(NotifyObservedSizes, elementIds);
            }
        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            _scrollState = scrollState;
            await CalculateScrollItems(false);
            StateHasChanged();
        }

        protected override string UpdateStyle(string css)
        {
            return css + $"display: grid; ";
        }

        private async Task GotoIndex(Alignment verticalAlignment)
        {
            double scrollTop = 0;
            var maxItemsInContainer = _containerHeight / _itemHeight;

            switch (verticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    _skipItems = _visibleIndex;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * _itemHeight;
                    else
                        scrollTop = (_skipItems - maxItemsInContainer / 2 + 0.5) * _itemHeight;
                    break;
                case Alignment.Start:
                    _skipItems = _visibleIndex;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    scrollTop = _skipItems * _itemHeight;
                    break;
                case Alignment.End:
                    if (_visibleIndex < maxItemsInContainer)
                        _skipItems = 0;
                    else
                        _skipItems = _visibleIndex;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * _itemHeight;
                    else
                        scrollTop = (_skipItems - maxItemsInContainer + 1) * _itemHeight;
                    break;
            }
            _items = await GetItems(_skipItems, _takeItems);
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, scrollTop);
        }

        protected string GetContentStyle()
        {
            var css = "display:grid; position:absolute; margin-right:5px; ";
            switch (HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += $"justify-self:stretch; width:{_itemWidth}px; ";
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

        private async Task CalculateScrollItems(bool initial)
        {
            if (initial)
            {
                _height = _totalNumItems * _itemHeight;
                await GotoIndex(VisibleIndex.verticalAlignment);
            }
            else
            {
                _skipItems = (int)(_scrollState.ScrollTop / _itemHeight);
                _takeItems = (int)Math.Ceiling((double)(_scrollState.ScrollTop + _containerHeight) / _itemHeight) - _skipItems;
                _items = await GetItems(_skipItems, _takeItems);
                _height = _totalNumItems * _itemHeight;
            }
        }

        private async Task<List<(TItem, int)>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();
                return Items.ToList().GetRange(startIndex, count).Select((item, index) =>
                        (item, startIndex + index)).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts ??= new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.Select((item, index) => (item, startIndex + index)).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                }
            }
            return new List<(TItem,int)>();
        }

        internal async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {

            //if (changed && _containerHeight > 0 && _itemHeight > 0)
            //{
            //    if (_initialising)
            //    {
            //        _initialising = false;
            //        await CalculateScrollItems(true);
            //    }
            //    StateHasChanged();
            //}



            if (observedSizes == null)
                return;

            bool changed = false;
            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _scrollViewer.Id)
                {
                    if (observedSize.ElementHeight > 0 && _containerHeight != observedSize.ElementHeight)
                    {
                        _containerHeight = observedSize.ElementHeight;
                        _itemWidth = observedSize.ElementWidth -
                                     ThemeManager.CurrentTheme.GetScrollBarProperties().width;
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
                if (changed && _containerHeight > 0 && _itemHeight > 0)
                {
                    if (_initialising)
                    {
                        _initialising = false;
                        await CalculateScrollItems(true);
                    }
                    StateHasChanged();
                }
            }
        }
    }
}