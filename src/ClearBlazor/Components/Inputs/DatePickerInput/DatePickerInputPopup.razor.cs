using Microsoft.AspNetCore.Components;

namespace ClearBlazor.Internal
{
    public partial class DatePickerInputPopup : ClearComponentBase
    {
        [Parameter]
        public ClearComponentBase? AssociatedComponent { get; set; } = null;

        public static DateOnly? DateOnly { get; set; } = null;

        public static DateOnly? DefaultDate { get; set; }

        public static Orientation Orientation { get; set; } = Orientation.Portrait;

        public static PopupPosition Position { get; set; } = PopupPosition.BottomLeft;

        public static PopupTransform Transform { get; set; } = PopupTransform.TopLeft;

        public static bool AllowVerticalFlip { get; set; } = true;

        public static bool AllowHorizontalFlip { get; set; } = true;

        private bool _mouseOver = false;

        private async Task DateSelection()
        {
            var associatedComponent = AssociatedComponent as DatePickerInput;
            if (associatedComponent == null)
                return;
            await associatedComponent.DateSelection();
        }

        private async Task DateChanged()
        {
            var associatedComponent = AssociatedComponent as DatePickerInput;
            if (associatedComponent == null)
                return;
            await associatedComponent.DateChanged(DateOnly);
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
    }
}