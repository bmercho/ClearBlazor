using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class TabButton:Button
    {

        [Parameter]
        public bool IsActive { get; set; } = false;

        [Parameter]
        public bool IsFirstTab { get; set; } = false;

        [Parameter]
        public bool IsLastTab { get; set; } = false;

        protected override string GetBorder(TextEditFillMode buttonStyle, Color colour)
        {
            switch (buttonStyle)
            {
                case ClearBlazor.TextEditFillMode.Filled:
                case ClearBlazor.TextEditFillMode.None:
                    if (IsActive)
                        if (IsFirstTab)
                            return $"border-width: 0 0 2px 0; border-style: solid; border-color: {colour.Value};  border-radius: 4px 0 0 0; ";
                        else
                            return $"border-width: 0 0 2px 0; border-style: solid; border-color: {colour.Value};  border-radius: 0; ";
                    else
                        return $"border-radius: 0; ";
                case ClearBlazor.TextEditFillMode.Outline:
                    return $"border-width: 1px; border-style: solid; border-color: {colour.Value}; border-radius: 0; ";
            }
            return string.Empty;
        }

        protected override string GetPadding(Size size)
        {
            switch (size)
            {
                case Size.VerySmall:
                    return $"padding: 24px 0; ";
                case Size.Small:
                    return $"padding: 30px 0; ";
                case Size.Normal:
                    return $"padding: 36px 0; ";
                case Size.Large:
                    return $"padding: 42px 0; ";
                case Size.VeryLarge:
                    return $"padding: 48px 0; ";
                default:
                    return $"padding: 36px 0; ";
            }
        }

    }
}
