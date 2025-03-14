using Microsoft.AspNetCore.Components;

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

        protected override string GetBorder(ButtonStyle buttonStyle, Color color)
        {
            switch (buttonStyle)
            {
                case ClearBlazor.ButtonStyle.Filled:
                case ClearBlazor.ButtonStyle.LabelOnly:
                    if (IsActive)
                        if (IsFirstTab)
                            return $"border-width: 0 0 2px 0; border-style: solid; border-color: {color.Value};  border-radius: 4px 0 0 0; ";
                        else
                            return $"border-width: 0 0 2px 0; border-style: solid; border-color: {color.Value};  border-radius: 0; ";
                    else
                        return $"border-radius: 0; ";
                case ClearBlazor.ButtonStyle.Outlined:
                    return $"border-width: 1px; border-style: solid; border-color: {color.Value}; border-radius: 0; ";
            }
            return string.Empty;
        }

        protected override string GetPadding(Size size)
        {
            switch (size)
            {
                case Size.VerySmall:
                    return $"padding: 0 24px; ";
                case Size.Small:       
                    return $"padding: 0 30px; ";
                case Size.Normal:      
                    return $"padding: 0 36px; ";
                case Size.Large:       
                    return $"padding: 0 42px; ";
                case Size.VeryLarge:   
                    return $"padding: 0 48px; ";
                default:               
                    return $"padding: 0 36px; ";
            }
        }

    }
}
