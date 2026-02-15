using ClearBlazor.Internal;
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
        /// Event raised when the date/time selection is complete.
        /// </summary>
        [Parameter]
        public EventCallback DateTimeSelected { get; set; }

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

        private Grid Grid = null!;

        private bool _popupOpen = false;

        private async Task TogglePopup()
        {
            _popupOpen = !_popupOpen;
            if (_popupOpen)
            {
                DateTimePickerInputPopup.DateTime = Value;
                DateTimePickerInputPopup.DefaultDateTime = DefaultDateTime;
                DateTimePickerInputPopup.ShowSeconds = ShowSeconds;
                DateTimePickerInputPopup.DateTimeFormat = DateTimeFormat;
                DateTimePickerInputPopup.Orientation = Orientation;
                DateTimePickerInputPopup.Position = Position;
                DateTimePickerInputPopup.Transform = Transform;
                DateTimePickerInputPopup.AllowVerticalFlip = AllowVerticalFlip;
                DateTimePickerInputPopup.AllowHorizontalFlip = AllowHorizontalFlip;

                await ShowPopup(typeof(DateTimePickerInputPopup), Grid, this);
            }
            else
                await HidePopup();
        }

        protected override async Task ClearEntry()
        {
            Value = null;
            await DateTimeChanged(Value);
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        internal async Task DateTimeSelection()
        {
            _popupOpen = false;
            await HidePopup();
            await DateTimeSelected.InvokeAsync();
        }
        internal async Task DateTimeChanged(DateTime? newDateTime)
        {
            Value = newDateTime;
            await ValueChanged.InvokeAsync(Value);
            await Refresh();
        }
    }
}