using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class MaterialIcon : ClearComponentBase
    {
        [Parameter]
        public string Icon { get; set; } = string.Empty;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public double Rotation { get; set; } = 0.0;

        [Parameter]
        public Color Color { get; set; } = ThemeManager.CurrentPalette.Dark;

        [Parameter]
        public string ViewBox { get; set; } = "0 0 24 24";

        [Parameter]
        public string? ToolTip { get; set; } = null;

        internal ToolTip? ToolTipElement { get; set; } = null;

        [Parameter]
        public ToolTipPosition? ToolTipPosition { get; set; } = null;


        [Parameter]
        public int? ToolTipDelay { get; set; } = null; // Milliseconds

        protected string GetIconStyle()
        {
            if (Color == null)
                return $"font-size:{GetIconSize()}rem; height:1em; vertical-align: middle;";
            else
                return $"fill:{Color.Value}; font-size:{GetIconSize()}rem; height:1em; vertical-align: middle;";
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
                await ToolTipElement.ShowToolTip();
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