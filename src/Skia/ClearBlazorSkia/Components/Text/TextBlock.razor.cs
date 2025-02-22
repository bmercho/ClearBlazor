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
        public int? FontWeight { get; set; }

        [Parameter]
        public FontStyle? FontStyle { get; set; } = null;

        //[Parameter]
        //public double? LineHeight { get; set; } = null;

        //[Parameter]
        //public string? LetterSpacing { get; set; } = null;

        //[Parameter]
        //public TextTransform? TextTransform { get; set; } = null;

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

        SKPaint _paint = new SKPaint();
        SKFont _font = new SKFont();
        SKRect bounds;
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
            _paint = new()
            {
                Color = SKColors.Yellow,
                IsAntialias = true,
            };

            string family;
            if (FontFamily != null)
                family = FontFamily;
            else
                family = string.Join(",", typo.FontFamily);

            float size;
            if (FontSize != null)
                size = (float)FontSize;
            else
                size = (float)typo.FontSize;

            SKFontStyleWeight weight;
            if (FontWeight != null)
                weight = (SKFontStyleWeight)FontWeight;
            else
                weight = SKFontStyleWeight.Normal;// typo.FontWeight;


            //if (FontStyle != null)
            //{
            //    if (FontStyle == ClearBlazor.FontStyle.Italic)
            //        _richString.FontItalic(true);
            //}
            //else
            //    if (typo.FontStyle == ClearBlazor.FontStyle.Italic)
            //        _richString.FontItalic(true);

            _font = new()
            {
                Size = size,
                Typeface = SKTypeface.FromFamilyName(
                                 family, weight,
                                 SKFontStyleWidth.Normal,
                                 SKFontStyleSlant.Italic)
            };
            _font.MeasureText(Text, out bounds, _paint );

//            _richString.EllipsisEnabled = true;
            //if (BackgroundColor != null)
            //_richString.BackgroundColor(@Color.BackgroundGrey.ToSKColor());
//            if (Color != null)
//                _richString.TextColor(Color.ToSKColor());
//            else
//                _richString.TextColor(ThemeManager.CurrentPalette.TextPrimary.ToSKColor());




            //_richString.Add(Text, fontItalic: true);

            //if (TextWrapping == TextWrap.Wrap)
            //    _richString.MaxWidth = (float)availableSize.Width;
            Size textSize = new Size(availableSize.Width, availableSize.Height);
            textSize.Width = bounds.Width;

            //switch (HorizontalAlignment)
            //{
            //    case Alignment.Stretch:
            //    case Alignment.Center:
            //        if (double.IsPositiveInfinity(availableSize.Width))
            //        {
            //            textSize.Width = bounds.Width;
            //            //if (TextWrapping == TextWrap.Wrap)
            //            //    _richString.MaxWidth = bounds.Width;
            //        }
            //        break;
            //    case Alignment.Start:
            //    case Alignment.End:
            //        textSize.Width = bounds.Width;
            //        break;
            //}
            textSize.Height = bounds.Height;
            //switch (VerticalAlignment)
            //{
            //    case Alignment.Stretch:
            //    case Alignment.Center:
            //        if (double.IsPositiveInfinity(availableSize.Height))
            //            textSize.Height = bounds.Height;
            //        break;
            //    case Alignment.Start:
            //    case Alignment.End:
            //        textSize.Height = bounds.Height;
            //        break;
            //}

            return textSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return arrangeSize;
        }

        protected override void DoPaint(SKCanvas canvas)
        {
            base.DoPaint(canvas);

            canvas.DrawText(Text, 0, bounds.Height, _font, _paint);
            //_richString.Paint(canvas, new SKPoint(0,0));
        }

    }
}