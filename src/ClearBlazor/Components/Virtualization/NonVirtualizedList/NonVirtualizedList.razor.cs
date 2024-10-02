using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// Use this component if virtualization is not required.
    /// Otherwise use VirtualizedList or InfiniteScrollerList component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class NonVirtualizedList<TItem> : ClearComponentBase,IBorder,
                                                     IBackground, IBoxShadow, IList<TItem>
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<(TItem rowData,int rowIndex)>? RowTemplate { get; set; }

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
        /// The height to be used for each item.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// In this case it is optional in which case the height is obtained from the first item.
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
        /// The horizontal content alignment within the control.
        /// </summary>
        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Stretch;

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
        private string _scrollViewerId = Guid.NewGuid().ToString();
        private CancellationTokenSource? _loadItemsCts;
        private string _baseRowId = Guid.NewGuid().ToString();

        // Used when VirtualizeMode is Virtualize
        private string _firstItemId = string.Empty;
        private double _height = 0;
        private double _itemHeight = 0;
        private double _itemWidth = 0;
        private double _scrollViewerHeight = 0;
        private string _resizeObserverId = string.Empty;
        private int _skipItems = 0;
        private int _takeItems = 0;
        //private bool _initialScroll = true;
        private ScrollState _scrollState = new();

        private List<(TItem item,int index)> _items { get; set; } = new List<(TItem item,int index)>();

        /// <summary>
        /// Retrieves the items (and indexes) for the given range between firstIndex and last index inclusive
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        /// <returns></returns>
        public async Task<List<(TItem,int)>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex-firstIndex + 1);
            else
                return await GetItems(secondIndex, firstIndex - secondIndex + 1);
        }

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
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.ScrollIntoView", _scrollViewerId,
                                                    _baseRowId + index, (int)verticalAlignment);
                    break;
                case VirtualizeMode.Virtualize:
                    await GotoVirtualIndex(index, verticalAlignment);
                    break;
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    break;
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
                        _skipItems = (int)(index - maxItemsInContainer);
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop =  _skipItems * _itemHeight;
                    else
                        scrollTop = (index - maxItemsInContainer + 1) * _itemHeight;
                    break;
            }
            _items = await GetItems(_skipItems, _takeItems);
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId, scrollTop);
        }

        /// <summary>
        /// Goto the start of the list
        /// </summary>
        public async Task GotoStart()
        {
            await GotoIndex(0, Alignment.Start);
        }

        /// <summary>
        /// Goto the end of the list
        /// </summary>
        public async Task GotoEnd()
        {
            await GotoIndex(_totalNumItems - 1, Alignment.End);
        }

        /// <summary>
        /// Refresh the list. Call this when items are added to or deleted from the data or if an item has changed 
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
            if (VirtualizeMode == VirtualizeMode.None)
                _items = await GetItems(0, int.MaxValue);
            else
                await CalculateScrollItems(false);
            StateHasChanged();
        }

        /// <summary>
        /// Returns true if the list has been scrolled to the end. 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AtEnd()
        {
            return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollEnd", _scrollViewerId,
                                            _baseRowId + (_totalNumItems-1).ToString());
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
                if (VirtualizeMode == VirtualizeMode.Virtualize)
                {
                    if (ItemHeight != null)
                        _itemHeight = ItemHeight.Value;

                    if (_items.Count() == 0)
                        _items = await GetItems(0, 1);
                }
                else
                    _items = await GetItems(0, int.MaxValue);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (VirtualizeMode == VirtualizeMode.Virtualize)
                {
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewerId,
                DotNetObjectReference.Create(this));
                    List<string> elementIds = new List<string>();
                    if (ItemHeight == null)
                        elementIds = new List<string>() { _scrollViewerId, _firstItemId };
                    else
                        elementIds = new List<string>() { _scrollViewerId };
                    _resizeObserverId = await ResizeObserverService.Service.
                              AddResizeObserver(NotifyObservedSizes, elementIds);
                }
                else
                    await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);
            }
        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            if (VirtualizeMode == VirtualizeMode.Virtualize)
            {
                _scrollState = scrollState;
                await CalculateScrollItems(false);
                StateHasChanged();
            }
        }

        protected override string UpdateStyle(string css)
        {
            return css + $"display: grid; ";
        }

        private string GetScrollViewerStyle()
        {
            return $"height:{Height}px; width:{Width}px; margin-top:5px; display:grid; " +
                   $"justify-self:stretch; overflow-x:hidden; overflow-y:auto; ";
        }

        protected string GetContentStyle(int index)
        {
            var css = "display:grid;";
            if (VirtualizeMode == VirtualizeMode.Virtualize && _itemHeight > 0)
                css += $"position:absolute; height: {_itemHeight}px; top: {(_skipItems + index) * _itemHeight}px;";
            switch (HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += $"justify-self:stretch; width:{Width}px; ";
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

        protected string GetContainerStyle()
        {
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                return $"display:grid; position: relative;height: {_height}px";
            else
                return string.Empty;
        }

        private async Task<List<(TItem,int)>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();

                if (count > _totalNumItems)
                    count = _totalNumItems;

                return Items.ToList().GetRange(startIndex, count).Select((item, index) => 
                              ( item, startIndex + index )).ToList();

            }
            else if (DataProvider != null)
            {
                _loadItemsCts ??= new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.Select((item,index) => (item,startIndex+index)).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                }
            }
            return new List<(TItem,int)>();
        }

        internal async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            bool changed = false;
            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _scrollViewerId)
                {
                    if (observedSize.ElementHeight > 0 && _scrollViewerHeight != observedSize.ElementHeight)
                    {
                        _scrollViewerHeight = observedSize.ElementHeight;
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
                if (changed && _scrollViewerHeight > 0 && _itemHeight > 0)
                {
                    if (_initializing)
                    {
                        _initializing = false;
                        await CalculateScrollItems(true);
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

    }
}