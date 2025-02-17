using Microsoft.AspNetCore.Components;
using SkiaSharp;
using System;
using Topten.RichTextKit;

namespace ClearBlazor
{
    public partial class TextBlock : ClearComponentBase, IBackground
    {
        [Parameter]
        public string? Text { get; set; } = null;

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
        public double? FontSize { get; set; } = null;

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
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

        [Parameter]
        public bool IsTextSelectionEnabled { get; set; } = false;

        [Parameter]
        public string ToolTip { get; set; } = "";

        [Parameter]
        public bool Clickable { get; set; } = false;

        RichString _richString = new RichString();
        protected override Size MeasureOverride(Size availableSize)
        {
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

            _richString = new RichString();
            _richString.EllipsisEnabled = true;
            //if (BackgroundColor != null)
            //_richString.BackgroundColor(@Color.BackgroundGrey.ToSKColor());
            if (Color != null)
                _richString.TextColor(Color.ToSKColor());
            else
                _richString.TextColor(ThemeManager.CurrentPalette.TextPrimary.ToSKColor());

            if (FontFamily != null)
                _richString.FontFamily(FontFamily);
            else
                _richString.FontFamily(string.Join(",", typo.FontFamily));

            if (FontSize != null)
                _richString.FontSize((float)FontSize);
            else
                _richString.FontSize((float)typo.FontSize);

            if (FontWeight != null)
                _richString.FontWeight((int)FontWeight);
            else
                _richString.FontWeight(typo.FontWeight);


            if (FontStyle != null)
            {
                if (FontStyle == ClearBlazor.FontStyle.Italic)
                    _richString.FontItalic(true);
            }
            else
                if (typo.FontStyle == ClearBlazor.FontStyle.Italic)
                    _richString.FontItalic(true);

            _richString.Add(Text, fontItalic: true);

            if (TextWrapping == TextWrap.Wrap)
                _richString.MaxWidth = (float)availableSize.Width;
            Size textSize = new Size(availableSize.Width, availableSize.Height);

            switch (HorizontalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    if (double.IsPositiveInfinity(availableSize.Width))
                    {
                        textSize.Width = _richString.MeasuredWidth;
                        if (TextWrapping == TextWrap.Wrap)
                            _richString.MaxWidth = _richString.MeasuredWidth;
                    }
                    break;
                case Alignment.Start:
                case Alignment.End:
                    textSize.Width = _richString.MeasuredWidth;
                    break;
            }
            switch (VerticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    if (double.IsPositiveInfinity(availableSize.Height))
                        textSize.Height = _richString.MeasuredHeight;
                    break;
                case Alignment.Start:
                case Alignment.End:
                    textSize.Height = _richString.MeasuredHeight;
                    break;
            }

            return textSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return arrangeSize;
        }

        protected override void DoPaint(SKCanvas canvas)
        {
            base.DoPaint(canvas);

            _richString.Paint(canvas, new SKPoint(0,0));
        }

    }
}