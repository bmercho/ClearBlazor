using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control that provides additional context for a UI element. 
    /// </summary>
    public partial class ToolTip : ClearComponentBase
    {
        /// <summary>
        /// Text shown in tooltip
        /// </summary>
        [Parameter]
        public string? Text { get; set; } = null;

        /// <summary>
        /// Size of tooltip
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// Position of tooltip
        /// </summary>
        [Parameter]
        public ToolTipPosition? ToolTipPosition { get; set; } = null;

        /// <summary>
        /// The delay in milliseconds before the tooltip is shown
        /// </summary>
        [Parameter]
        public int? Delay { get; set; } = null; 

        private bool Open = true;

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

        /// <summary>
        /// Shows the tooltip
        /// </summary>
        public void ShowToolTip()
        {
            Open = true;
            StateHasChanged();
        }

        /// <summary>
        /// Hides the tooltip
        /// </summary>
        public void HideToolTip()
        {
            Open = false;
            StateHasChanged();
        }
    }
}