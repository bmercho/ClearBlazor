using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    /// <summary>
    /// ListView is a templated list component supporting virtualization and allowing multiple selections.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class ListView<TItem> : ClearComponentBase, IBackground, IBorder, IBoxShadow
    {
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
        /// Event that is raised when the SelectedItem is changed.(when in single select mode)
        /// </summary>
        [Parameter]
        public EventCallback<TItem> OnSelectionChanged { get; set; }

        /// <summary>
        /// Event that is raised when the SelectedItems is changed.(when in multi select mode)
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem?>> OnSelectionsChanged { get; set; }

        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public RenderFragment<ItemInfo<TItem>> RowTemplate { get; set; } = null!;

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
        /// The horizontal content alignment within the control.
        /// </summary>
        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Stretch;

        /// <summary>
        /// The height of each item.
        /// If not provided uses the height of the first item.
        /// Ignored if VariableItemHeight is true.
        /// </summary>
        [Parameter]
        public int? ItemHeight { get; set; }

        /// <summary>
        /// If true it ignores ItemHeight and internally uses the InfiniteScroller component
        /// </summary>
        [Parameter]
        public VirtualizeMode VirtualizeMode { get; set; } = VirtualizeMode.None;

        /// <summary>
        /// See <a href=IBackgroundApi>IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = Color.Transparent;

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

        private TItem? _highlightedItem = default;
        private bool _mouseOver = false;
        private IList<TItem>? UnderlyingList;
        private SelectionHelper<TItem> _selectionHelper = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (HorizontalAlignment == null)
                HorizontalAlignment = Alignment.Stretch;
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; ";
            return css;
        }

        protected string GetContentStyle(TItem item)
        {
            var css = "display:grid;";
            if (ItemHeight != null)
                css += $"height: {ItemHeight}px;";
            switch (HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += $"justify-self:stretch; ";
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

            if (HoverHighlight && IsHighlighted(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            if (IsSelected(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; ";

            return css;
        }

        private ItemInfo<TItem> GetItemInfo(TItem item)
        {
            return new ItemInfo<TItem> { Item = item, IsHighlighted= IsHighlighted(item), IsSelected=IsSelected(item) };
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
                        await OnSelectionChanged.InvokeAsync(SelectedItem);
                    }
                    break;
                case SelectionMode.SimpleMulti:
                    if (_selectionHelper.HandleSimpleMultiSelect(item, SelectedItems))
                    {
                        await OnSelectionsChanged.InvokeAsync(SelectedItems);
                    }
                    break;
                case SelectionMode.Multi:
                    if (await _selectionHelper.HandleMultiSelect(item, index, SelectedItems, 
                                                                 UnderlyingList, ctrlDown, shiftDown))
                    {
                        await OnSelectionsChanged.InvokeAsync(SelectedItems);
                    }
                    break;
            }
            StateHasChanged();
        }
    }
}