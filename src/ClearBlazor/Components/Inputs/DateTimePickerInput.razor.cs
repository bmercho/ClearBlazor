using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A date picker input component
    /// </summary>
    public partial class DateTimePickerInput : ContainerInputBase<DateTime?>, IBackground
    {
        /// <summary>
        /// The default date/time. This is used when the Value parameter is null. If this is also null, 
        /// the default date/time will be DateTime.Now. 
        /// </summary>
        [Parameter]
        public DateTime? DefaultDateTime { get; set; } = null;

        /// <summary>
        /// Specifies the format for the date and time. The default format is 'dd MMM yyyy HH:mm'.
        /// </summary>
        [Parameter]
        public string DateTimeFormat { get; set; } = "dd MMM yyyy HH:mm";

        /// <summary>
        /// Gets or sets a value indicating whether seconds are displayed in the time representation.
        /// </summary>
        [Parameter]
        public bool ShowSeconds { get; set; } = false;

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

        private string? DateTimeString => Value == null ? string.Empty : ((DateTime)Value).ToString(DateTimeFormat);

        private bool PopupOpen = false;


        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Value == null)
                if (DefaultDateTime == null)
                    Value = DateTime.Now;
                else
                    Value = DefaultDateTime;
        }

        private bool IsMouseNotOver()
        {
            return !MouseOver;
        }

        private async Task TogglePopup()
        {
            PopupOpen = !PopupOpen;
            await DateTimeChanged();
        }

        protected override async Task ClearEntry()
        {
            Value = null;
            await DateTimeChanged();
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        private async Task DateTimeSelected()
        {
            PopupOpen = false;
            await DateTimeChanged();
        }
        private async Task DateTimeChanged()
        {
            await ValueChanged.InvokeAsync(Value);
            await Refresh();
        }
    }
}