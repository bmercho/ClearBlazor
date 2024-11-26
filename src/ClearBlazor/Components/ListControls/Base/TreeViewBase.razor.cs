using ClearBlazorInternal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class TreeViewBase<TItem> : ListBase<TItem>
            where TItem : TreeItem<TItem>
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? NodeTemplate { get; set; }

        /// <summary>
        /// The child content of this control. Contains the columns for that table.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// The height to be used for each row.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// </summary>
        [Parameter]
        public required int RowHeight { get; set; } = 30;

        /// <summary>
        /// The spacing between the rows.
        /// </summary>
        [Parameter]
        public int RowSpacing { get; set; } = 5;

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);

        /// <summary>
        /// The horizontal content alignment within the control.
        /// </summary>
        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Stretch;


        internal int _columnSpacing = 5;
        internal bool _stickyHeader = true;

        private bool _initializing = true;
        private double _componentHeight = 0;
        private double _componentWidth = 0;
        private IconButton _measureIcon = null!;
        private string _scrollViewerId = Guid.NewGuid().ToString();
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        // Used when VirtualizeMode is Virtualize
        private double _height = 0;
        private double _scrollViewerHeight = 0;
        internal double _itemWidth = 0;
        internal double _scrollTop = 0;
        private bool _loadingUp = false;
        private bool _loadingDown = false;
        private double _prevScrollTop = 0;

        private Grid _grid = null!;
        private double _gridWidth = 0;

        private string _headerId = Guid.NewGuid().ToString();
        internal double _headerHeight = 0;
        private TreeTableViewHeader<TItem> _header = null!;

        private List<TableColumn<TItem>> Columns { get; } = new List<TableColumn<TItem>>();
        private string _columnDefinitions = string.Empty;
        internal RenderFragment<TItem>? _rowTemplate = null;

        private List<(TItem item, int index)> _allNodes { get; set; } = new List<(TItem item, int index)>();

        private List<(TItem item, int index)> _visibleNodes { get; set; } = new List<(TItem item, int index)>();

        private List<TItem> _nodes { get; set; } = new List<TItem>();

        public async Task ExpandAll()
        {
            foreach(var node in _allNodes)
            {
                node.item.IsVisible = true;
                node.item.IsExpanded = true;
            }
            GetVisibleNodes();
            await Refresh();
        }

        public async Task CollapseAll()
        {
            foreach (var node in _allNodes)
            {
                if (node.item.Parent == null)
                    node.item.IsVisible = true;
                else
                    node.item.IsVisible = false;
                node.item.IsExpanded = false;
            }

            GetVisibleNodes();
            await Refresh();
        }

        public async Task Refresh()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    break;
                case VirtualizeMode.Virtualize:
                    GetVisibleNodes();
                    _nodes = await GetItems(_skipItems, _takeItems);
                    break;
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    break;
            }
            StateHasChanged();
        }
        public async Task FullRefresh()
        {
            switch (VirtualizeMode)
            {
                case VirtualizeMode.None:
                    break;
                case VirtualizeMode.Virtualize:
                    LoadAllNodes();
                    GetVisibleNodes();
                    _nodes = await GetItems(_skipItems, _takeItems);
                    break;
                case VirtualizeMode.InfiniteScroll:
                    break;
                case VirtualizeMode.Pagination:
                    break;
            }
            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (_nodes.Count() == 0)
            {
                LoadAllNodes();
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        _nodes = _allNodes.Select(n => n.item).ToList();
                        break;
                    case VirtualizeMode.Virtualize:
                        await CheckForNewVirtualizationRows(_scrollTop, true);
                        break;
                    case VirtualizeMode.InfiniteScroll:
                        break;
                    case VirtualizeMode.Pagination:
                        //if (_items.Count() == 0)
                        //{
                        //    _items = await GetItems(0, PageSize);
                        //    _numPages = (int)Math.Ceiling((double)_totalNumItems / (double)PageSize);
                        //}
                        break;
                }
            }
            //SetSelectedItems();
            _rowHeight = RowHeight;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                List<string> elementIds = new List<string>() { Id, _measureIcon.Id, _headerId, _grid.Id };

                if (VirtualizeMode == VirtualizeMode.Virtualize)
                    elementIds.Add(_scrollViewerId);
                await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds );

                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewerId,
                                            DotNetObjectReference.Create(this));
            }
            _horizontalContentAlignment = HorizontalContentAlignment;
        }

        protected override void AddChild(ClearComponentBase child)
        {
            TableColumn<TItem>? column = child as TableColumn<TItem>;
            if (column != null && !Columns.Contains(column))
            {
                Columns.Add(column);
                _columnDefinitions = string.Join(", ", Columns.Select(c => c.ColumnDefinition));
                StateHasChanged();
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid;";

            return css;
        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            try
            {
                switch (VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        break;
                    case VirtualizeMode.Virtualize:
                        double top = scrollState.ScrollTop;
                        if (_scrollTop == top)
                            return;

                        _scrollTop = top;
                        await CheckForNewVirtualizationRows(_scrollTop, false);
                        StateHasChanged();
                        break;
                    case VirtualizeMode.InfiniteScroll:
                        //if (Math.Ceiling(scrollState.ClientHeight + scrollState.ScrollTop) >= scrollState.ScrollHeight)
                        //{
                        //    if (scrollState.ScrollHeight > _pageOffsets[_pageOffsets.Count - 1])
                        //    {
                        //        // If only one page loaded, now that we know the offset of that page, load the second page.
                        //        // From now on two pages will always be loaded.
                        //        if (_pageOffsets.Count == 1)
                        //            await GetSecondPageAsync(scrollState.ScrollHeight);
                        //        else
                        //            await GetNextPageDataAsync(_pageOffsets.Count - 1,
                        //                                       scrollState.ScrollHeight, scrollState.ScrollTop);
                        //        StateHasChanged();
                        //    }
                        //    else
                        //        await CheckForNewRows(scrollState.ScrollTop);

                        //}
                        //else
                        //    await CheckForNewRows(scrollState.ScrollTop);

                        break;
                    case VirtualizeMode.Pagination:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetScrollViewerStyle()
        {
            return $"height:{_componentHeight}px; width:{_componentWidth}px; margin-top:5px;" +

                   $"justify-self:stretch; overflow-x:hidden; overflow-y:auto; " +
                   $"display:grid; grid-template-columns: subgrid; " +
                   $"grid-area: 1 / 1 / span 1 / span {Columns.Count}; ";
        }

        public async Task Scroll(int value)
        {
            var scrollTop = await JSRuntime.InvokeAsync<double>("window.scrollbar.GetScrollTop", _scrollViewerId);

            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId, scrollTop + value);
        }

        protected string GetContainerStyle()
        {
            int header = _showHeader ? 1 : 0;
            string css = $"display:grid; " +
                         $"width:{_gridWidth - ThemeManager.CurrentTheme.GetScrollBarProperties().width}px; " +
                         $" grid-template-columns: subgrid;align-content: flex-start; " +
                         $"grid-area: 1 / 1 / span 1 / span {Columns.Count}; ";

            if (VirtualizeMode == VirtualizeMode.Virtualize)
                css += $"position: relative;height: {_height}px";

            return css;
        }

        private void LoadAllNodes()
        {
            var index = 0;
            _allNodes.Clear();
            if (Items == null)
                return;

            foreach (var item in Items)
            {
                item.IsVisible = true;
                item.Index = index;
                AddItemAndChildren(item, ref index);
            }
            GetVisibleNodes();
            var ns = _allNodes.Select(n => n.item).ToList();
        }

        private void AddItemAndChildren(TItem item, ref int index)
        {
            _allNodes.Add((item, index));
            index++;
            foreach (var child in item.Children)
            {
                child.Parent = item;
                child.Index = index;
                child.IsVisible = item.IsExpanded;
                child.Level = item.Level + 1;
                AddItemAndChildren(child, ref index);
            }
        }

        private void GetVisibleNodes()
        {
            _visibleNodes.Clear();
            int index = 0;
            foreach (var node in _allNodes)
            {
                if (node.item.IsVisible)
                    _visibleNodes.Add((node.item, index++));
            }
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
                        _componentWidth = observedSize.ElementWidth;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _measureIcon.Id)
                {
                    if (observedSize.ElementWidth > 0 && _iconWidth != observedSize.ElementWidth)
                    {
                        _iconWidth = observedSize.ElementWidth;
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
                else if (observedSize.TargetId == _grid.Id)
                {
                    if (observedSize.ElementHeight > 0 && _gridWidth != observedSize.ElementHeight)
                    {
                        _gridWidth = observedSize.ElementWidth;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _headerId)
                {
                    if (observedSize.ElementHeight > 0 && _headerHeight != observedSize.ElementHeight)
                    {
                        _headerHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
            }
            if (changed)
            {
                if (_iconWidth > 0)
                    StateHasChanged();
                if (VirtualizeMode == VirtualizeMode.Virtualize)
                {
                    if (_scrollViewerHeight > 0 && _initializing)
                    {
                        _initializing = false;
                        await CheckForNewVirtualizationRows(_scrollTop, true);
                        //await GotoIndex(InitialIndex.index, InitialIndex.verticalAlignment);

                    }
                }
            }
        }

        private async Task<bool> CheckForNewVirtualizationRows(double scrollTop, bool reload)
        {
            int numItems = (int)Math.Ceiling((double)(_scrollViewerHeight/ (_rowHeight + RowSpacing)));
            var skipItems = (int)(scrollTop / (_rowHeight + RowSpacing));
            var takeItems = numItems+1;

            if (reload || skipItems != _skipItems || takeItems != _takeItems || _nodes.Count == 0)
            {
                if (_loadingUp || _loadingDown)
                    _loadItemsCts?.Cancel();
                await _semaphoreSlim.WaitAsync();
                try
                {
                    if (_prevScrollTop < scrollTop)
                        _loadingDown = true;
                    else
                        _loadingUp = true;

                    //if (ShowLoadingSpinner)
                    //    StateHasChanged();

                    _prevScrollTop = scrollTop;
                    _skipItems = skipItems;
                    _takeItems = takeItems;
                    _nodes = await GetItems(_skipItems, _takeItems);
                }
                finally
                {
                    _loadingDown = false;
                    _loadingUp = false;
                    _semaphoreSlim.Release();
                    StateHasChanged();
                }
                return true;
            }
            return false;
        }

        internal override async Task<List<TItem>> GetItems(int startIndex, int count, 
                                                           bool inReverse = false)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = _allNodes.Where(n => n.item.IsVisible).Count();
                _height = _totalNumItems * (_rowHeight + RowSpacing);

                if (startIndex + count > _totalNumItems)
                    count = _totalNumItems - startIndex;

                List<(TItem,int)> nodes = new();
                return _visibleNodes.Skip(startIndex).Take(count).Select(n => n.item).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts ??= new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    _height = _totalNumItems * (_rowHeight + RowSpacing);
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