using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control that contains a list of ListBoxItems. 
    /// A ListBoxItem can consist of text, icon or am avatar.
    /// The list can be hierarchical.
    /// If custom UI is required for list items use the ListView control instead.
    /// </summary>
    /// <typeparam name="TListBox"></typeparam>
    public partial class ListBox<TListBox> : InputBase, IBackground, IBorder, IBoxShadow
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// The selected value of the list box.
        /// </summary>
        [Parameter]
        public TListBox? Value { get; set; }

        /// <summary>
        /// Event that is raised when the Value changes
        /// </summary>
        [Parameter]
        public EventCallback<TListBox?> ValueChanged { get; set; }

        /// <summary>
        /// The selected values of the list box. (when MultiSelect is true)
        /// </summary>
        [Parameter]
        public List<TListBox?>? Values { get; set; } = null;

        /// <summary>
        /// Event that is raised when Values changes
        /// </summary>
        [Parameter]
        public EventCallback<List<TListBox?>> ValuesChanged { get; set; }

        /// <summary>
        /// Provides the data for list. If not null this is used instead of the ChildContent 
        /// </summary>
        [Parameter]
        public List<ListDataItem<TListBox>>? ListData { get; set; } = null;

        /// <summary>
        /// Used when ListData is not null to horizontally align the content of items
        /// </summary>
        [Parameter]
        public Alignment ContentAlignment { get; set; } = Alignment.Stretch;

        /// <summary>
        /// The spacing between item
        /// </summary>
        [Parameter]
        public double Spacing { get; set; } = 0;

        /// <summary>
        /// The row size for each item
        /// </summary>
        [Parameter]
        public Size RowSize { get; set; } = Size.Normal;

        /// <summary>
        /// Indicates if row items are clickable
        /// </summary>
        [Parameter]
        public bool Clickable { get; set; } = true;

        /// <summary>
        /// Indicates if 
        /// </summary>
        [Parameter]
        public bool MultiSelect { get; set; } = false;

        /// <summary>
        /// Allows a single selection to be toggled off as well as on 
        /// </summary>
        [Parameter]
        public bool AllowSelectionToggle { get; set; } = false;

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

        /// <summary>
        /// Event raised when the selection is changed
        /// </summary>
        [Parameter]
        public EventCallback<ListDataItem<TListBox>> OnSelectionChanged { get; set; }

        /// <summary>
        /// Event raised when selections ar changed
        /// </summary>
        [Parameter]
        public EventCallback<List<ListDataItem<TListBox>>> OnSelectionsChanged { get; set; }

        private List<ListBoxItem<TListBox>> SelectedItems = new();
        private ListBoxItem<TListBox>? SelectedItem = null;

        private ListBoxItem<TListBox>? SelectedParentItem { get; set; } = null;

        bool? _backgroundIsNull = null;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (HorizontalAlignment == null)
                HorizontalAlignment = Alignment.Stretch;

            if (_backgroundIsNull == null)
                _backgroundIsNull = BackgroundColor == null;

            if (_backgroundIsNull == true)
                BackgroundColor = ThemeManager.CurrentColorScheme.SurfaceContainerLow;
        }

        internal async Task HandleChild(ListBoxItem<TListBox> item)
        {
            item.HorizontalAlignmentDefaultOverride = Alignment.Start;
            item.RowSize = RowSize;
            item.ColorOverride = Color;
            item.Level = 1;
            if (MultiSelect)
            {
                if (typeof(TListBox).IsEnum)
                {
                    if (Value == null || item.Value == null)
                        return;

                    var enumValue1 = (long)Convert.ChangeType(Value, typeof(long));
                    var enumValue2 = (long)Convert.ChangeType(item.Value, typeof(long));
                    if ((enumValue1 & enumValue2) == enumValue2)
                    {
                        SelectedItems.Add(item);
                        //await OnSelectionsChanged.InvokeAsync(GetDataItems(SelectedItems));
                        item.Select();
                        StateHasChanged();
                    }
                }
                else if (Values != null && item != null && Values.Contains(item.Value!))
                {
                    SelectedItems.Add(item);
                    //await OnSelectionsChanged.InvokeAsync(GetDataItems(SelectedItems));
                    item.Select();
                    StateHasChanged();
                }
            }
            else
            {
                if (Value != null && Value.Equals(item.Value))
                {
                    SelectedItem = item;
                   // await OnSelectionChanged.InvokeAsync(
                   //       new ListDataItem<TListBox>(item.Text!, item.Value!, item.Icon, item.Avatar));
                    item.Select();
                    StateHasChanged();
                }
            }
        }

        internal async Task<bool> SetSelected(ListBoxItem<TListBox> item)
        {
            bool selected = false;

            if (SelectedParentItem != null && SelectedParentItem != item)
                SelectedParentItem.Unselect();

            if (item.HasChildren)
            {
                SelectedParentItem = item;
                return true;
            }

            if (MultiSelect)
            {
                if (SelectedItems.Contains(item))
                {
                    SelectedItems.Remove(item);
                    selected = false;
                }
                else
                {
                    SelectedItems.Add(item);
                    selected = true;
                }
                if (typeof(TListBox).IsEnum)
                {
                    long enumValue = 0;
                    foreach (var s in SelectedItems.Select(i => i.Value))
                        if (s != null)
                            enumValue += (long)Convert.ChangeType(s, typeof(long));
                    Value = (TListBox?)Enum.ToObject(typeof(TListBox), enumValue);
                    await ValueChanged.InvokeAsync(Value);

                    await OnSelectionsChanged.InvokeAsync(GetDataItems(SelectedItems));
                }
                else
                {
                    Values = SelectedItems.Select(s => s!.Value).ToList();
                    await ValuesChanged.InvokeAsync(Values);
                    await OnSelectionsChanged.InvokeAsync(GetDataItems(SelectedItems));
                }
            }
            else
            {
                if (SelectedItem == item && AllowSelectionToggle)// && !item._hasChildren)
                {
                    SelectedItem = null;
                    selected = false;
                    Value = default;
                }
                else
                {
                    if (SelectedItem != null)
                        SelectedItem.Unselect();

                    SelectedItem = item;
                    selected = true;
                    Value = item.Value;
                }
                await ValueChanged.InvokeAsync(item.Value);
                await OnSelectionChanged.InvokeAsync(
                         new ListDataItem<TListBox>(item.Text!, item.Value!, item.Icon, item.Avatar));
            }
            await ValidateField();
            StateHasChanged();
            return selected;
        }

        private List<ListDataItem<TListBox>> GetDataItems(List<ListBoxItem<TListBox>> items)
        {
            var dataItems = new List<ListDataItem<TListBox>>();
            foreach (var selectedItem in SelectedItems)
                dataItems.Add(new ListDataItem<TListBox>(selectedItem.Text!, selectedItem.Value!,
                                                     selectedItem.Icon, selectedItem.Avatar));
            return dataItems;
        }
    }
}