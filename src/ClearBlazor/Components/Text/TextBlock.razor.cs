using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class TextBlock : ClearComponentBase, IBackground,IColor
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseOverEventArgs> OnMouseOver { get; set; }

        [Parameter]
        public Color? Color { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        [Parameter]
        public Typo? Typo { get; set; } = null;

        [Parameter]
        public TypographyBase? Typography { get; set; } = null;

        [Parameter]
        public string? FontFamily { get; set; } = null;

        [Parameter]
        public string? FontSize { get; set; } = null;

        [Parameter]
        public int? FontWeight { get; set; } = null;

        [Parameter]
        public FontStyle? FontStyle { get; set; } = null;

        [Parameter]
        public double? LineHeight { get; set; } = null;

        [Parameter]
        public string? LetterSpacing { get; set; } = null;

        [Parameter]
        public TextTransform? TextTransform { get; set; } = null;

        [Parameter]
        public TextWrap? TextWrapping { get; set; } = null;

        [Parameter]
        public bool? TextTrimming { get; set; } = null;

        /// <summary>
        /// The horizontal alignment of the text within the TextBlock. 
        /// If alignment is set to Stretch the text is centered.
        /// </summary>
        [Parameter]
        public Alignment TextAlignment { get; set; } = Alignment.Start;

        [Parameter]
        public bool IsTextSelectionEnabled { get; set; } = false;

        [Parameter]
        public string ToolTip { get; set; } = "";

        [Parameter]
        public bool Clickable { get; set; } = false;


        protected override string UpdateStyle(string css)
        {
            if (Color != null)
                css += $"color: {Color.Value}; ";
            else
                css += $"color: {ThemeManager.CurrentPalette.TextPrimary.Value}; ";

            TypographyBase typo = ThemeManager.CurrentTheme.Typography.Default;
            if (Typo != null)
            {
                switch (Typo)
                {
                    case ClearBlazor.Typo.H1:
                        typo = ThemeManager.CurrentTheme.Typography.H1;
                        break;
                    case ClearBlazor.Typo.H2:
                        typo = ThemeManager.CurrentTheme.Typography.H2;
                        break;
                    case ClearBlazor.Typo.H3:
                        typo = ThemeManager.CurrentTheme.Typography.H3;
                        break;
                    case ClearBlazor.Typo.H4:
                        typo = ThemeManager.CurrentTheme.Typography.H4;
                        break;
                    case ClearBlazor.Typo.H5:
                        typo = ThemeManager.CurrentTheme.Typography.H5;
                        break;
                    case ClearBlazor.Typo.H6:
                        typo = ThemeManager.CurrentTheme.Typography.H6;
                        break;
                    case ClearBlazor.Typo.Subtitle1:
                        typo = ThemeManager.CurrentTheme.Typography.Subtitle1;
                        break;
                    case ClearBlazor.Typo.Subtitle2:
                        typo = ThemeManager.CurrentTheme.Typography.Subtitle2;
                        break;
                    case ClearBlazor.Typo.Body1:
                        typo = ThemeManager.CurrentTheme.Typography.Body1;
                        break;
                    case ClearBlazor.Typo.Body2:
                        typo = ThemeManager.CurrentTheme.Typography.Body2;
                        break;
                    case ClearBlazor.Typo.Button:
                        typo = ThemeManager.CurrentTheme.Typography.ButtonNormal;
                        break;
                    case ClearBlazor.Typo.Caption:
                        typo = ThemeManager.CurrentTheme.Typography.Caption;
                        break;
                    case ClearBlazor.Typo.Overline:
                        typo = ThemeManager.CurrentTheme.Typography.Overline;
                        break;
                    case null:
                        break;
                }
            }
            else if (Typography != null)
            {
                typo = Typography;
            } 

            if (FontFamily != null)
                css += $"font-family: {string.Join(",",FontFamily)}; ";
            else
                css += $"font-family: {string.Join(",", typo.FontFamily)}; ";

            if (FontSize != null)
                css += $"font-size: {FontSize}; ";
            else
                css += $"font-size: {typo.FontSize}; ";

            if (FontWeight != null)
                css += $"font-weight: {FontWeight}; ";
            else
                css += $"font-weight: {typo.FontWeight}; ";

            if (FontStyle != null)
                css += $"font-style: {GetFontStyle((FontStyle)FontStyle)}; ";
            else
                css += $"font-style: {GetFontStyle(typo.FontStyle)}; ";

            //if (LineHeight != null)
            //    css += $"line-height: {LineHeight}; ";
            //else
            //    css += $"line-height: {typo.LineHeight}; ";

            if (LetterSpacing != null)
                css += $"letter-spacing: {LetterSpacing}; ";
            else
                css += $"letter-spacing: {typo.LetterSpacing}; ";

            if (TextTransform != null)
                css += $"text-transform: {GetTextTransform((TextTransform)TextTransform)}; ";
            else
                if (typo.TextTransform != null)
                    css += $"text-transform: {typo.TextTransform}; ";
            switch (TextAlignment)
            {
                case Alignment.Stretch:
                    css += "text-align:center; ";
                    break;
                case Alignment.Start:
                    css += "text-align:left; ";
                    break;
                case Alignment.Center:
                    css += "text-align:center; ";
                    break;
                case Alignment.End:
                    css += "text-align:right; ";
                    break;
            }
            if (TextWrapping != null)
            {
                switch (TextWrapping)
                {
                    case TextWrap.Wrap:
                        css += $"white-space: normal; ";
                        break;
                    case TextWrap.NoWrap:
                        css += $"white-space: nowrap; "; 
                        break;
                    case TextWrap.WrapOnNewLines:
                        css += $"white-space: pre; ";
                        break;
                    case null:
                        break;
                }
            }
            else
            {
                switch (typo.TextWrapping)
                {
                    case TextWrap.Wrap:
                        css += $"white-space: normal; ";
                        break;
                    case TextWrap.NoWrap:
                        css += $"white-space: nowrap; ";
                        break;
                    case TextWrap.WrapOnNewLines:
                        css += $"white-space: pre; ";
                        break;
                }
            }

            if (TextTrimming != null)
            {
                if ((bool)TextTrimming)
                    css += $"white-space: nowrap; overflow:hidden; text-overflow: ellipsis; ";
            }
            else
                if (typo.TextTrimming)
                    css += $"white-space: nowrap;overflow:hidden; text-overflow: ellipsis; ";

            if (!IsTextSelectionEnabled)
                css += "user-select: none; -ms-user-select: none; " +
                       "-webkit-user-select: none; -moz-user-select: none; " +
                       "cursor: default; ";

            if (Clickable)
                css += "cursor:pointer; ";

            return css;
        }
    }
}