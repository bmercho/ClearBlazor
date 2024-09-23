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
    public partial class NonVirtualizedList<TItem> : ClearComponentBase,IBorder,IBackground, IBoxShadow
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
        private double? _previousContainerHeight = null;
        private double? _previousFirstItemHeight = null;
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

        private List<TItem> _items { get; set; } = new List<TItem>();

        protected override void OnParametersSet()
        {
            base.OnParametersSetAsync();

            // index is 1 based - convert to 0 based
            if (VisibleIndex.index > 0)
                _visibleIndex = VisibleIndex.index - 1;
            else
                _visibleIndex = 0;
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
            if (gotoEnd)
                await GotoIndex(_totalNumItems, Alignment.End);
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_items.Count() == 0)
                _items = await GetItems(0, 1);

            if (_items.Count() == 0)
                return;

            if (RowTemplate == null)
                return;

            bool changed = false;
            var containerSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", _scrollViewer.Id);

            if (containerSizeInfo == null ||
                _previousContainerHeight != containerSizeInfo.ElementHeight)
            {
                if (containerSizeInfo != null)
                {
                    _previousContainerHeight = containerSizeInfo.ElementHeight;
                    _containerHeight = containerSizeInfo.ElementHeight;
                    _itemWidth = containerSizeInfo.ElementWidth - ThemeManager.CurrentTheme.GetScrollBarProperties().width;
                    changed = true;
                }
                StateHasChanged();
            }

            if (_initialising)
            {
                var firstItemSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", _firstItemId);

                if (firstItemSizeInfo == null ||
                     _previousFirstItemHeight != firstItemSizeInfo.ElementHeight)
                {
                    if (firstItemSizeInfo != null)
                    {
                        _previousFirstItemHeight = firstItemSizeInfo.ElementHeight;
                        _itemHeight = firstItemSizeInfo.ElementHeight;
                        changed = true;
                    }
                    StateHasChanged();
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

        private async Task<List<TItem>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();
                return Items.Skip(startIndex).Take(count).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts ??= new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                }
            }
            return new List<TItem>();
        }

    }
}