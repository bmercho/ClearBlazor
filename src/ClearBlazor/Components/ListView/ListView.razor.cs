using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class ListView<TItem> : ClearComponentBase, IBorder,
                                                     IBackground, IBoxShadow, IList<TItem> 
        where TItem : ListItem, IEquatable<TItem>
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
        /// Defines what happens when the boundary of a scrolling area is reached in the vertical direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour OverscrollBehaviour { get; set; } = OverscrollBehaviour.Auto;

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
        private ScrollState _scrollState = new();

        // Used for selection and highlighting
        private TItem? _highlightedItem = default;
        private bool _mouseOver = false;
        private SelectionHelper<TItem> _selectionHelper = new();

        private List<TItem> _items { get; set; } = new List<TItem>();

        /// <summary>
        /// Retrieves the items (and indexes) for the given range between firstIndex and last index inclusive
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        /// <returns></returns>
        public async Task<List<TItem>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex - firstIndex + 1);
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
                case VirtualizeMode.Pagination:
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
                case VirtualizeMode.Pagination:
                    break;
            }
        }

        /// <summary>
        /// Goto the end of the list
        /// </summary>
        public async Task GotoEnd()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                case VirtualizeMode.Virtualize:
                    await GotoIndex(_totalNumItems - 1, Alignment.End);
                    break;
                case VirtualizeMode.Pagination:
                    break;
            }
        }

        /// <summary>
        /// Refresh the list. Call this when items are added to or deleted from the data or if an item has changed 
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
                case VirtualizeMode.Pagination:
                    break;
            }
        }

        /// <summary>
        /// Returns true if the list has been scrolled to the end. 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AtEnd()
        {
            return await JSRuntime.InvokeAsync<bool>("window.scrollbar.AtScrollEnd", _scrollViewerId);
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
                    case VirtualizeMode.Pagination:
                        break;
                }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    if (firstRender)
                    {
                        await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);
                        _resizeObserverId = await ResizeObserverService.Service.
                                                  AddResizeObserver(NotifyObservedSizes,
                                                  new List<string>() { _scrollViewerId });
                    }
                    break;
                case VirtualizeMode.Virtualize:
                    if (firstRender)
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

        protected string GetContentStyle(TItem item, int index)
        {
            var css = "display:grid;";
            if (VirtualizeMode == VirtualizeMode.Virtualize && _itemHeight > 0)
                css += $"position:absolute; height: {_itemHeight}px; width: {_itemWidth}px; " +
                       $"top: {(_skipItems + index) * _itemHeight}px;";
            if (HoverHighlight && IsHighlighted(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            if (IsSelected(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; ";

            return css;
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

        protected string GetContainerStyle()
        {
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                return $"display:grid; position: relative;height: {_height}px";
            else
                return string.Empty;
        }

        private async Task<List<TItem>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();

                if (count > _totalNumItems)
                    count = _totalNumItems;

                return Items.ToList().GetRange(startIndex, count).Select((item, index) => 
                            { item.Index = startIndex + index; return item; }).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts ??= new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.Select((item, index) => 
                           { item.Index = startIndex + index; return item; }).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                }
            }
            return new List<TItem>();
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
    }
}