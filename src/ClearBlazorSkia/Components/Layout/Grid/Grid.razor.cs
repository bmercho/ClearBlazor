using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SkiaSharp;
using System;
using System.Text.RegularExpressions;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// By default a grid will occupy all of the available space given by its parent.
    /// In other words HorizontalAlignment and VerticalAlignment are both by default 'Stretch'.
    /// </summary>
    public partial class Grid : ClearComponentBase, IBorder
    {
        /// <summary>
        /// Defines columns by a comma delimited string of column widths. 
        /// A column width consists of one to three values separated by colons. The seconds and third values are optional.
        /// The first value can be one of:
        ///    '*'    - a weighted proportion of available space.
        ///    'auto' - the minimum width of the content
        ///    value  - a pixel value for the width
        /// The second value is the minimum width in pixels
        /// The third value is the maximum width in pixels
        /// 
        /// eg *,2*,auto,200  - Four columns, the 3rd column auto sizes to content, the 4th column is 200px wide, and the remaining space
        /// is shared between columns 1 and 2 but column 2 is twice as wide as column 1.
        /// eg *:100:200,* - Two columns sharing available width equally except column 1 must be a minimum of 100px and a maximum of 200px. 
        /// So if the available width is 600px then column 1 will be 200px and column 2 400px.
        /// If the available width is 150px then column 1 will be 100px and column 2 50px.
        /// If the available width is 300px then column 1 will be 150px and column 2 150px.
        /// </summary>        
        [Parameter]
        public string Columns { get; set; } = "*";

        /// <summary>
        /// Defines rows by a comma delimited string of row heights which are similar to columns. 
        /// </summary>
        [Parameter]
        public string Rows { get; set; } = "*";

        /// <summary>
        /// The spacing in pixels between each column
        /// </summary>
        [Parameter]
        public double ColumnSpacing { get; set; } = 0;

        /// <summary>
        /// The spacing in pixels between each row
        /// </summary>
        [Parameter]
        public double RowSpacing { get; set; } = 0;

        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        // IBorder

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        // IBoxShadow

        /// <summary>
        /// See <a href="IBoxShadowApi">IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; }

        // IBackground

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        // IBackgroundGradient

        /// <summary>
        /// See <a href="IBackgroundGradientApi">IBackgroundGradient</a>
        /// </summary>
        [Parameter]
        public string? BackgroundGradient1 { get; set; }


        /// <summary>
        /// See <a href="IBackgroundGradientApi">IBackgroundGradient</a>
        /// </summary>
        [Parameter]
        public string? BackgroundGradient2 { get; set; }


        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        internal override void PaintCanvas(SKCanvas canvas)
        {
            canvas.ClipRect(new SKRect((float)Left, (float)Top, 
                                       (float)ActualWidth, (float)ActualHeight));

            if (BackgroundColor != null)
                canvas.Clear(BackgroundColor.ToSKColor());
            SKPaint strokePaint1 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                IsAntialias = true,
                StrokeWidth = (float)_borderThickness.Right
            };
            canvas.DrawRect(new SKRect((float)Left, (float)Top,
                                       (float)ActualWidth, (float)ActualHeight), strokePaint1);
            base.PaintCanvas(canvas);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size resultSize = new Size(0, 0);

            foreach (ClearComponentBase child in Children)
            {
                child.Measure(availableSize);
                resultSize.Width = Math.Max(resultSize.Width, child.DesiredSize.Width);
                resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height);
            }

            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ?
                resultSize.Width : availableSize.Width;

            resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ?
                resultSize.Height : availableSize.Height;

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (ClearComponentBase child in Children)
            {
                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
            }
            ActualWidth = finalSize.Width;
            ActualHeight = finalSize.Height;
            return finalSize;
        }
    }
}