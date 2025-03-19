using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a spinner component.
    /// </summary>
    public partial class Spinner:ClearComponentBase
    {
        /// <summary>
        /// Color of spinner.
        /// </summary>
        [Parameter]
        public Color? Color { get; set; }

        /// <summary>
        /// Size of spinner.
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        protected override string UpdateStyle(string css)
        {
            int borderSize = 0;
            int size = 0;
            switch (Size)
            {
                case Size.VerySmall:
                    borderSize = 4;
                    size = 10;
                    break;
                case Size.Small:
                    borderSize = 6;
                    size = 15;
                    break;
                case Size.Normal:
                    borderSize = 8;
                    size = 20;
                    break;
                case Size.Large:
                    borderSize = 10;
                    size = 25;
                    break;
                case Size.VeryLarge:
                    borderSize = 12;
                    size = 30;
                    break;
            }
            css += $"border: {borderSize}px solid {GetBackground().Value}; border-top: {borderSize}px solid {GetColor().Value}; " +
                   $"border-radius: 50%; width: {size}px; height: {size}px; " +
                   $"animation: spin 700ms linear infinite; top: 40 %;";

            return css;
        }

        private Color GetColor()
        {
            if (Color == null)
                return Color.Primary;
            else
                return Color;
        }
        private Color GetBackground()
        {
            return Color.ContrastingColor(GetColor());    
        }
    }
}