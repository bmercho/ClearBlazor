using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ListBox<TListBox> : InputBase, IBackground, IBorder
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public TListBox? Value { get; set; }

        [Parameter]
        public EventCallback<TListBox?> ValueChanged { get; set; }

        [Parameter]
        public List<TListBox?>? Values { get; set; } = null;

        [Parameter]
        public EventCallback<List<TListBox?>> ValuesChanged { get; set; }

        [Parameter]
        public List<ListDataItem<TListBox>>? ListData { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; }

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
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        [Parameter]
        public Alignment ContentAlignment { get; set; } = Alignment.Stretch;

        [Parameter]
        public double Spacing { get; set; } = 0;

        [Parameter]
        public Size RowSize { get; set; } = Size.Normal;

        [Parameter]
        public bool Clickable { get; set; } = true;

        [Parameter]
        public bool MultiSelect { get; set; } = false;

        [Parameter]
        public bool AllowSelectionToggle { get; set; } = false;

        [Parameter]
        public EventCallback<ListDataItem<TListBox>> OnSelectionChanged { get; set; }
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
                BackgroundColor = AppBarTokens.ContainerColor;
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

        public async Task<bool> SetSelected(ListBoxItem<TListBox> item)
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