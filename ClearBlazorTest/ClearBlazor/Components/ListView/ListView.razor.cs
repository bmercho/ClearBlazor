using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ListView<TItem> : ClearComponentBase, IContent, IBackground, IBorder, IBoxShadow
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public List<TItem?> SelectedItems { get; set; } = new();

        [Parameter]
        public EventCallback<List<TItem?>> SelectedItemsChanged { get; set; }

        [Parameter]
        public TItem? SelectedItem { get; set; } = default;

        [Parameter]
        public EventCallback<TItem?> SelectedItemChanged { get; set; }

        [Parameter]
        public RenderFragment<ItemInfo<TItem>> RowTemplate { get; set; } = null!;

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.None;

        [Parameter]
        public bool AllowSelectionToggle { get; set; } = false;

        [Parameter]
        public List<TItem>? ListViewData { get; set; } = null;

        [Parameter]
        public bool Virtualize { get; set; } = false;

        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Stretch;

        [Parameter]
        public int? ItemHeight { get; set; }



        [Parameter]
        public Color? BackgroundColour { get; set; } = Color.Transparent;

        [Parameter]
        public string? BorderThickness { get; set; } = "0";

        [Parameter]
        public Color? BorderColour { get; set; } = Color.Transparent;

        [Parameter]
        public string? CornerRadius { get; set; } = "0";

        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        [Parameter]
        public EventCallback<TItem> OnSelectionChanged { get; set; }
        [Parameter]
        public EventCallback<List<TItem?>> OnSelectionsChanged { get; set; }

        private TItem? _highlightedItem = default;
        private bool _mouseOver = false;

        private TItem? SelectedParentItem { get; set; } = default;

        private ScrollViewer? ScrollViewer;

        private Virtualize<TItem>? VirtualizeElement;

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

            if (IsHighlighted(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColour.Value}; ";

            if (IsSelected(item))
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColour.Value}; ";

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
                case SelectionMode.Multi:
                    foreach(TItem? item1 in SelectedItems)
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
        private async Task ItemClicked(TItem item)
        {
            switch (SelectionMode)
            {
                case SelectionMode.None:
                    return;
                case SelectionMode.Single:
                    if (SelectedItem != null && SelectedItem.Equals(item))
                    {
                        if (AllowSelectionToggle)
                        {
                            SelectedItem = default;
                            await SelectedItemChanged.InvokeAsync(default);
                            await OnSelectionChanged.InvokeAsync(default);
                        }
                    }
                    else
                    {
                        SelectedItem = item;
                        await SelectedItemChanged.InvokeAsync(item);
                        await OnSelectionChanged.InvokeAsync(item);
                    }
                    break;
                case SelectionMode.Multi:
                    if (IsSelected(item))
                        SelectedItems.Remove(item);
                    else
                        SelectedItems.Add(item);
                    await SelectedItemsChanged.InvokeAsync(SelectedItems);
                    await OnSelectionsChanged.InvokeAsync(SelectedItems);
                    break;
            }
            StateHasChanged();
        }
    }
}