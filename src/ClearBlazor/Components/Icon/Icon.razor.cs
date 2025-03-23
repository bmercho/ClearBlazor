using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    /// <summary>
    /// Used to show Material Icons and Custom Icons
    /// </summary>
    public partial class Icon : ClearComponentBase
    {
        /// <summary>
        /// The name of the icon
        /// </summary>
        [Parameter]
        public string IconName { get; set; } = string.Empty;

        /// <summary>
        /// The size of the icon
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// The rotation angle of the icon, in degrees
        /// </summary>
        [Parameter]
        public double Rotation { get; set; } = 0.0;

        /// <summary>
        /// The color used for the icon.
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        /// <summary>
        /// The containing box for the icon
        /// </summary>
        [Parameter]
        public string ViewBox { get; set; } = "0 0 24 24";

        /// <summary>
        /// Tooltip string for the button
        /// </summary>
        [Parameter]
        public string? ToolTip { get; set; } = null;

        /// <summary>
        /// Tooltip position (when shown)
        /// </summary>
        [Parameter]
        public ToolTipPosition? ToolTipPosition { get; set; } = null;

        /// <summary>
        /// The delay in milliseconds before the tooltip is shown
        /// </summary>
        [Parameter]
        // Milliseconds
        public int? ToolTipDelay { get; set; } = null;

        internal ToolTip? ToolTipElement { get; set; } = null;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Name == "Icon1")
            {

            }
        }

        protected string GetIconStyle()
        {
            if (Color == null)
                return $"fill:{ThemeManager.CurrentColorScheme.OnSurface.Value}; " +
                       $"font-size:{GetIconSize()}rem; height:1em; vertical-align: middle;";
            else
                return $"fill:{Color.Value}; " +
                       $"font-size:{GetIconSize()}rem; height:1em; vertical-align: middle;";
        }

        protected string GetTransform()
        {
            if (Rotation != 0)
                return $"rotate({Rotation}) ";

            return string.Empty;
        }

        protected float GetIconSize()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return 1.0f;
                case Size.Small:
                    return 1.25f;
                case Size.Normal:
                    return 1.5f;
                case Size.Large:
                    return 1.9f;
                case Size.VeryLarge:
                    return 2.25f;
                default:
                    return 1.5f;
            }
        }

        protected async Task OnMouseEnter(MouseEventArgs e)
        {
            if (ToolTipElement == null)
                await Task.CompletedTask;
            else
                ToolTipElement.ShowToolTip();
            StateHasChanged();
        }

        protected async Task OnMouseLeave(MouseEventArgs e)
        {
            ToolTipElement?.HideToolTip();
            await Task.CompletedTask;
            StateHasChanged();
        }
    }
}