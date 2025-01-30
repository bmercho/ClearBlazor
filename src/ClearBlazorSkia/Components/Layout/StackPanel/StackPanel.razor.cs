using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// By default a grid will occupy all of the available space given by its parent.
    /// In other words HorizontalAlignment and VerticalAlignment are both by default 'Stretch'.
    /// </summary>
    public partial class StackPanel : PanelBase, IBorder
    {
        /// <summary>
        /// Defines the orientation of the stack panel. Landscape or portrait
        /// </summary>
        [Parameter]
        public StackOrientation Orientation { get; set; } = StackOrientation.Vertical;

        /// <summary>
        /// The spacing between children in the direction defined by Orientation.
        /// </summary>
        [Parameter]
        public double Spacing { get; set; } = 0;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        protected override void DoPaint(SKCanvas canvas)
        {
            base.DoPaint(canvas);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size stackDesiredSize = new Size(0, 0);

            Size layoutSlotSize = availableSize;
            double childLogicalSize;

            switch (Orientation)
            {
                case StackOrientation.Vertical:
                case StackOrientation.VerticalReverse:
                    layoutSlotSize.Height = Double.PositiveInfinity;
                    break;
                case StackOrientation.Horizontal:
                case StackOrientation.HorizontalReverse:
                    layoutSlotSize.Width = Double.PositiveInfinity;
                    break;
            }

            foreach (ClearComponentBase child in Children)
            {
                child.Measure(layoutSlotSize);
                Size childDesiredSize = child.DesiredSize;

                switch (Orientation)
                {
                    case StackOrientation.Vertical:
                    case StackOrientation.VerticalReverse:
                        stackDesiredSize.Width = Math.Max(stackDesiredSize.Width, childDesiredSize.Width);
                        stackDesiredSize.Height += childDesiredSize.Height;
                        childLogicalSize = childDesiredSize.Height;
                        break;
                    case StackOrientation.Horizontal:
                    case StackOrientation.HorizontalReverse:
                        stackDesiredSize.Width += childDesiredSize.Width;
                        stackDesiredSize.Height = Math.Max(stackDesiredSize.Height, childDesiredSize.Height);
                        childLogicalSize = childDesiredSize.Width;
                        break;
                }
            }

            return stackDesiredSize;
        }

        protected override void ArrangeOverride(double left, double top)
        {
            Rect rcChild = new Rect(new Size(left, top));
            double previousChildSize = 0.0;

            foreach (ClearComponentBase child in Children)
            {
                switch (Orientation)
                {
                    case StackOrientation.Vertical:
                    case StackOrientation.VerticalReverse:
                        rcChild.Top += previousChildSize;
                        previousChildSize = child.DesiredSize.Height;
                        rcChild.Height = previousChildSize;
                        rcChild.Width = Math.Max(left, child.DesiredSize.Width); break;
                    case StackOrientation.Horizontal:
                    case StackOrientation.HorizontalReverse:
                        rcChild.Left += previousChildSize;
                        previousChildSize = child.DesiredSize.Width;
                        rcChild.Width = previousChildSize;
                        rcChild.Height = Math.Max(top, child.DesiredSize.Height); break;
                }
                child.Arrange(rcChild.Left, rcChild.Top);
            }
        }
    }
}