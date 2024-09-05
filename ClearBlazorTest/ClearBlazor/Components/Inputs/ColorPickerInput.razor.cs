using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ColorPickerInput : ContainerInputBase<Color>, IBackground
    {
        [Parameter]
        public bool ShowHex { get; set; } = false;
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopLeft;
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;
        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;

        private bool PopupOpen = false;
        private ElementReference PickerElement;

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
        private async Task OnColorChanged()
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }
}