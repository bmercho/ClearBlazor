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
        /// Specifies the format of the time. The default format is 'hh:mm tt'.
        /// </summary>
        [Parameter]
        public string TimeFormat { get; set; } = "hh:mm tt";

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
        private SizeInfo? SizeInfo = null;
        private ElementReference PickerElement;
        private TimePicker? TimePicker = null;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            SizeInfo? existing = null;
            if (SizeInfo != null)
                existing = SizeInfo;
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", PickerElement);
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

        protected override string GetInputType()
        {
            return string.Empty;
        }

        private async Task OpenChanged()
        {
            if (TimePicker != null && PopupOpen == false)
            {
                if (Hours24)
                    TimePicker.SetMode(PickerMode.Hour24);
                else
                    TimePicker.SetMode(PickerMode.Hour12);
                await ValueChanged.InvokeAsync(Value);
            }
        }

        private async Task MinuteSelected()
        {
            PopupOpen = false;
            StateHasChanged();
            await ValueChanged.InvokeAsync(Value);
            if (TimePicker != null)
                if (Hours24)
                    TimePicker.SetMode(PickerMode.Hour24);
                else
                    TimePicker.SetMode(PickerMode.Hour12);
        }

        private async Task TimeChanged()
        {
            await ValueChanged.InvokeAsync(Value);
            StateHasChanged();
        }
    }
}