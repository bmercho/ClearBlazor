using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class Select<TItem> : ContainerInputBase<TItem>, IContent
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;
        [Parameter]
        public List<ListDataItem<TItem>>? SelectData { get; set; } = null;
        [Parameter]
        public List<TItem?>? Values { get; set; } = null;
        [Parameter]
        public EventCallback<List<TItem?>> ValuesChanged { get; set; }
        [Parameter]
        public bool MultiSelect { get; set; } = false;
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopLeft;
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;
        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;


        private SizeInfo? SizeInfo = null;
        private ElementReference SelectElement;
        private string SelectWidth = "min-width: 20ch; ";
        private string? SelectionText = string.Empty;
        private double MaximumWidth = 0;
        private bool DropdownOpen = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (VerticalAlignment == null)
                VerticalAlignment = Alignment.Start;
        }

        internal void HandleChild(SelectItem<TItem> item)
        {

            SelectWidth = GetSelectWidth(item);
            StateHasChanged();
        }
        private string GetSelectWidth(SelectItem<TItem>? item)
        {
            if (HorizontalAlignment == Alignment.Stretch)
            {
                return string.Empty;
            }
            if (item != null && !string.IsNullOrEmpty(item.Text))
            {
                var width = item.Text.Length + 7;
                if (item.Icon != null || item.Avatar != null)
                    width += 5;
                if (MultiSelect)
                    width += 5;
                if (width > MaximumWidth)
                    MaximumWidth = width;
            }

            switch (Size)
            {
                case Size.VerySmall:
                    return $"min-width: {MaximumWidth * 1.0}ch; ";
                case Size.Small:
                    return $"min-width: {MaximumWidth * 1.1}ch; ";
                case Size.Normal:
                    return $"min-width: {MaximumWidth * 1.2}ch; ";
                case Size.Large:
                    return $"min-width: {MaximumWidth * 1.4}ch; ";
                case Size.VeryLarge:
                    return $"min-width: {MaximumWidth * 1.6}ch; ";
            }
            return $"min-width: {MaximumWidth * 1.2}ch; ";
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Value != null)
                SelectionText = Value.ToString();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
                SizeInfo? existing = null;
                if (SizeInfo != null)
                    existing = SizeInfo;
                SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", SelectElement);
                if (existing == null ||
                    !existing.Equals(SizeInfo))
                    StateHasChanged();
        }

        private async Task OnListSelectionChanged(ListDataItem<TItem> item)
        {
            if (MultiSelect)
                return;
            DropdownOpen = false;
            if (item != null)
                SelectionText = item.Name;
            await ValueChanged.InvokeAsync(Value);
            await ValidateField();
            StateHasChanged();
        }

        private async Task OnListSelectionsChanged(List<ListDataItem<TItem>> selectedItems)
        {
            if (!MultiSelect)
                return;
            SelectionText = string.Join(", ", selectedItems.Select(s => s.Name));

            if (typeof(TItem).IsEnum)
                await ValueChanged.InvokeAsync(Value);
            else
                await ValuesChanged.InvokeAsync(Values);
            await ValidateField();
            StateHasChanged();
        }

        private int GetPopupWidth()
        {
            if (SizeInfo == null)
                return 200;
            else
                return (int)SizeInfo.ParentWidth + 40;
        }

        private bool IsMouseNotOver()
        {
            return !MouseOver;
        }

        private void ToggleDropdown()
        {
            DropdownOpen = !DropdownOpen;
            StateHasChanged();
        }

        public override async Task<bool> ValidateField()
        {
            IsValid = true;
            ValidationErrorMessages.Clear();
            await Task.CompletedTask;
            return IsValid;
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        protected override async Task ClearEntry()
        {
            await Task.CompletedTask;
        }
    }
}