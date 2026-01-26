using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a button for actions, links, and commands.
    /// </summary>
    public partial class Button : ClearComponentBase
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// Defines the button style.
        /// </summary>
        [Parameter]
        public ButtonStyle? ButtonStyle { get; set; } = null;

        /// <summary>
        /// The color used for the button. What gets this color (background, text or outline)
        /// depends on the button style.
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        /// <summary>
        /// The size of the button
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// The shape of the button
        /// </summary>
        [Parameter]
        public ContainerShape Shape { get; set; } = ContainerShape.Rounded;

        /// <summary>
        /// The icon to be shown within the button
        /// </summary>
        [Parameter]
        public string? Icon { get; set; } = null; 

        /// <summary>
        /// The color of the icon within the button
        /// </summary>
        [Parameter]
        public Color? IconColor { get; set; } = null;

        /// <summary>
        /// The location of the icon within the button
        /// </summary>
        [Parameter]
        public IconLocation IconLocation { get; set; } = IconLocation.Start;

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

        /// <summary>
        /// Indicates if button is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; } = false;


        private string LabelStyle = string.Empty;

        private int Spacing { get; set; } = 5;
        internal ButtonStyle? StyleOverride { get; set; } = null;
        internal Color? ColorOverride { get; set; } = null;
        internal Color? _outlineColorOverride { get; set; } = null;
        internal Size? SizeOverride { get; set; } = null;

        internal Color? _iconColor = null;

        internal ToolTip? ToolTipElement { get; set; } = null;

        private bool _mouseOver = false;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        private string GetClasses()
        {
            if (!Disabled)
                return "clear-ripple ";

            return string.Empty;

        }

        protected override string UpdateStyle(string css)
        {
            Color? textColor = null;
            LabelStyle = string.Empty;

            css += "display:flex; ";
            css += "align-items: center; justify-content: center; ";
            var buttonStyle = GetButtonStyle();
            if (buttonStyle == ClearBlazor.ButtonStyle.Elevated)
                if (_mouseOver && !Disabled)
                    css += GetBoxShadowCss(3);
                else
                    css += GetBoxShadowCss(2);

            if (Dragging)
            {
                css += "pointer-events:none; ";
            }
            else
            {
                if (Disabled)
                    css += "cursor: default; pointer-events:none; ";
                else
                    css += "cursor: pointer; ";
            }
            var size = GetSize();

            css += GetBorderRadius();
            css += GetIconSize(size);
            var color = GetColor();
            switch (buttonStyle)
            {
                case ClearBlazor.ButtonStyle.Filled:
                    css += $"background: {GetFilledBackgroundColor(color).Value}; ";
                    textColor = GetFilledTextColor(color);
                    css += $"color: {GetFilledTextColor(color).Value}; ";
                    break;
                case ClearBlazor.ButtonStyle.Elevated:
                    css += $"background: {GetElevatedBackgroundColor(color).Value}; ";
                    textColor = GetElevatedTextColor(color);
                    css += $"color: {GetElevatedTextColor(color).Value}; ";
                    break;
                case ClearBlazor.ButtonStyle.Outlined:
                    textColor = GetTextOrOutlinedTextColor(color);
                    css += $"color: {GetTextOrOutlinedTextColor(color).Value}; ";
                    css += $"background: {GetTextOrOutlinedBackgroundColor().Value}; ";
                    break;
                case ClearBlazor.ButtonStyle.LabelOnly:
                    textColor = GetTextOrOutlinedTextColor(color);
                    css += $"color: {GetTextOrOutlinedTextColor(color).Value}; ";
                    css += $"background: {GetTextOrOutlinedBackgroundColor().Value}; ";
                    break;
            }
            css += GetBorder(buttonStyle, GetOutlineColor(color));

            if (textColor != null)
            {
                LabelStyle += $"color: {textColor.Value}; ";
                if (IconColor == null)
                    _iconColor = textColor;
                else
                    _iconColor = IconColor;
            }

            css += GetHeight(Size);

            css += GetPadding(size);

            TypographyBase typo;
            switch (size)
            {
                case Size.VerySmall:
                    typo = ThemeManager.CurrentTheme.Typography.ButtonVerySmall;
                    Spacing = 2;
                    break;
                case Size.Small:
                    typo = ThemeManager.CurrentTheme.Typography.ButtonSmall;
                    Spacing = 4;
                    break;
                case Size.Normal:
                    typo = ThemeManager.CurrentTheme.Typography.ButtonNormal;
                    Spacing = 6;
                    break;
                case Size.Large:
                    typo = ThemeManager.CurrentTheme.Typography.ButtonLarge;
                    Spacing = 8;
                    break;
                case Size.VeryLarge:
                    typo = ThemeManager.CurrentTheme.Typography.ButtonVeryLarge;
                    Spacing = 8;
                    break;
                default:
                    typo = ThemeManager.CurrentTheme.Typography.ButtonNormal;
                    Spacing = 6;
                    break;
            }

            if (ChildContent == null)
                Spacing = 0;



            LabelStyle += $"font-family: {string.Join(",", typo.FontFamily)}; ";

            LabelStyle += $"font-size: {typo.FontSize}; ";

            LabelStyle += $"font-weight: {typo.FontWeight}; ";

            LabelStyle += $"font-style: {GetFontStyle(typo.FontStyle)}; ";

            LabelStyle += $"line-height: {typo.LineHeight}; ";

            LabelStyle += $"letter-spacing: {typo.LetterSpacing}; ";

            LabelStyle += $"text-transform: {typo.TextTransform}; ";

            LabelStyle += "align-self:center; ";

            LabelStyle += "user-select: none; -ms-user-select: none; " +
                          "-webkit-user-select: none; -moz-user-select: none; " +
                          "cursor: default; ";

            return css;
        }

        protected virtual string GetBorderRadius()
        {
            switch (Shape)
            {
                case ContainerShape.Circle:
                    return "border-radius:50%; ";
                case ContainerShape.Square:
                    return "";
                case ContainerShape.Rounded:
                    return "border-radius:10px; ";
                case ContainerShape.FullyRounded:
                    return $"border-radius:{GetPxHeight(Size)/2}px; ";
            }
            return "";
        }

        protected virtual string GetIconSize(Size size)
        {
            return "";
        }

        protected virtual string GetPadding(Size size)
        {
            if (Padding != string.Empty)
                return Thickness.Parse(Padding).ThicknessToCss();

            switch (size)
            {
                case Size.VerySmall:
                    return $"padding: 0px 4px; ";
                case Size.Small:
                    return $"padding: 0px 16px; ";
                case Size.Normal:
                    return $"padding: 0px 16px; ";
                case Size.Large:
                    return $"padding: 0px 22px; ";
                case Size.VeryLarge:
                    return $"padding: 0px 28px; ";
                default:
                    return $"padding: 0px 16px; ";
            }
        }

        protected virtual string GetHeight(Size size)
        {
            return $"height:{GetPxHeight(size)}px; ";
        }

        protected virtual int GetPxHeight(Size size)
        {
            switch (size)
            {
                case Size.VerySmall:
                    return 30;
                case Size.Small:
                    return 34;
                case Size.Normal:
                    return 38;
                case Size.Large:
                    return 43;
                case Size.VeryLarge:
                    return 50;
                default:
                    return 38;
            }
        }

        protected virtual string GetBorder(ButtonStyle buttonStyle, Color color)
        {
            switch (buttonStyle)
            {
                case ClearBlazor.ButtonStyle.Filled:
                case ClearBlazor.ButtonStyle.Elevated:
                    return string.Empty;
                case ClearBlazor.ButtonStyle.Outlined:
                    return $"border-width: 1px; border-style: solid; border-color: " +
                           $"{GetOutlineColor(color)}; ";
                case ClearBlazor.ButtonStyle.LabelOnly:
                    return string.Empty;
            }
            return string.Empty;
        }

        protected async Task OnMouseEnter(MouseEventArgs e)
        {
            _mouseOver = true;
            if (ToolTipElement == null)
                await Task.CompletedTask;
            else
                ToolTipElement.ShowToolTip();
            StateHasChanged();
        }

        protected async Task OnMouseLeave(MouseEventArgs e)
        {
            _mouseOver = false;
            ToolTipElement?.HideToolTip();
            await Task.CompletedTask;
            StateHasChanged();
        }

        private Color GetFilledBackgroundColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.12);
            if (color == null)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Primary;
            return _mouseOver ? color.Darken(.1) : color;
        }

        private Color GetElevatedBackgroundColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.12);
            return _mouseOver ? ThemeManager.CurrentColorScheme.SurfaceContainer :
                                ThemeManager.CurrentColorScheme.SurfaceContainerLow;
        }

        private Color GetFilledTextColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.38);
            return Color.GetAssocTextColor(color);
        }

        private Color GetElevatedTextColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.38);
            if (color == null)
                return ThemeManager.CurrentColorScheme.Primary;
            return color;
        }

        private Color GetTextOrOutlinedTextColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.38);
            if (color == null)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) : 
                                    ThemeManager.CurrentColorScheme.Primary;
            return _mouseOver ? color.Darken(.1) : color;
        }

        private Color GetTextOrOutlinedBackgroundColor()
        {
            if (Disabled)
                return Color.Transparent;
            return _mouseOver ? ThemeManager.CurrentColorScheme.SurfaceContainerHighest.SetAlpha(.8) :
                                Color.Transparent;
        }

        internal Color GetOutlineColor(Color? color)
        {
            if (_outlineColorOverride != null)
                return _outlineColorOverride;

            if (Disabled)
            {
                if (Parent is ButtonGroup)
                    return color == null? ThemeManager.CurrentColorScheme.Primary : (Color)color;
                return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.38);
            }

            if (color == null)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Primary;

            return _mouseOver ? color.Darken(.1) : color;
        }

        protected bool GetDisabledState() => Disabled;

        private ButtonStyle GetButtonStyle()
        {
            if (ButtonStyle != null)
                return (ButtonStyle)ButtonStyle;

            if (StyleOverride != null)
                return (ButtonStyle)StyleOverride;

            return ClearBlazor.ButtonStyle.Filled;
        }
        internal Color? GetColor()
        {
            if (Color != null)
                return Color;

            if (ColorOverride != null)
                return ColorOverride;

            return Color;
        }
        internal Size GetSize()
        {
            if (SizeOverride != null)
                return (Size)SizeOverride;

            return Size;
        }

    }
}