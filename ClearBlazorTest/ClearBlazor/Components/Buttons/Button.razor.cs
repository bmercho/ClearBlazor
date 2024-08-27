using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace ClearBlazor
{
    public partial class Button : ClearComponentBase, IContent,IColour
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public TextEditFillMode? ButtonStyle { get; set; } = null;

        [Parameter]
        public Color? Colour { get; set; } = null;

        [Parameter]
        public bool DisableBoxShadow { get; set; } = false;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public ContainerShape Shape { get; set; } = ContainerShape.Square;

        [Parameter]
        public string? Icon { get; set; } = null; 

        [Parameter]
        public Color? IconColour { get; set; } = null;

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
        internal Color? ColourOverride { get; set; } = null;
        internal Size? SizeOverride { get; set; } = null;

        internal ToolTip? ToolTipElement { get; set; } = null;

        private bool _mouseOver = false;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            VerticalAlignment = Alignment.Center;
        }
        protected override void ComputeOwnClasses(StringBuilder sb)
        {
            base.ComputeOwnClasses(sb); 
            if (!Disabled)
                sb.Append("clear-ripple tooltip-wrapper");
        }

        protected override string UpdateStyle(string css)
        {
            Color? textColour = null;
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
                css += "cursor: default; ";
            else
                css += "cursor: pointer; ";

            var size = GetSize();

            css += GetBorderRadius();
            css += GetIconSize(size);
            var colour = GetColour();
            switch (buttonStyle)
            {
                case TextEditFillMode.Filled:
                    css += $"background: {GetFilledBackgroundColour(colour).Value}; ";
                    textColour = GetFilledTextColour(colour);
                    css += $"color: {GetFilledTextColour(colour).Value}; ";
                    break;
                case TextEditFillMode.Outline:
                    textColour = GetTextOrOutlinedTextColour(colour);
                    css += $"color: {GetTextOrOutlinedTextColour(colour).Value}; ";
                    css += $"border-width: 1px; border-style: solid; border-color: {GetOutlineColour(colour).Value}; ";
                    css += $"background: {GetTextOrOutlinedBackgroundColour().Value}; ";
                    break;
                case TextEditFillMode.None:
                    textColour = GetTextOrOutlinedTextColour(colour);
                    css += $"color: {GetTextOrOutlinedTextColour(colour).Value}; ";
                    css += $"background: {GetTextOrOutlinedBackgroundColour().Value}; ";
                    break;
            }
            css += GetBorder(buttonStyle, GetOutlineColour(colour));

            if (textColour != null)
            {
                LabelStyle += $"color: {textColour.Value}; ";
                if (IconColour == null)
                    IconColour = textColour;
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

            return css;
        }

        protected virtual string GetBorderRadius()
        {
            if (Shape == ContainerShape.Circle)
                return "border-radius:50%; ";

            return "border-radius:4px; ";
        }

        protected virtual string GetIconSize(Size size)
        {
            return "";
        }

        protected virtual string GetPadding(Size size)
        {
            if (Shape == ContainerShape.Circle)
                return string.Empty;

            switch (size)
            {
                case Size.VerySmall:
                    return $"padding: 4px 0px; ";
                case Size.Small:
                    return $"padding: 10px 0px; ";
                case Size.Normal:
                    return $"padding: 16px 0px; ";
                case Size.Large:
                    return $"padding: 22px 0px; ";
                case Size.VeryLarge:
                    return $"padding: 28px 0px; ";
                default:
                    return $"padding: 16px 0px; ";
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

        protected virtual string GetBorder(TextEditFillMode buttonStyle, Color colour)
        {
            switch (buttonStyle)
            {
                case TextEditFillMode.Filled:
                    return $"border-width: 0px; ";
                case TextEditFillMode.Outline:
                    return $"border-width: 1px; border-style: solid; border-color: {colour.Value}; ";
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

        private Color GetFilledBackgroundColour(Color? colour)
        {
            if (Disabled)
                return ThemeManager.CurrentPalette.BackgroundDisabled;
            if (colour == null)
                return _mouseOver ? ThemeManager.CurrentPalette.Primary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Primary;
            if (colour == Color.Primary)
                return _mouseOver ? ThemeManager.CurrentPalette.Primary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Primary;
            if (colour == Color.Secondary)
                return _mouseOver ? ThemeManager.CurrentPalette.Secondary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Secondary;
            if (colour == Color.Tertiary)
                return _mouseOver ? ThemeManager.CurrentPalette.Tertiary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Tertiary;
            if (colour == Color.Info)
                return _mouseOver ? ThemeManager.CurrentPalette.Info.Darken(.1) :
                                    ThemeManager.CurrentPalette.Info;
            if (colour == Color.Success)
                return _mouseOver ? ThemeManager.CurrentPalette.Success.Darken(.1) :
                                    ThemeManager.CurrentPalette.Success;
            if (colour == Color.Warning)
                return _mouseOver ? ThemeManager.CurrentPalette.Warning.Darken(.1) :
                                    ThemeManager.CurrentPalette.Warning;
            if (colour == Color.Error)
                return _mouseOver ? ThemeManager.CurrentPalette.Error.Darken(.1) :
                                    ThemeManager.CurrentPalette.Error;
            if (colour == Color.Dark)
                return _mouseOver ? ThemeManager.CurrentPalette.Dark.Darken(.1) :
                                    ThemeManager.CurrentPalette.Dark;

            return _mouseOver ? colour.Darken(.2) : colour;
        }

        private Color GetFilledTextColour(Color? colour)
        {
            if (Disabled)
                return ThemeManager.CurrentPalette.TextDisabled;
            return Color.GetAssocTextColour(colour);
        }

        private Color GetTextOrOutlinedTextColour(Color? colour)
        {
            if (Disabled)
                return ThemeManager.CurrentPalette.TextDisabled;
            if (colour == null)
                return _mouseOver ? ThemeManager.CurrentPalette.Primary.Darken(.1) : ThemeManager.CurrentPalette.Primary;
            if (colour == Color.Primary)
                return _mouseOver ? ThemeManager.CurrentPalette.Primary.Darken(.1) : ThemeManager.CurrentPalette.Primary;
            if (colour == Color.Secondary)
                return _mouseOver ? ThemeManager.CurrentPalette.Secondary.Darken(.1) : ThemeManager.CurrentPalette.Secondary;
            if (colour == Color.Tertiary)
                return _mouseOver ? ThemeManager.CurrentPalette.Tertiary.Darken(.1) : ThemeManager.CurrentPalette.Tertiary;
            if (colour == Color.Info)
                return _mouseOver ? ThemeManager.CurrentPalette.Info.Darken(.1) : ThemeManager.CurrentPalette.Info;
            if (colour == Color.Success)
                return _mouseOver ? ThemeManager.CurrentPalette.Success.Darken(.1) : ThemeManager.CurrentPalette.Success;
            if (colour == Color.Warning)
                return _mouseOver ? ThemeManager.CurrentPalette.Warning.Darken(.1) : ThemeManager.CurrentPalette.Warning;
            if (colour == Color.Error)
                return _mouseOver ? ThemeManager.CurrentPalette.Error.Darken(.1) : ThemeManager.CurrentPalette.Error;
            if (colour == Color.Dark)
                return _mouseOver ? ThemeManager.CurrentPalette.Dark.Darken(.1) : ThemeManager.CurrentPalette.Dark;

            return _mouseOver ? colour.Darken(.1) : colour;
        }

        private Color GetTextOrOutlinedBackgroundColour()
        {
            if (Disabled)
                return Color.Transparent;
            return _mouseOver ? ThemeManager.CurrentPalette.GrayLighter.SetAlpha(.2) :
                                Color.Transparent;
        }

        private Color GetOutlineColour(Color? colour)
        {
            if (Disabled)
            {
                if (Parent is ButtonGroup)
                    return colour == null? ThemeManager.CurrentPalette.Primary : (Color)colour;
                return ThemeManager.CurrentPalette.TextDisabled;
            }
            if (colour == null)
                return _mouseOver ? ThemeManager.CurrentPalette.Primary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Primary;
            if (colour == Color.Primary)
                return _mouseOver ? ThemeManager.CurrentPalette.Primary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Primary;
            if (colour == Color.Secondary)
                return _mouseOver ? ThemeManager.CurrentPalette.Secondary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Secondary;
            if (colour == Color.Tertiary)
                return _mouseOver ? ThemeManager.CurrentPalette.Tertiary.Darken(.1) :
                                    ThemeManager.CurrentPalette.Tertiary;
            if (colour == Color.Info)
                return _mouseOver ? ThemeManager.CurrentPalette.Info.Darken(.1) :
                                    ThemeManager.CurrentPalette.Info;
            if (colour == Color.Success)
                return _mouseOver ? ThemeManager.CurrentPalette.Success.Darken(.1) :
                                    ThemeManager.CurrentPalette.Success;
            if (colour == Color.Warning)
                return _mouseOver ? ThemeManager.CurrentPalette.Warning.Darken(.1) :
                                    ThemeManager.CurrentPalette.Warning;
            if (colour == Color.Error)
                return _mouseOver ? ThemeManager.CurrentPalette.Error.Darken(.1) :
                                    ThemeManager.CurrentPalette.Error;
            if (colour == Color.Dark)
                return _mouseOver ? ThemeManager.CurrentPalette.Dark.Darken(.1) :
                                    ThemeManager.CurrentPalette.Dark;


            return _mouseOver ? colour.Darken(.1) : colour;
        }

        protected bool GetDisabledState() => Disabled;

        private TextEditFillMode GetButtonStyle()
        {
            if (ButtonStyleOverride != null)
                return (TextEditFillMode)ButtonStyleOverride;

            if (ButtonStyle != null)
                return (TextEditFillMode)ButtonStyle;

            return TextEditFillMode.Filled;
        }
        private Color? GetColour()
        {
            if (Colour != null)
                return Colour;

            if (ColourOverride != null)
                return ColourOverride;

            return Colour;
        }
        internal Size GetSize()
        {
            if (SizeOverride != null)
                return (Size)SizeOverride;

            return Size;
        }

    }
}