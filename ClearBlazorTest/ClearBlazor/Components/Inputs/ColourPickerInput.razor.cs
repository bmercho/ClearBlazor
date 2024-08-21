using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class ColourPickerInput : ContainerInputBase<Color>, IBackground
    {
        [Parameter]
        public bool ShowHex { get; set; } = false;
        [Parameter]
        public Color? BackgroundColour { get; set; } = null;
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopLeft;
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;
        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;

        private bool PopupOpen = false;
        private SizeInfo? SizeInfo = null;
        private ElementReference PickerElement;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            SizeInfo? existing = null;
            if (SizeInfo != null)
                existing = SizeInfo;
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", PickerElement);
            //if (existing == null ||
            //    !existing.Equals(SizeInfo))
            //    StateHasChanged();
        }

        private string GetSize()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return "width:100px;height:100px; ";
                case Size.Small:
                    return "width:100px;height:100px; ";
                case Size.Normal:
                    return "width:350px;height:350px; ";
                case Size.Large:
                    return "width:100px;height:100px; ";
                case Size.VeryLarge:
                    return "width:100px;height:100px; ";
            }
            return "width:350px;height:350px; ";
        }

        private bool IsMouseNotOver()
        {
            return !MouseOver;
        }

        private void TogglePopup()
        {
            PopupOpen = !PopupOpen;
            StateHasChanged();
        }

        private int GetPopupWidth()
        {
            if (SizeInfo == null)
                return 200;
            else
                return (int)SizeInfo.ParentWidth + 40;
        }

        protected override async Task ClearEntry()
        {
            await Task.CompletedTask;
        }
        protected override string ComputeInputStyle()
        {
            string css = base.ComputeInputStyle() + $"background: {Value?.Value}";
            return css;
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }
        private async Task OnColourChanged()
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }
}