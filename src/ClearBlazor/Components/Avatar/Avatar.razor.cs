using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace ClearBlazor
{
    public partial class Avatar : ClearComponentBase
    {
        [Parameter]
        public string Icon { get; set; } = string.Empty;

        [Parameter]
        public string Image { get; set; } = string.Empty;

        [Parameter]
        public string Alt { get; set; } = string.Empty;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public TextEditFillMode AvatarStyle { get; set; } = TextEditFillMode.Filled;

        [Parameter]
        public ContainerShape Shape { get; set; } = ContainerShape.Circle;

        [Parameter]
        public Color Color { get; set; } = ThemeManager.CurrentPalette.Primary;

        [Parameter]
        public string Text { get; set; } = string.Empty;

        [Parameter]
        public Color? IconColor { get; set; } = null;

        public Color TextColor { get; set; } = ThemeManager.CurrentPalette.PrimaryContrastText;
        
        public string FontSize { get; set; } = "";
        public string FontFamily { get; set; } = "";
        public int FontWeight { get; set; } = 0;
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        protected override string UpdateStyle(string css)
        {
            FontSize = GetFontSize();
            FontFamily = GetFontFamily();
            FontWeight = GetFontWeight();
            FontStyle = GetFontStyle();

            css += $"display:grid; {GetBorderRadius()} ";
            switch (Size)
            {
                case Size.VerySmall:
                    css += "height:30px; width:30px; ";
                    break;
                case Size.Small:
                    css += "height:34px; width:34px; ";
                    break;
                case Size.Normal:
                    css += "height:38px; width:38px; ";
                    break;
                case Size.Large:
                    css += "height:43px; width:43px; ";
                    break;
                case Size.VeryLarge:
                    css += "height:50px; width:50px; ";
                    break;
                default:
                    css += "height:38px; width:38px; ";
                    break;
            }
            IconColor = GetIconColor();
            switch (AvatarStyle)
            {
                case TextEditFillMode.None:
                    TextColor = Color;
                    css += $"background: {ThemeManager.CurrentPalette.AvatarBackgroundColor.Value}; ";
                    break;
                case TextEditFillMode.Filled:
                    TextColor = Color.GetAssocTextColor(Color);
                    css += $"background: {Color.Value}; ";
                    break;
                case TextEditFillMode.Outline:
                    TextColor = Color;
                    css += $"border-width: 1px; border-style: solid; border-color: {Color.Value}; ";
                    css += $"background: {ThemeManager.CurrentPalette.AvatarBackgroundColor.Value}; ";
                    break;
            }

            return css;
        }

        private Color GetIconColor()
        {
            if (IconColor == null)
            {
                switch (AvatarStyle)
                {
                    case TextEditFillMode.Filled:
                    case TextEditFillMode.None:
                        return ThemeManager.CurrentPalette.Dark;
                    case TextEditFillMode.Outline:
                        return Color;
                }
                return ThemeManager.CurrentPalette.Dark;
            }
            else 
                return IconColor;

        }


        private string GetBorderRadius()
        {
            switch (Shape)
            {
                case ContainerShape.Circle:
                    return "border-radius:50%; ";
                case ContainerShape.Square:
                    return "";
                case ContainerShape.SquareRounded:
                    return "border-radius:4px; ";
            }
            return "";
        }

        private FontStyle GetFontStyle()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.AvatarVerySmall.FontStyle;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.AvatarSmall.FontStyle;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.AvatarNormal.FontStyle;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.AvatarLarge.FontStyle;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.AvatarVeryLarge.FontStyle;
                default:
                    return ThemeManager.CurrentTheme.Typography.AvatarNormal.FontStyle;
            }
        }

        private int GetFontWeight()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.AvatarVerySmall.FontWeight;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.AvatarSmall.FontWeight;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.AvatarNormal.FontWeight;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.AvatarLarge.FontWeight;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.AvatarVeryLarge.FontWeight;
                default:
                    return ThemeManager.CurrentTheme.Typography.AvatarNormal.FontWeight;
            }
        }

        private string GetFontFamily()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.AvatarVerySmall.FontFamily);
                case Size.Small:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.AvatarSmall.FontFamily);
                case Size.Normal:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.AvatarNormal.FontFamily);
                case Size.Large:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.AvatarLarge.FontFamily);
                case Size.VeryLarge:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.AvatarVeryLarge.FontFamily);
                default:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.AvatarNormal.FontFamily);
            }
        }

        private string GetFontSize()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.AvatarVerySmall.FontSize;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.AvatarSmall.FontSize;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.AvatarNormal.FontSize;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.AvatarLarge.FontSize;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.AvatarVeryLarge.FontSize;
                default:
                    return ThemeManager.CurrentTheme.Typography.AvatarNormal.FontSize;
            }
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
    }
}