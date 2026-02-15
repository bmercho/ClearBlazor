using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// TimePickerInput is a component for selecting time with customizable formats, 24-hour or 12-hour modes, and
    /// various display options.
    /// </summary>
    public partial class TimePickerInputPopup : ClearComponentBase
    {
        [Parameter]
        public ClearComponentBase? AssociatedComponent { get; set; } = null;

        public static TimeOnly? TimeOnly { get; set; } = null;

        public static TimeOnly? DefaultTime { get; set; } = null;

        public static string TimeFormat { get; set; } = "hh:mm tt";

        public static bool ShowSeconds { get; set; } = false;

        public static bool Hours24 { get; set; } = false;

        public static MinuteStep MinuteStep { get; set; } = MinuteStep.One;

        public static Orientation Orientation { get; set; } = Orientation.Portrait;

        public static PopupPosition Position { get; set; } = PopupPosition.BottomLeft;

        public static PopupTransform Transform { get; set; } = PopupTransform.TopLeft;

        public static bool AllowVerticalFlip { get; set; } = true;

        public static bool AllowHorizontalFlip { get; set; } = true;

        private bool _mouseOver = false;
        private TimePicker TimePicker = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (Hours24)
                    SetPickerMode(PickerMode.Hour24);
                else
                    SetPickerMode(PickerMode.Hour12);
            }
        }

        private bool IsMouseNotOver()
        {
            return !_mouseOver;
        }

        private void OnMouseEnter()
        {
            _mouseOver = true;
        }
        private void OnMouseLeave()
        {
            _mouseOver = false;
        }

        internal void SetPickerMode(PickerMode pickerMode)
        {
            TimePicker.SetMode(pickerMode);
        }

        private async Task TimeSelection()
        {
            var associatedComponent = AssociatedComponent as TimePickerInput;
            if (associatedComponent == null)
                return;
            await associatedComponent.TimeSelection();

        }

        private async Task TimeChanged()
        {
            var associatedComponent = AssociatedComponent as TimePickerInput;
            if (associatedComponent == null)
                return;
            await associatedComponent.TimeChanged(TimeOnly);
        }
    }
}