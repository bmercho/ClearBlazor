using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control that shows text.
    /// </summary>
    public partial class TextBlock : ClearComponentBase, IBackground
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// The text to be displayed. If null the control show the ChildContent
        /// </summary>
        [Parameter]
        public string? Text { get; set; }

        /// <summary>
        /// The color of the text
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Defines a predefined topography for the text to be shown.
        /// Generally this is all that is required to define the topography
        /// of the text. 
        /// </summary>
        [Parameter]
        public Typo? Typo { get; set; } = null;

        /// <summary>
        /// Defines the topography for the text to be shown
        /// Not used if typo is not null
        /// </summary>
        [Parameter]
        public TypographyBase? Typography { get; set; } = null;

        /// <summary>
        /// The font family of the text
        /// Overrides the font family defined in Typo or Typography
        /// </summary>
        [Parameter]
        public string? FontFamily { get; set; } = null;

        /// <summary>
        /// The font size of the text
        /// Overrides the font size defined in Typo or Typography
        /// </summary>
        [Parameter]
        public string? FontSize { get; set; } = null;

        /// <summary>
        /// The font weight of the text
        /// Overrides the font weight defined in Typo or Typography
        /// </summary>
        [Parameter]
        public int? FontWeight { get; set; } = null;

        /// <summary>
        /// The font style of the text
        /// Overrides the font style defined in Typo or Typography
        /// </summary>
        [Parameter]
        public FontStyle? FontStyle { get; set; } = null;

        /// <summary>
        /// The line height of the text
        /// Overrides the line height defined in Typo or Typography
        /// </summary>
        [Parameter]
        public double? LineHeight { get; set; } = null;

        /// <summary>
        /// The letter spacing of the text
        /// Overrides the letter spacing defined in Typo or Typography
        /// </summary>
        [Parameter]
        public string? LetterSpacing { get; set; } = null;

        /// <summary>
        /// The transform applied to the text
        /// Overrides the text transform defined in Typo or Typography
        /// </summary>
        [Parameter]
        public TextTransform? TextTransform { get; set; } = null;

        /// <summary>
        /// The text wrapping of the text
        /// Overrides the text wrapping defined in Typo or Typography
        /// </summary>
        [Parameter]
        public TextWrap? TextWrapping { get; set; } = null;

        /// <summary>
        /// The text trimming of the text
        /// Overrides the text trimming defined in Typo or Typography
        /// </summary>
        [Parameter]
        public bool? TextTrimming { get; set; } = null;

        /// <summary>
        /// The text decoration of the text
        /// </summary>
        [Parameter]
        public TextDecoration? TextDecoration { get; set; } = null;

        /// <summary>
        /// The horizontal alignment of the text within the TextBlock. 
        /// If alignment is set to Stretch the text is centered.
        /// </summary>
        [Parameter]
        public Alignment TextAlignment { get; set; } = Alignment.Start;

        /// <summary>
        /// Indicates if the text can be selected
        /// </summary>
        [Parameter]
        public bool IsTextSelectionEnabled { get; set; } = false;

        /// <summary>
        /// The tooltip string
        /// </summary>
        [Parameter]
        public string ToolTip { get; set; } = "";

        /// <summary>
        /// Indicates if the text can be clicked
        /// </summary>
        [Parameter]
        public bool Clickable { get; set; } = false;

        internal Color? ColorOverride { get; set; } = null;


        protected override string UpdateStyle(string css)
        {
            css += $"color: {GetColor().Value}; ";

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
                css += $"font-family: {string.Join(",", FontFamily)}; ";
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

            switch (TextDecoration)
            {
                case ClearBlazor.TextDecoration.Overline:
                case ClearBlazor.TextDecoration.OverlineUnderline:
                    // Remove overflow-x, if it exists, as it stops overline showing ??
                    if (css.Contains("overflow-x"))
                    {
                        var index1 = css.IndexOf("overflow-x");
                        var index2 = css.IndexOf(';', index1) + 1;
                        if (index1 >= 0 && index2 >= 0)
                            css = css.Substring(0, index1) + css.Substring(index2, css.Length - index2);
                    }
                    if (TextDecoration == ClearBlazor.TextDecoration.Overline)
                        css += $"text-decoration: overline; ";
                    else
                        css += $"text-decoration: underline overline; ";
                    break;
                case ClearBlazor.TextDecoration.LineThrough:
                    css += $"text-decoration: line-through; ";
                    break;
                case ClearBlazor.TextDecoration.Underline:
                    css += $"text-decoration: underline; ";
                    break;
                case null:
                    break;
            }

            if (!IsTextSelectionEnabled)
            {
                css += "user-select: none; -ms-user-select: none; " +
                       "-webkit-user-select: none; -moz-user-select: none; ";
                if (!ClearComponentBase.Dragging)
                    css += "cursor:default; ";
            }



            if (Clickable && !ClearComponentBase.Dragging)
                css += "cursor:pointer; ";

            return css;
        }

        protected void OnPointerOver()
        {
            StateHasChanged();
        }

        internal Color GetColor()
        {
            if (Color != null)
                return Color;

            if (ColorOverride != null)
                return ColorOverride;

            return ThemeManager.CurrentColorScheme.OnSurfaceVariant;
        }
    }
}