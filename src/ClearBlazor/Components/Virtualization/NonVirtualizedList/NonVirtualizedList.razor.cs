using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// Use this component if virtualization is not required, the item heights are the same 
    /// and the number of items are known.
    /// Otherwise use VirtualizedList or InfiniteScrollerList component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class NonVirtualizedList<TItem> : ClearComponentBase,IBorder,
                                                     IBackground, IBoxShadow, IList<TItem>, IAsyncDisposable
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<(TItem item,int index)>? RowTemplate { get; set; }

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
        /// See <a href="IBorderApi">IBorder</a>
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
        private double _itemWidth = 0;
        private double _itemHeight = -1;
        private bool _initialising = true;
        private ScrollViewer _scrollViewer = null!;
        private CancellationTokenSource? _loadItemsCts;
        private string _resizeObserverId = string.Empty;

        private List<(TItem item,int index)> _items { get; set; } = new List<(TItem item,int index)>();

        public async Task<List<(TItem,int)>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex-firstIndex + 1);
            else
                return await GetItems(secondIndex, firstIndex - secondIndex + 1);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            _visibleIndex = VisibleIndex.index;

            if (_items.Count() == 0)
                _items = await GetItems(0, 1);
        }

        /// <summary>
        /// Goto the given index in the data
        /// </summary>
        /// <param name="index">Index to goto. The index is zero bassed.</param>
        /// <param name="verticalAlignment">Where the index should be aligned in the scroll viewer.</param>
        /// <returns></returns>
        public async Task GotoIndex(int index, Alignment verticalAlignment)
        {
            _visibleIndex = index;

            await GotoIndex(verticalAlignment);
        }

        /// <summary>
        /// Refresh the list. Call this when items are added to or deleted from the data or if an item has changed 
        /// </summary>
        /// <returns></returns>
        public async Task Refresh(bool gotoEnd)
        {
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
                _resizeObserverId = await ResizeObserverService.Service.
                                          AddResizeObserver(NotifyObservedSizes,
                                                            new List<string>() { _scrollViewer.Id, _firstItemId });
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
                    scrollTop = (_visibleIndex - maxItemsInContainer / 2 + 0.5) * _itemHeight;
                    break;
                case Alignment.Start:
                    scrollTop = _visibleIndex * _itemHeight;
                    break;
                case Alignment.End:
                    scrollTop = (_visibleIndex - maxItemsInContainer + 1) * _itemHeight;
                    break;
            }
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, scrollTop);
        }

        protected string GetContentStyle()
        {
            var css = "display:grid; margin-right:5px; ";
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

        public async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
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
                        _items = await GetItems(0, int.MaxValue);

                        _initialising = false;
                    }
                    StateHasChanged();
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await ResizeObserverService.Service.RemoveResizeObserver(_resizeObserverId);
        }

    }
}