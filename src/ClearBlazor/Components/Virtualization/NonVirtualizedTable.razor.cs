using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// TableView is a templated table component supporting virtualization and allowing multiple selections.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class NonVirtualizedTable<TItem> : ClearComponentBase, IBorder, IBackground,IBoxShadow
    {
        /// <summary>
        /// The child content of this control. Contains the columns for that table.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the index of the Items to be initially shown in visible area.
        /// It can be shown in the centre, start or end of the visible are.
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
        private int _initialIndex = 0;
        private ScrollViewer _scrollViewer = null!;
        private string _headerId = Guid.NewGuid().ToString();
        private bool _initialising = true;
        private double? _previousHeaderHeight = null;
        private double? _headerHeight = null;
        private CancellationTokenSource? _loadItemsCts;

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
            // index is 1 based - convert to 0 based
            if (index > 0)
                _initialIndex = index - 1;
            else
                _initialIndex = 0;

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

        protected override void OnParametersSet()
        {
            base.OnParametersSetAsync();

            // index is 1 based - convert to 0 based
            if (InitialIndex.index > 0)
                _initialIndex = InitialIndex.index - 1;
            else
                _initialIndex = 0;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            bool changed = false;

            if (_initialising)
            {
                var headerSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", _headerId);

                if (headerSizeInfo == null ||
                     _previousHeaderHeight != headerSizeInfo.ElementHeight)
                {
                    if (headerSizeInfo != null)
                    {
                        _previousHeaderHeight = headerSizeInfo.ElementHeight;
                        _headerHeight = headerSizeInfo.ElementHeight;
                        changed = true;
                    }
                    StateHasChanged();
                }
            }

            if (changed && _headerHeight > 0)
            {
                if (_initialising)
                {
                    //_items = await GetItems(0, int.MaxValue);

                    _initialising = false;
                }
                StateHasChanged();
            }
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
            //double scrollTop = 0;
            //var maxItemsInContainer = _containerHeight / _itemHeight;

            //switch (verticalAlignment)
            //{
            //    case Alignment.Stretch:
            //    case Alignment.Center:
            //        scrollTop = (_initialIndex - maxItemsInContainer / 2 + 0.5) * _itemHeight;
            //        break;
            //    case Alignment.Start:
            //        scrollTop = _initialIndex * _itemHeight;
            //        break;
            //    case Alignment.End:
            //        scrollTop = (_initialIndex - maxItemsInContainer + 1) * _itemHeight;
            //        break;
            //}
            //await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", _scrollViewer.Id, scrollTop);
        }

        private string[] GetLines(string? content)
        {
            return content == null ? Array.Empty<string>() : content.Split('\r');
        }

        private string GetHeaderStyle(int column)
        {
            string justify = "start";
            switch (Columns[column-1].HeaderAlignment)
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

        private string GetRowStyle(int row, int column)
        {
            string justify = "start";
            switch (Columns[column-1].ContentAlignment)
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
            return $"align-self:start; border-width:1px 0 0 0; border-style:solid; margin:{RowSpacing/2}px 0 0 0; " +
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

    }
}