using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace ClearBlazor
{
    public partial class TreeView<TItem> : ClearComponentBase, IBorder,
                                                     IBackground, IBoxShadow where TItem : TreeItem<TItem>
    {
        /// <summary>
        /// Indicates how a list of items is Virtualized.
        /// </summary>
        [Parameter]
        public VirtualizeMode VirtualizeMode { get; set; } = VirtualizeMode.None;

        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? NodeTemplate { get; set; }

        /// <summary>
        /// The height to be used for each row.
        /// This is only used if the VirtualizeMode is Virtualize.
        /// </summary>
        [Parameter]
        public required int RowHeight { get; set; } = 30;


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
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);

        /// <summary>
        /// If true highlights the item when it is hovered over.
        /// </summary>
        [Parameter]
        public bool HoverHighlight { get; set; } = true;

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

        private bool _initializing = true;
        private IconButton _measureIcon = null!;
        private string _scrollViewerId = Guid.NewGuid().ToString();
        internal double _iconWidth = 0;
        private CancellationTokenSource? _loadItemsCts;
        private int _totalNumItems = 0;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private TreeViewNode<TItem>? _highlightedItem = null;
        // Used when VirtualizeMode is Virtualize
        private double _height = 0;
        private double _scrollViewerHeight = 0;
        internal double _itemWidth = 0;
        internal int _skipItems = 0;
        internal int _takeItems = 0;
        private double _scrollTop = 0;
        private bool _loadingUp = false;
        private bool _loadingDown = false;
        private double _prevScrollTop = 0;

        protected Dictionary<Guid, TreeViewNode<TItem>> TreeNodes { get; set; } = [];

        private List<(TItem item, int index)> _allNodes { get; set; } = new List<(TItem item, int index)>();

        private List<(TItem item, int index)> _visibleNodes { get; set; } = new List<(TItem item, int index)>();

        private List<(TItem item, int index)> _nodes { get; set; } = new List<(TItem item, int index)>();

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

        /// <summary>
        /// Refresh an item in the list when it has been updated. (only re-renders the given item)
        /// </summary>
        /// <returns></returns>
        public void Refresh(TItem item)
        {
            if (TreeNodes.ContainsKey(item.NodeId))
                TreeNodes[item.NodeId].Refresh();
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
                        _nodes = _allNodes;
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

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                List<string> elementIds = new List<string>() { _measureIcon.Id };

                if (VirtualizeMode == VirtualizeMode.Virtualize)
                    elementIds.Add(_scrollViewerId);
                await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds );

                await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", _scrollViewerId,
                                            DotNetObjectReference.Create(this));
            }
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
                        if (await CheckForNewVirtualizationRows(_scrollTop, false))
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
            return $"height:{Height}px; width:{Width}px; margin-top:5px; display:grid; " +
                   $"justify-self:stretch; overflow-x:hidden; overflow-y:auto; ";
        }

        internal void AddTreeNode(TreeViewNode<TItem> treeNode)
        {
            if (TreeNodes.ContainsKey(treeNode.NodeData.NodeId))
                return;

            TreeNodes.Add(treeNode.NodeData.NodeId, treeNode);
        }

        internal void RemoveTreeNode(TreeViewNode<TItem> treeNode)
        {
            if (!TreeNodes.ContainsKey(treeNode.NodeData.NodeId))
                return;

            TreeNodes.Remove(treeNode.NodeData.NodeId);
        }

        internal void SetHighlightedItem(TreeViewNode<TItem> treeNode)
        {
            if (_highlightedItem != null)
                _highlightedItem.Unhighlight();
            _highlightedItem = treeNode;
        }

        public async Task Scroll(int value)
        {
            var scrollTop = await JSRuntime.InvokeAsync<double>("window.scrollbar.GetScrollTop", _scrollViewerId);

            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId, scrollTop + value);
        }

        protected string GetContainerStyle()
        {
            if (VirtualizeMode == VirtualizeMode.Virtualize)
                return $"display:grid; position: relative;height: {_height}px";
            else
                return string.Empty;
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
                if (observedSize.TargetId == _measureIcon.Id)
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
        }

        private async Task<bool> CheckForNewVirtualizationRows(double scrollTop, bool reload)
        {
            int numItems = (int)Math.Ceiling((double)(_scrollViewerHeight/ RowHeight));
            var skipItems = (int)(scrollTop / RowHeight);
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

        private async Task<List<(TItem, int)>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = _allNodes.Where(n => n.item.IsVisible).Count();
                _height = _totalNumItems * RowHeight;

                if (startIndex + count > _totalNumItems)
                    count = _totalNumItems - startIndex;

                List<(TItem,int)> nodes = new();
                return _visibleNodes.Skip(startIndex).Take(count).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts ??= new CancellationTokenSource();
                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    _height = _totalNumItems * RowHeight;
                    return result.Items.Select((item, index) => (item, startIndex + index)).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                }
            }
            return new List<(TItem, int)>();
        }
    }
}