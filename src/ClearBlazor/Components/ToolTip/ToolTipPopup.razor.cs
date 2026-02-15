using Microsoft.AspNetCore.Components;

namespace ClearBlazor.Internal
{
    /// <summary>
    /// A control that provides additional context for a UI element. 
    /// </summary>
    public partial class ToolTipPopup : ClearComponentBase
    {
        public static string? Text { get; set; } = null;

        public static Size Size { get; set; } = Size.Normal;

        public static ToolTipPosition? ToolTipPosition { get; set; } = null;

        private PopupPosition GetPopupPosition()
        {
            switch (ToolTipPosition)
            {
                case ClearBlazor.ToolTipPosition.Bottom:
                    return PopupPosition.BottomCentre;
                case ClearBlazor.ToolTipPosition.Top:
                    return PopupPosition.TopCentre;
                case ClearBlazor.ToolTipPosition.Left:
                    return PopupPosition.CentreLeft;
                case ClearBlazor.ToolTipPosition.Right:
                    return PopupPosition.CentreRight;
            }
            return PopupPosition.TopCentre;
        }

        private PopupTransform GetPopupTransform()
        {
            switch(ToolTipPosition)
            {
                case ClearBlazor.ToolTipPosition.Bottom:
                    return PopupTransform.TopCentre;
                case ClearBlazor.ToolTipPosition.Top:
                    return PopupTransform.BottomCentre;
                case ClearBlazor.ToolTipPosition.Left:
                    return PopupTransform.CentreRight;
                case ClearBlazor.ToolTipPosition.Right:
                    return PopupTransform.CentreLeft;
            }
            return PopupTransform.BottomCentre;

        }
    }
}