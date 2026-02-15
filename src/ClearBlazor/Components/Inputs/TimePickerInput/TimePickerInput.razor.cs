using ClearBlazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// TimePickerInput is a component for selecting time with customizable formats, 24-hour or 12-hour modes, and
    /// various display options.
    /// </summary>
    public partial class TimePickerInput : ContainerInputBase<TimeOnly?>, IBackground
    {
        /// <summary>
        /// The default time. This is used when the Value parameter is null. If this is also null, 
        /// the default time will be 00:00. 
        /// </summary>
        [Parameter]
        public TimeOnly? DefaultTime { get; set; } = null;

        /// <summary>
        /// Specifies the format of the time. The default format is 'hh:mm tt'.
        /// </summary>
        [Parameter]
        public string TimeFormat { get; set; } = "hh:mm tt";

        /// <summary>
        /// Event raised when the time selection is complete.
        /// </summary>
        [Parameter]
        public EventCallback TimeSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether seconds are displayed in the time representation.
        /// </summary>
        [Parameter]
        public bool ShowSeconds { get; set; } = false;

        /// <summary>
        /// Indicates whether the time format is 24-hour. Defaults to false, meaning a 12-hour format is used.
        /// </summary>
        [Parameter]
        public bool Hours24 { get; set; } = false;

        /// <summary>
        /// Defines the step interval for minutes, allowing customization of minute increments. Defaults to a one-minute
        /// step.
        /// </summary>
        [Parameter]
        public MinuteStep MinuteStep { get; set; } = MinuteStep.One;

        /// <summary>
        /// The orientation of the component. Defaults to portrait.    
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

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

        private string? TimeString => Value == null ? string.Empty : ((TimeOnly)Value).ToString(TimeFormat);

        private bool _popupOpen = false;
        private TimePicker? TimePicker = null;
        private Grid Grid = null!;

        private async Task TogglePopup()
        {
            _popupOpen = !_popupOpen;
            if (_popupOpen)
            {
                TimePickerInputPopup.TimeOnly = Value;
                TimePickerInputPopup.DefaultTime = DefaultTime;
                TimePickerInputPopup.TimeFormat = TimeFormat;
                TimePickerInputPopup.ShowSeconds = ShowSeconds;
                TimePickerInputPopup.Hours24 = Hours24;
                TimePickerInputPopup.MinuteStep = MinuteStep;
                TimePickerInputPopup.Orientation = Orientation;
                TimePickerInputPopup.Position = Position;
                TimePickerInputPopup.Transform = Transform;
                TimePickerInputPopup.AllowVerticalFlip = AllowVerticalFlip;
                TimePickerInputPopup.AllowHorizontalFlip = AllowHorizontalFlip;

                await ShowPopup(typeof(TimePickerInputPopup), Grid, this);
            }
            else
                await HidePopup();
        }

        protected override async Task ClearEntry()
        {
            Value = null;
            await TimeChanged(Value);
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        internal async Task TimeSelection()
        {
            _popupOpen = false;
            await HidePopup();
            await TimeSelected.InvokeAsync();
        }

        internal async Task TimeChanged(TimeOnly? newTimeOnly)
        {
            Value = newTimeOnly;
            await ValueChanged.InvokeAsync(Value);
            await Refresh();
        }
    }
}