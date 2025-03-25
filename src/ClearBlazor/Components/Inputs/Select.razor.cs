using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a selectable input control that can display a list of items and allows for single or multiple
    /// selections.
    /// </summary>
    /// <typeparam name="TItem">Represents the type of items that can be selected within the control.</typeparam>
    public partial class Select<TItem> : ContainerInputBase<TItem>
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// Holds a list of selectable data items of type TItem. It can be null and is used for data binding in
        /// components.
        /// </summary>
        [Parameter]
        public List<ListDataItem<TItem>>? SelectData { get; set; } = null;

        /// <summary>
        /// A list of nullable items of type TItem. It can be set to null and is used to hold multiple values.
        /// </summary>
        [Parameter]
        public List<TItem?>? Values { get; set; } = null;

        /// <summary>
        /// An event callback that triggers when the list of values changes. It allows for handling updates to the list
        /// of items.
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem?>> ValuesChanged { get; set; }

        /// <summary>
        /// Indicates whether multiple selections are allowed. Defaults to false.
        /// </summary>
        [Parameter]
        public bool MultiSelect { get; set; } = false;

        /// <summary>
        /// Defines the position of a popup, defaulting to the bottom left corner. It uses the PopupPosition enumeration
        /// for various placement options.
        /// </summary>
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopLeft;

        /// <summary>
        /// Indicates whether vertical flipping is permitted. Defaults to true.
        /// </summary>
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;

        /// <summary>
        /// Indicates whether horizontal flipping is permitted. Defaults to true.
        /// </summary>
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

        private Size GetIconSize()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return Size.VerySmall;
                case Size.Small:
                    return Size.Small;
                case Size.Normal:
                    return Size.Small;
                case Size.Large:
                    return Size.Normal;
                case Size.VeryLarge:
                    return Size.Large;
            }
            return Size.Small;
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

        internal override async Task<bool> ValidateField()
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