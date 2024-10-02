using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class NonVirtualizedTree<TItem> : ClearComponentBase, IBorder,
                                                     IBackground, IBoxShadow where TItem : TreeItem<TItem>
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<(TItem rowData, int rowIndex)>? RowTemplate { get; set; }

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

        private string _scrollViewerId = Guid.NewGuid().ToString();
        private string _baseRowId = Guid.NewGuid().ToString();
        private CancellationTokenSource? _loadItemsCts;
        private int _totalNumItems = 0;

        private List<(TItem item, int index)> _items { get; set; } = new List<(TItem item, int index)>();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (_items.Count() == 0)
                _items = await GetItems(0, int.MaxValue);

        }

        private string GetScrollViewerStyle()
        {
            return $"height:{Height}px; width:{Width}px; margin-top:5px; display:grid; " +
                   $"justify-self:stretch; overflow-x:hidden; overflow-y:auto; " +
                   $"grid-template-columns: auto 1fr;  grid-auto-rows: min-content; ";
        }

        protected string GetContentStyle(TItem item, int index)
        {
            var css = $"margin-left:{item.Level * 20}px; align-self:center; display:grid; grid-area: {index + 1} / 2 / span 1 / span 1; " +
                      $"background-color:transparent; ";
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

        protected string GetExpandStyle(TItem item, int index)
        {
            var css = $"margin-left:{item.Level * 20}px; grid-area: {index + 1} / 1 / span 1 / span 1;align-self:center; ";
            return css;
        }


        public async Task OnRowClicked(MouseEventArgs e, TItem item)
        {
            if (item.HasChildren)
            {
                item.Expanded = !item.Expanded;
                foreach (var child in item.Children)
                {
                    if (item.Expanded)
                        MakeVisible(child);
                    else
                        MakeInvisible(child);
                }
            }
            StateHasChanged();
            await Task.CompletedTask;
        }

        private void MakeVisible(TItem item)
        {
            if (item.Parent != null)
                if (item.Parent.Expanded)
                {
                    item.IsVisible = true;
                    foreach (var child in item.Children)
                        MakeVisible(child);
                }
        }

        private void MakeInvisible(TItem item)
        {
            item.IsVisible = false;
            foreach (var child in item.Children)
                MakeInvisible(child);
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

                var index = 0;
                _items.Clear();
                foreach (var item in Items)
                {
                    item.IsVisible = true;
                    AddItemAndChildren(item, ref index);
                }
                var its = _items.Select(i => i.item).ToList();
                var ins = _items.Select(i => i.index).ToList();
                return _items;
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


        private void AddItemAndChildren(TItem item, ref int index)
        {
            _items.Add((item, index));
            index++;
            foreach (var child in item.Children)
            {
                child.Parent = item;
                child.IsVisible = item.Expanded;
                child.Level = item.Level + 1;
                AddItemAndChildren(child, ref index);
            }
        }
    }
}