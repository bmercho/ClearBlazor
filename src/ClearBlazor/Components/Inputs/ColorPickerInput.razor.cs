using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A color picker input component
    /// </summary>
    public partial class ColorPickerInput : ContainerInputBase<Color>, IBackground
    {
        /// <summary>
        /// Indicates whether to display the color in hexadecimal format. Defaults to false.
        /// </summary>
        [Parameter]
        public bool ShowHex { get; set; } = false;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Defines the position of a popup, defaulting to the bottom left corner. 
        /// </summary>
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;

        /// <summary>
        /// Defines the position of a popup relative to its target. The default position is set to the top-left corner.
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