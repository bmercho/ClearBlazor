using Microsoft.AspNetCore.Components;
using ClearBlazor;
using System.Threading;

namespace ClearBlazorInternal
{
    public class ListBase<TItem> : ClearComponentBase, IBorder, IBoxShadow, IBackground
           where TItem : ListItem
    {
        /// <summary>
        /// Indicates how a list of items is Virtualized.
        /// </summary>
        [Parameter]
        public VirtualizeMode VirtualizeMode { get; set; } = VirtualizeMode.None;

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
        /// Defines what happens when the boundary of a scrolling area is reached in the vertical direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour OverscrollBehaviour { get; set; } = OverscrollBehaviour.None;

        /// <summary>
        /// Show a horizontal scrollbar if there are too many columns.. 
        /// </summary>
        [Parameter]
        public bool HorizontalScrollbar { get; set; } = false;

        /// <summary>
        /// The currently selected items. (when in Multiselect mode)
        /// </summary>
        [Parameter]
        public List<TItem> SelectedItems { get; set; } = [];

        /// <summary>
        /// The currently selected item. (when in single select mode)
        /// </summary>
        [Parameter]
        public TItem? SelectedItem { get; set; } = default;

        /// <summary>
        /// Event that is raised when the SelectedItems is changed.(when in multi select mode)
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem>> SelectedItemsChanged { get; set; }

        /// <summary>
        /// Event that is raised when the SelectedItem is changed.(when in single select mode)
        /// </summary>
        [Parameter]
        public EventCallback<TItem> SelectedItemChanged { get; set; }

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

        internal int _totalNumItems = 0;
        private int _lastSelectedRow = 0;
        internal CancellationTokenSource? _loadItemsCts;
        internal string? _resizeObserverId = null;
        internal int _skipItems = 0;
        internal int _takeItems = 0;
        internal bool _showHeader = true;
        internal Alignment _horizontalContentAlignment = Alignment.Stretch;
        internal double _iconWidth = 0;
        internal double _iconHeight = 0;
        internal GridLines _horizontalGridLines = GridLines.None;
        internal GridLines _verticalGridLines = GridLines.None;
        internal ListRowBase<TItem>? _highlightedItem = null;
        internal int _rowHeight = 30;
        private SemaphoreSlim _semaphoreSlim1 = new SemaphoreSlim(1, 1);


        protected Dictionary<Guid, TItem> _selectedItems { get; set; } = [];

        protected Dictionary<Guid, ListRowBase<TItem>> ListRows { get; set; } = [];

        //  RowId, (rowHeight, top)
        internal Dictionary<string, (double RowHeight, double Top)> RowSizes { get; set; } = [];

        // Index, RowId
        protected Dictionary<int, string> RowIds { get; set; } = [];

        protected List<TItem> _items { get; set; } = new List<TItem>();

        /// <summary>
        /// Refresh an item in the list when it has been updated. (only re-renders the given item)
        /// </summary>
        /// <returns></returns>
        public void Refresh(TItem item)
        {
            if (ListRows.ContainsKey(item.ListItemId))
                ListRows[item.ListItemId].Refresh();
        }

        /// <summary>
        /// Refresh whole list.
        /// </summary>
        /// <returns></returns>
        internal void RefreshAllRows()
        {
            foreach (var row in ListRows)
                row.Value.Refresh();
        }

        /// <summary>
        /// Removes all selections.
        /// </summary>
        /// <returns></returns>
        public async Task RemoveAllSelections()
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsSelected = false;
                Refresh(SelectedItem);
                SelectedItem = default;
                await NotifySelection();
            }

            foreach (var selection in _selectedItems.Values)
            {
                selection.IsSelected = false;
                Refresh(selection);
            }
            _selectedItems.Clear();
            await NotifySelections();
        }

        internal void SetSelectedItems()
        {
            _selectedItems.Clear();
            foreach(var item in SelectedItems)
                _selectedItems.Add(item.ListItemId, item);
        }

        internal void AddListRow(ListRowBase<TItem> listItem)
        {
            if (ListRows.ContainsKey(listItem.RowData.ListItemId))
                return;

            ListRows.Add(listItem.RowData.ListItemId, listItem);
        }

        internal void RemoveListRow(ListRowBase<TItem> listItem)
        {
            if (!ListRows.ContainsKey(listItem.RowData.ListItemId))
                return;

            ListRows.Remove(listItem.RowData.ListItemId);
        }

        internal async Task HandleRowSelection(TItem selectedItem, 
                                               int selectedIndex, 
                                               bool ctrlDown, bool shiftDown)
        {
            switch (SelectionMode)
            {
                case SelectionMode.None:
                    return;
                case SelectionMode.Single:
                    if (HandleSingleSelect(selectedItem, AllowSelectionToggle))
                        await NotifySelection();
                    break;
                case SelectionMode.SimpleMulti:
                    if (HandleSimpleMultiSelect(selectedItem))
                        await NotifySelections();
                    break;
                case SelectionMode.Multi:
                    if (await HandleMultiSelect(selectedItem, selectedIndex,
                                                ctrlDown, shiftDown))
                        await NotifySelections();
                    break;
            }

        }

        private bool HandleSingleSelect(TItem item, bool allowSelectionToggle)
        {
            if (SelectedItem != null && SelectedItem.Equals(item))
            {
                if (allowSelectionToggle)
                {
                    item.IsSelected = false;
                    Refresh(item);

                    SelectedItem = default;
                    return true;
                }
                return false;
            }
            else
            {
                if (SelectedItem != null)
                {
                    SelectedItem.IsSelected = false;
                    Refresh(SelectedItem);
                }
                SelectedItem = item;
                SelectedItem.IsSelected = true;
                Refresh(SelectedItem);

                return true;
            }
        }

        private bool HandleSimpleMultiSelect(TItem item)
        {
            if (AlreadySelected(item))
            {
                item.IsSelected = false;
                Refresh(item);
                _selectedItems.Remove(item.ListItemId);
            }
            else
            {
                item.IsSelected = true;
                Refresh(item);
                _selectedItems.Add(item.ListItemId, item);
            }
            return true;
        }

        private async Task<bool> HandleMultiSelect(TItem item,
                                                  int itemIndex,
                                                  bool ctrlDown,
                                                  bool shiftDown)
        {
            bool alreadySelected = AlreadySelected(item);

            if (!ctrlDown && !shiftDown)
            {
                _lastSelectedRow = itemIndex;
                if (!alreadySelected || _selectedItems.Count > 0)
                {
                    foreach (var item1 in _selectedItems.Values)
                    {
                        item1.IsSelected = false;
                        Refresh(item1);
                    }
                    _selectedItems.Clear();
                    item.IsSelected = true;
                    Refresh(item);
                    _selectedItems.Add(item.ListItemId, item);
                    return true;
                }
                else
                    return false;
            }
            else if (ctrlDown && !shiftDown)
            {
                if (alreadySelected)
                {
                    item.IsSelected = false;
                    Refresh(item);
                    _selectedItems.Remove(item.ListItemId);
                }
                else
                {
                    item.IsSelected = true;
                    Refresh(item);
                    _selectedItems.Add(item.ListItemId, item);
                }
                _lastSelectedRow = itemIndex;
                return true;
            }
            else
            {
                if (!ctrlDown)
                {
                    foreach (var item1 in _selectedItems.Values)
                    {
                        item1.IsSelected = false;
                        Refresh(item);
                    }
                    _selectedItems.Clear();
                }

                var range = await GetSelections(_lastSelectedRow, itemIndex);
                if (range != null && range.Count > 0)
                {
                    foreach (var item1 in range)
                    {
                        bool selected = AlreadySelected(item1);
                        if (!selected)
                        {
                            var item2 = _items.FirstOrDefault(i => i.ListItemId == item1.ListItemId);
                            if (item2 != null)
                            {
                                item2.IsSelected = true;
                                _selectedItems.Add(item2.ListItemId, item2);
                                if (ListRows.ContainsKey(item2.ListItemId))
                                    ListRows[item2.ListItemId].SetRowData(item2);
                                Refresh(item2);
                            }
                            else
                            {
                                item1.IsSelected = true;
                                _selectedItems.Add(item1.ListItemId, item1);
                                if (ListRows.ContainsKey(item1.ListItemId))
                                    ListRows[item1.ListItemId].SetRowData(item1);
                                Refresh(item1);
                            }
                        }
                    }
                }
                return true;
            }
        }

        private async Task<List<TItem>> GetSelections(int firstIndex, int secondIndex)
        {
            if (secondIndex > firstIndex)
                return await GetItems(firstIndex, secondIndex - firstIndex + 1);
            else
                return await GetItems(secondIndex, firstIndex - secondIndex + 1);
        }

        private bool AlreadySelected(TItem item)
        {
            return _selectedItems.FirstOrDefault(s => s.Key == item.ListItemId).Value != null;
        }

        private async Task NotifySelection()
        {
            await SelectedItemChanged.InvokeAsync(SelectedItem);
            StateHasChanged();
        }

        private async Task NotifySelections()
        {
            SelectedItems = _selectedItems.Values.ToList();
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
            StateHasChanged();
        }

        internal void SetHighlightedItem(ListRowBase<TItem>? row)
        {
            if (_highlightedItem != null)
                _highlightedItem.Unhighlight();
            _highlightedItem = row;
        }

        virtual internal async Task<List<TItem>> GetItems(int startIndex, int count, 
                                                          bool inReverse = false)
        {
            await _semaphoreSlim1.WaitAsync();
            try
            { 
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();

                if (startIndex + count > _totalNumItems)
                    count = _totalNumItems - startIndex;

                if (inReverse)
                        return Items.ToList().GetRange(startIndex, count).
                                                       Select((item, index) =>
                        { item.ItemIndex = startIndex + index; return item; }).Reverse().ToList();
                else
                        return Items.ToList().GetRange(startIndex, count).Select((item, index) =>
                        { item.ItemIndex = startIndex + index; return item; }).ToList();
                }
                else if (DataProvider != null)
            {
                _loadItemsCts = new CancellationTokenSource();

                try
                {
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.Select((item, index) =>
                    {
                        item.ItemIndex = startIndex + index;
                        switch (SelectionMode)
                        {
                            case SelectionMode.None:
                                break;
                            case SelectionMode.Single:
                                item.IsSelected = SelectedItem == null ? false : SelectedItem.ListItemId == item.ListItemId;
                                if (item.IsSelected)
                                    SelectedItem = item;
                                break;
                            case SelectionMode.SimpleMulti:
                            case SelectionMode.Multi:
                                item.IsSelected = _selectedItems.ContainsKey(item.ListItemId);
                                if (item.IsSelected)
                                    _selectedItems[item.ListItemId] = item;
                                break;
                        }
                        if (ListRows.ContainsKey(item.ListItemId))
                            ListRows[item.ListItemId].SetRowData(item);
                        return item;
                    }).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                    _loadItemsCts = null;
                }
            }
            return new List<TItem>();
            }
            catch (Exception ex)
            {
                return new List<TItem>();
            }
            finally
            {
                _semaphoreSlim1.Release();
            }

        }
    }
}
