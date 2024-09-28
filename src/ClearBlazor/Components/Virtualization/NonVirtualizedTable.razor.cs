using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// TableView is a templated table component supporting virtualization and allowing multiple selections.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class NonVirtualizedTable<TItem> : ClearComponentBase, IBorder, IBackground, IBoxShadow
    {
        /// <summary>
        /// The child content of this control. Contains the columns for that table.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
        /// Is is zero based.
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) InitialIndex { get; set; } = (0, Alignment.Start);


        /// <summary>
        ///  The items to be displayed in the list. If this is not null DataProvider is used.
        ///  If DataProvider is also not null then Items takes precedence.
        /// </summary>
        [Parameter]
        public List<TItem> Items { get; set; } = new List<TItem>();

        /// <summary>
        /// Defines the data provider used to get pages of data from where ever. eg database
        /// Used if Items is null.
        /// </summary>
        [Parameter]
        public DataProviderRequestDelegate<TItem>? DataProvider { get; set; }

        /// <summary>
        /// If true it ignores ItemHeight and internally uses the InfiniteScroller component
        /// </summary>
        [Parameter]
        public VirtualizeMode VirtualizeMode { get; set; } = VirtualizeMode.None;

        [Parameter]
        public string ColumnDefs { get; set; } = "";

        [Parameter]
        public int RowSpacing { get; set; } = 5;

        [Parameter]
        public int ColumnSpacing { get; set; } = 5;

        [Parameter]
        public GridLines HorizontalGridLines { get; set; } = GridLines.None;

        [Parameter]
        public GridLines VerticalGridLines { get; set; } = GridLines.None;

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColor { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }


        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        private int _totalNumItems = 0;
        private string _firstItemId = Guid.NewGuid().ToString();
        private int _visibleIndex = 0;
        private string _scrollViewerId = Guid.NewGuid().ToString();
        private string _headerId = Guid.NewGuid().ToString();
        private bool _initialising = true;
        private double _headerHeight = 0;
        private double _itemHeight = 0;
        private double _containerHeight = 0;
        private CancellationTokenSource? _loadItemsCts;
        private string _resizeObserverId = string.Empty;

        private List<(TItem item, int index)> _items { get; set; } = new List<(TItem item, int index)>();

        private List<TableColumn<TItem>> Columns { get; } = new List<TableColumn<TItem>>();

        /// <summary>
        /// Goto the given index in the data
        /// </summary>
        /// <param name="index">Index to goto.</param>
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

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            _visibleIndex = InitialIndex.index;

            if (_items.Count() == 0)
                _items = await GetItems(0, 1);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
                _resizeObserverId = await ResizeObserverService.Service.
                                          AddResizeObserver(NotifyObservedSizes,
                                                            new List<string>() { _scrollViewerId,
                                                                                 _headerId,_firstItemId });
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; ";
            return css;
        }


        protected override void AddChild(ClearComponentBase child)
        {
            TableColumn<TItem>? column = child as TableColumn<TItem>;
            if (column != null && !Columns.Contains(column))
            {
                Columns.Add(column);
                StateHasChanged();
            }
        }

        private async Task GotoIndex(Alignment verticalAlignment)
        {
            double scrollTop = 0;
            var maxItemsInContainer = _containerHeight / (_itemHeight+RowSpacing);

            switch (verticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    scrollTop = (_visibleIndex - maxItemsInContainer / 2 + 0.5) * (_itemHeight+RowSpacing);
                    break;
                case Alignment.Start:
                    scrollTop = _visibleIndex * (_itemHeight+RowSpacing);
                    break;
                case Alignment.End:
                    scrollTop = (_visibleIndex - maxItemsInContainer + 1) * (_itemHeight+RowSpacing);
                    break;
            }
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewerId, scrollTop);
        }

        private string[] GetLines(string? content)
        {
            return content == null ? Array.Empty<string>() : content.Split('\r');
        }

        private string GetHeaderStyle(int column)
        {
            string justify = "start";
            switch (Columns[column - 1].HeaderAlignment)
            {
                case Alignment.Stretch:
                    justify = "stretch";
                    break;
                case Alignment.Start:
                    justify = "start";
                    break;
                case Alignment.Center:
                    justify = "center";
                    break;
                case Alignment.End:
                    justify = "end";
                    break;
            }
            return $"display:grid; grid-area: 1 / {column} /span 1 /span 1; justify-self: {justify};";
        }

        private string GetRowStyle()
        {
            return $"height:{Height - _headerHeight - RowSpacing}px; margin-top:5px; display:grid; " +
                   $"justify-self:stretch; overflow-x:hidden; overflow-y:auto; " +
                   $"grid-area: 2 / 1 / span 1 / span {Columns.Count}; grid-template-columns:subgrid; row-gap:{RowSpacing}px; ";
        }

        private string GetRowStyle(int row, int column)
        {
            string justify = "start";
            switch (Columns[column - 1].ContentAlignment)
            {
                case Alignment.Stretch:
                    justify = "stretch";
                    break;
                case Alignment.Start:
                    justify = "start";
                    break;
                case Alignment.Center:
                    justify = "center";
                    break;
                case Alignment.End:
                    justify = "end";
                    break;
            }
            return $"display:grid; grid-area: {row} / {column} /span 1 /span 1; justify-self: {justify};";
        }

        private string GetHorizontalGridLineStyle(int row, int columnCount)
        {
            var marginTop = -RowSpacing / 2;
            if (row == 2)
                marginTop = RowSpacing / 2;
            return $"align-self:start; border-width:1px 0 0 0; border-style:solid; margin:{marginTop}px 0 0 0; " +
                   $"grid-area: {row} / 1 / span 1 / span {columnCount}; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
        }

        private string GetVerticalGridLineStyle(int column, int rowCount)
        {
            return $"justify-self:start; border-width:0 0 0 1px; border-style:solid; margin:0 0 0 {-ColumnSpacing / 2}px; " +
                   $"grid-area: 1 / {column} / span {rowCount} / span 1; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
        }

        private async Task<List<(TItem, int)>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();

                if (count > _totalNumItems)
                    count = _totalNumItems;

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
            return new List<(TItem, int)>();
        }

        public async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            bool changed = false;
            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _scrollViewerId)
                {
                    if (observedSize.ElementHeight > 0 && _containerHeight != observedSize.ElementHeight)
                    {
                        _containerHeight = observedSize.ElementHeight;
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
                else if (observedSize.TargetId == _firstItemId)
                {
                    if (observedSize.ElementHeight > 0 && _itemHeight != observedSize.ElementHeight)
                    {
                        _itemHeight = observedSize.ElementHeight;
                        changed = true;
                    }
                }
            }
            if (changed && _containerHeight > 0 && _headerHeight > 0 && _itemHeight > 0)
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
}