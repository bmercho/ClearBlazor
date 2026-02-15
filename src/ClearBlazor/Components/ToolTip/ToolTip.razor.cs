using ClearBlazor.Internal;
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
        /// Shows the tooltip
        /// </summary>
        public async Task ShowToolTip()
        {
            if (Parent == null)
                return;
            ToolTipPopup.Text = Text;
            ToolTipPopup.Size = Size;
            ToolTipPopup.ToolTipPosition = ToolTipPosition;
            await ShowPopup(typeof(ToolTipPopup), Parent);
        }

        /// <summary>
        /// Hides the tooltip
        /// </summary>
        public async Task HideToolTip()
        {
            await HidePopup();
        }
    }
}