using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A date picker input component
    /// </summary>
    public partial class DateTimePickerInputPopup : ClearComponentBase
    {
        [Parameter]
        public ClearComponentBase? AssociatedComponent { get; set; } = null;

        public static DateTime? DateTime { get; set; } = null;

        public static DateTime? DefaultDateTime { get; set; } = null;

        public static string DateTimeFormat { get; set; } = "dd MMM yyyy HH:mm";

        public static bool ShowSeconds { get; set; } = false;

        public static Orientation Orientation { get; set; } = Orientation.Portrait;

        public static PopupPosition Position { get; set; } = PopupPosition.BottomLeft;

        public static PopupTransform Transform { get; set; } = PopupTransform.TopLeft;

        public static bool AllowVerticalFlip { get; set; } = true;

        public static bool AllowHorizontalFlip { get; set; } = true;

        private bool _mouseOver = false;

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

        private async Task DateTimeChanged()
        {
            var associatedComponent = AssociatedComponent as DateTimePickerInput;
            if (associatedComponent == null)
                return;
            await associatedComponent.DateTimeChanged(DateTime);
        }
    }
}