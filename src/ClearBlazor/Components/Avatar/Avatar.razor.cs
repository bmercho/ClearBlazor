using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a component which displays circular user profile pictures, icons or text.
    /// </summary>
    public partial class Avatar : ClearComponentBase
    {
        /// <summary>
        /// The icon to be shown within the avatar
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// The image to be shown within the avatar
        /// </summary>
        [Parameter]
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// The size of the avatar
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// The style of the avatar
        /// </summary>
        [Parameter]
        public AvatarStyle AvatarStyle { get; set; } = AvatarStyle.Filled;

        /// <summary>
        /// The shape of the avatar
        /// </summary>
        [Parameter]
        public ContainerShape Shape { get; set; } = ContainerShape.Circle;

        /// <summary>
        /// The color used for the button. What gets this color (background, text or outline)
        /// depends on the button style.
        /// </summary>
        [Parameter]
        public Color Color { get; set; } = Color.Primary;

        /// <summary>
        /// The text to be shown within the avatar
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// The color of the icon within the avatar
        /// </summary>
        [Parameter]
        public Color? IconColor { get; set; } = null;

        private Color TextColor { get; set; } = Color.Primary;

        private string FontSize { get; set; } = "";
        private string FontFamily { get; set; } = "";
        private int FontWeight { get; set; } = 0;
        private FontStyle FontStyle { get; set; } = FontStyle.Normal;
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
                case AvatarStyle.LabelOnly:
                    TextColor = Color;
                    css += $"background: {Color.SurfaceContainerHigh.Value}; ";
                    break;
                case AvatarStyle.Filled:
                    TextColor = Color.GetAssocTextColor(Color);
                    css += $"background: {Color.Value}; ";
                    break;
                case AvatarStyle.Outlined:
                    TextColor = Color;
                    css += $"border-width: 1px; border-style: solid; border-color: {Color.Value}; ";
                    css += $"background: {Color.SurfaceContainerHigh.Value}; ";
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
                    case AvatarStyle.Filled:
                    case AvatarStyle.LabelOnly:
                        return Color.ContrastingColor(Color);
                    case AvatarStyle.Outlined:
                        return Color;
                }
                return Color.ContrastingColor(Color);
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
                case ContainerShape.Rounded:
                    return "border-radius:4px; ";
                case ContainerShape.FullyRounded:
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