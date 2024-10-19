using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class ListBase<TItem> : ClearComponentBase, IBorder, IBackground, IBoxShadow
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
        /// The currently selected items. (when in Multiselect mode)
        /// </summary>
        [Parameter]
        public List<TItem> SelectedItems { get; set; } = new();

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
        public Color? BackgroundColor { get; set; } = ThemeManager.CurrentPalette.Background;

        internal int _totalNumItems = 0;
        private int _lastSelectedRow = 0;
        internal CancellationTokenSource? _loadItemsCts;

        protected Dictionary<Guid, ListRowBase<TItem>> ListRows { get; set; } = new();

        /// <summary>
        /// Refresh an item in the list when it has been updated. (only re-renders the given item)
        /// </summary>
        /// <returns></returns>
        public void Refresh(TItem item)
        {
            if (ListRows.ContainsKey(item.Id))
                ListRows[item.Id].Refresh();
        }

        /// <summary>
        /// Refresh an item in the list when it has been updated. (only re-renders the given item)
        /// </summary>
        /// <returns></returns>
        internal void RefreshAll()
        {
            foreach(var row in ListRows)
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
                SelectedItem = null;
                await NotifySelection();
            }

            foreach (var selection in SelectedItems)
            {
                selection.IsSelected = false;
                Refresh(selection);
            }
            SelectedItems = new();
            await NotifySelections();
        }

        internal void AddListRow(ListRowBase<TItem> listItem)
        {
            if (ListRows.ContainsKey(listItem.RowData.Id))
                return;

            ListRows.Add(listItem.RowData.Id, listItem);
        }

        internal void RemoveListRow(ListRowBase<TItem> listItem)
        {
            if (!ListRows.ContainsKey(listItem.RowData.Id))
                return;

            ListRows.Remove(listItem.RowData.Id);
        }

        internal async Task HandleRowSelection(ListRowBase<TItem> selectedRow, bool ctrlDown, bool shiftDown)
        {
            switch (SelectionMode)
            {
                case SelectionMode.None:
                    return;
                case SelectionMode.Single:
                    if (HandleSingleSelect(selectedRow.RowData, AllowSelectionToggle))
                        await NotifySelection();
                    break;
                case SelectionMode.SimpleMulti:
                    if (HandleSimpleMultiSelect(selectedRow.RowData))
                        await NotifySelections();
                    break;
                case SelectionMode.Multi:
                    if (await HandleMultiSelect(selectedRow.RowData, selectedRow.RowIndex,
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
                var l = ListRows.Keys.ToList();
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
                SelectedItems.Remove(item);
            }
            else
            {
                item.IsSelected = true;
                Refresh(item);
                SelectedItems.Add(item);
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
                if (!alreadySelected || SelectedItems.Count > 0)
                {
                    foreach (var item1 in SelectedItems)
                    {
                        item1.IsSelected = false;
                        Refresh(item1);
                    }
                    SelectedItems.Clear();
                    item.IsSelected = true;
                    Refresh(item);
                    SelectedItems.Add(item);
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
                    SelectedItems.Remove(item);
                }
                else
                {
                    item.IsSelected = true;
                    Refresh(item);
                    SelectedItems.Add(item);
                }
                _lastSelectedRow = itemIndex;
                return true;
            }
            else
            {
                if (!ctrlDown)
                {
                    foreach (var item1 in SelectedItems)
                    {
                        item1.IsSelected = false;
                        Refresh(item);
                    }
                    SelectedItems.Clear();
                }

                var range = await GetSelections(_lastSelectedRow, itemIndex);
                if (range != null && range.Count > 0)
                {
                    foreach (var item1 in range)
                    {
                        bool selected = AlreadySelected(item1);
                        if (!selected)
                        {
                            item1.IsSelected = true;
                            Refresh(item1);
                            SelectedItems.Add(item1);
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
            return SelectedItems.FirstOrDefault(s => s == item) != null;
        }

        private async Task NotifySelection()
        {
            await SelectedItemChanged.InvokeAsync(SelectedItem);
            StateHasChanged();
        }

        private async Task NotifySelections()
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
            StateHasChanged();
        }

        internal async Task<List<TItem>> GetItems(int startIndex, int count)
        {
            if (startIndex < 0)
                startIndex = 0;

            if (Items != null)
            {
                _totalNumItems = Items.Count();

                if (startIndex + count > _totalNumItems)
                    count = _totalNumItems - startIndex;

                return Items.ToList().GetRange(startIndex, count).Select((item, index) =>
                { item.Index = startIndex + index; return item; }).ToList();
            }
            else if (DataProvider != null)
            {
                _loadItemsCts = new CancellationTokenSource();

                try
                {
                    Console.WriteLine($"GetItems: StartIndex:{startIndex} Count:{count}");
                    var result = await DataProvider(new DataProviderRequest(startIndex, count, _loadItemsCts.Token));
                    _totalNumItems = result.TotalNumItems;
                    return result.Items.Select((item, index) =>
                    { item.Index = startIndex + index; return item; }).ToList();
                }
                catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
                {
                    Console.WriteLine("Cancelled");
                    _loadItemsCts = null;
                }
            }
            return new List<TItem>();
        }

    }
}
