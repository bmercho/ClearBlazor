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

        private bool PopupOpen = false;
        private bool _modified = false;

        private async Task TogglePopup()
        {
            PopupOpen = !PopupOpen;
            if (!PopupOpen)
            {
                if (_modified)
                {
                    _modified = false;
                    await TimeSelected.InvokeAsync();
                }
            }  
            await Refresh();
        }

        protected override async Task ClearEntry()
        {
            Value = null;
            await TimeChanged();
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        private async Task TimeSelection()
        {
            PopupOpen = false;
            if (_modified)
            {
                _modified = false;
                await TimeSelected.InvokeAsync();
            }
            await Refresh();
        }

        private async Task TimeChanged()
        {
            _modified = true;
            await ValueChanged.InvokeAsync(Value);
            await Refresh();
        }

        private async Task PopupClosed()
        {
            PopupOpen = false;
            if (_modified)
            {
                _modified = false;
                await TimeSelected.InvokeAsync();
            }
            await Refresh();
        }

        internal async Task OutsideClick()
        {
            if (!PopupOpen)
                return;

            if (!MouseOver)
            {
                PopupOpen = false;
                if (_modified)
                {
                    _modified = false;
                    await TimeSelected.InvokeAsync();
                }
                await Refresh();
            }
        }
    }
}