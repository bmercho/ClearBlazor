using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A date picker input component
    /// </summary>
    public partial class DatePickerInput : ContainerInputBase<DateOnly?>, IBackground
    {
        /// <summary>
        /// The default date. This is used when the Value parameter is null. If this is also null, the default date will be DateTime.Now. 
        /// </summary>
        [Parameter]
        public DateOnly? DefaultDate { get; set; }

        /// <summary>
        /// Specifies the format for the date. The default format is 'dd MMM yyyy'.
        /// </summary>
        [Parameter]
        public string DateFormat { get; set; } = "dd MMM yyyy";


        /// <summary>
        /// Event raised when the date selection is complete.
        /// </summary>
        [Parameter]
        public EventCallback DateSelected { get; set; }

        /// <summary>
        /// Orientation of the component. Defaults to portrait.
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

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

        private string? DateString => Value == null ? string.Empty : ((DateOnly)Value).ToString(DateFormat);

        private bool PopupOpen = false;

        private bool IsMouseNotOver()
        {
            return !MouseOver;
        }

        private async Task TogglePopup()
        {
            PopupOpen = !PopupOpen;
            await DateChanged();
        }

        protected override async Task ClearEntry()
        {
            Value = null;
            await DateChanged();
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        private async Task DateSelection()
        {
            PopupOpen = false;
            await DateSelected.InvokeAsync();
        }
        private async Task DateChanged()
        {
            await ValueChanged.InvokeAsync(Value);
            DoRender = true;
            StateHasChanged();
        }
    }
}