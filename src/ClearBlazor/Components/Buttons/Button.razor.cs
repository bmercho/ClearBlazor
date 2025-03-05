using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace ClearBlazor
{
    public partial class Button : ClearComponentBase, IColor
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public TextEditFillMode? ButtonStyle { get; set; } = null;

        [Parameter]
        public Color? Color { get; set; } = null;

        [Parameter]
        public Color? OutlineColor { get; set; } = null;

        [Parameter]
        public bool DisableBoxShadow { get; set; } = false;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public ContainerShape Shape { get; set; } = ContainerShape.SquareRounded;

        [Parameter]
        public string? Icon { get; set; } = null; 

        [Parameter]
        public Color? IconColor { get; set; } = null;

        [Parameter]
        public IconLocation IconLocation { get; set; } = IconLocation.Start;

        [Parameter]
        public string? ToolTip { get; set; } = null;

        [Parameter]
        public ToolTipPosition? ToolTipPosition { get; set; } = null;


        [Parameter]
        public int? ToolTipDelay { get; set; } = null; // Milliseconds

        [Parameter]
        public bool Disabled { get; set; } = false;


        public string LabelStyle = string.Empty;

        public int Spacing { get; set; } = 5;
        internal TextEditFillMode? ButtonStyleOverride { get; set; } = null;
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
        protected override void ComputeOwnClasses(StringBuilder sb)
        {
            base.ComputeOwnClasses(sb); 
            if (!Disabled)
                sb.Append("clear-ripple tooltip-wrapper ");
        }

        protected override string UpdateStyle(string css)
        {
            Color? textColor = null;
            LabelStyle = string.Empty;

            css += "display:flex; ";
            css += "align-items: center; justify-content: center; ";
            var buttonStyle = GetButtonStyle();
            if (buttonStyle == TextEditFillMode.Filled && !DisableBoxShadow)
                if (_mouseOver && !Disabled)
                    css += GetBoxShadowCss(3);
                else
                    css += GetBoxShadowCss(2);

            if (Disabled)
                css += "cursor: default; pointer-events:none; ";
            else
                css += "cursor: pointer; ";

            var size = GetSize();

            css += GetBorderRadius();
            css += GetIconSize(size);
            var color = GetColor();
            switch (buttonStyle)
            {
                case TextEditFillMode.Filled:
                    css += $"background: {GetFilledBackgroundColor(color).Value}; ";
                    textColor = GetFilledTextColor(color);
                    css += $"color: {GetFilledTextColor(color).Value}; ";
                    break;
                case TextEditFillMode.Outline:
                    textColor = GetTextOrOutlinedTextColor(color);
                    css += $"color: {GetTextOrOutlinedTextColor(color).Value}; ";
                    css += $"background: {GetTextOrOutlinedBackgroundColor().Value}; ";
                    break;
                case TextEditFillMode.None:
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
                case ContainerShape.SquareRounded:
                    return "border-radius:4px; ";
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
            switch (size)
            {
                case Size.VerySmall:
                    return "height:30px; ";
                case Size.Small:
                    return "height:34px; ";
                case Size.Normal:
                    return "height:38px; ";
                case Size.Large:
                    return "height:43px; ";
                case Size.VeryLarge:
                    return "height:50px; ";
                default:
                    return "height:38px; ";
            }
        }

        protected virtual string GetBorder(TextEditFillMode buttonStyle, Color color)
        {
            switch (buttonStyle)
            {
                case TextEditFillMode.Filled:
                    return $"border-width: 0px; ";
                case TextEditFillMode.Outline:
                    return $"border-width: 1px; border-style: solid; border-color: " +
                           $"{GetOutlineColor(color)}; ";
                case TextEditFillMode.None:
                    return $"border-width: 0px; ";
            }
            return $"border-width: 0px; ";
        }

        protected async Task OnMouseEnter(MouseEventArgs e)
        {
            _mouseOver = true;
            if (ToolTipElement == null)
                await Task.CompletedTask;
            else
                await ToolTipElement.ShowToolTip();
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
                return ThemeManager.CurrentColorScheme.BackgroundDisabled;
            if (color == null)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Primary;
            if (color == Color.Primary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Primary;
            if (color == Color.Secondary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Secondary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Secondary;
            if (color == Color.Tertiary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Tertiary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Tertiary;
            if (color == Color.Info)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Info.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Info;
            if (color == Color.Success)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Success.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Success;
            if (color == Color.Warning)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Warning.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Warning;
            if (color == Color.Error)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Error.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Error;
            if (color == Color.Dark)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Dark.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Dark;

            return _mouseOver ? color.Darken(.2) : color;
        }

        private Color GetFilledTextColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.TextDisabled;
            return Color.GetAssocTextColor(color);
        }

        private Color GetTextOrOutlinedTextColor(Color? color)
        {
            if (Disabled)
                return ThemeManager.CurrentColorScheme.TextDisabled;
            if (color == null)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) : ThemeManager.CurrentColorScheme.Primary;
            if (color == Color.Primary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) : ThemeManager.CurrentColorScheme.Primary;
            if (color == Color.Secondary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Secondary.Darken(.1) : ThemeManager.CurrentColorScheme.Secondary;
            if (color == Color.Tertiary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Tertiary.Darken(.1) : ThemeManager.CurrentColorScheme.Tertiary;
            if (color == Color.Info)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Info.Darken(.1) : ThemeManager.CurrentColorScheme.Info;
            if (color == Color.Success)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Success.Darken(.1) : ThemeManager.CurrentColorScheme.Success;
            if (color == Color.Warning)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Warning.Darken(.1) : ThemeManager.CurrentColorScheme.Warning;
            if (color == Color.Error)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Error.Darken(.1) : ThemeManager.CurrentColorScheme.Error;
            if (color == Color.Dark)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Dark.Darken(.1) : ThemeManager.CurrentColorScheme.Dark;

            return _mouseOver ? color.Darken(.1) : color;
        }

        private Color GetTextOrOutlinedBackgroundColor()
        {
            if (Disabled)
                return Color.Transparent;
            return _mouseOver ? ThemeManager.CurrentColorScheme.GrayLighter.SetAlpha(.2) :
                                Color.Transparent;
        }

        internal Color GetOutlineColor(Color? color)
        {
            if (_outlineColorOverride != null)
                return _outlineColorOverride;

            if (OutlineColor != null)
                return OutlineColor;

            if (Disabled)
            {
                if (Parent is ButtonGroup)
                    return color == null? ThemeManager.CurrentColorScheme.Primary : (Color)color;
                return ThemeManager.CurrentColorScheme.TextDisabled;
            }

            if (color == null)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Primary;
            if (color == Color.Primary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Primary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Primary;
            if (color == Color.Secondary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Secondary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Secondary;
            if (color == Color.Tertiary)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Tertiary.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Tertiary;
            if (color == Color.Info)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Info.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Info;
            if (color == Color.Success)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Success.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Success;
            if (color == Color.Warning)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Warning.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Warning;
            if (color == Color.Error)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Error.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Error;
            if (color == Color.Dark)
                return _mouseOver ? ThemeManager.CurrentColorScheme.Dark.Darken(.1) :
                                    ThemeManager.CurrentColorScheme.Dark;


            return _mouseOver ? color.Darken(.1) : color;
        }

        protected bool GetDisabledState() => Disabled;

        private TextEditFillMode GetButtonStyle()
        {
            if (ButtonStyle != null)
                return (TextEditFillMode)ButtonStyle;

            if (ButtonStyleOverride != null)
                return (TextEditFillMode)ButtonStyleOverride;

            return TextEditFillMode.Filled;
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