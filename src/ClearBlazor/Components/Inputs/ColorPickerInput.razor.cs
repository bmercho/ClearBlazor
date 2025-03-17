using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A color picker input component
    /// </summary>
    public partial class ColorPickerInput : ContainerInputBase<Color>, IBackground
    {
        [Parameter]
        public bool ShowHex { get; set; } = false;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
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