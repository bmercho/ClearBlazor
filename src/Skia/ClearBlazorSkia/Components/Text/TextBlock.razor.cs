using Microsoft.AspNetCore.Components;
using SkiaSharp;
using Topten.RichTextKit;

namespace ClearBlazor
{
    public partial class TextBlock : ClearComponentBase, IBackground
    {
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
            _richString = new RichString()
                .BackgroundColor(Color.Custom("yellow").ToSKColor())
              .Alignment(TextAlignment)
              .FontFamily("Segoe UI")
              .MarginBottom(20)
              .Add("Welcome To RichTextKit", fontSize: 24, fontWeight: 700, fontItalic: true)
              .Paragraph().Alignment(TextAlignment.Left)
              .FontSize(18)
              .Add("This is a test string");
            _richString.MaxWidth = (float)availableSize.Width;
            Size textSize = new Size(availableSize.Width, availableSize.Height);

            switch (HorizontalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    if (double.IsPositiveInfinity(availableSize.Width))
                    {
                        textSize.Width = _richString.MeasuredWidth;
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

        protected override Size ArrangeOverride(Size arrangeSize,
                                                double offsetHeight,
                                                double offsetWidth)
        {
            return arrangeSize;
        }

        protected override void DoPaint(SKCanvas canvas)
        {
            base.DoPaint(canvas);

            _richString.Paint(canvas, new SKPoint((float)Left, (float)Top));
        }

    }
}