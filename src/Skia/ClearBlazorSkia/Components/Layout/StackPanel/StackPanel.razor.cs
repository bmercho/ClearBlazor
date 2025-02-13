using Microsoft.AspNetCore.Components;
using SkiaSharp;
using System.Data;

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
                //if (child is not PanelBase panel)
                //    continue;

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

            ContentSize = new Size(stackDesiredSize.Width, stackDesiredSize.Height);

            // Since we can offset and clip our content, we never need to be larger than the parent suggestion.
            // If we returned the full size of the content, we would always be so big we didn't need to scroll.  :)
            stackDesiredSize.Width = Math.Min(stackDesiredSize.Width, availableSize.Width);
            stackDesiredSize.Height = Math.Min(stackDesiredSize.Height, availableSize.Height);

            return stackDesiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize,
                                                double offsetHeight,
                                                double offsetWidth)

        {
            Rect rcChild = new Rect(new Size(arrangeSize.Width, arrangeSize.Height));
            double previousChildSize = 0.0;

            rcChild.Top += offsetHeight;
            rcChild.Left += offsetWidth;

            foreach (ClearComponentBase child in Children)
            {
                //if (child is not PanelBase panel)
                //    continue;

                switch (Orientation)
                {
                    case StackOrientation.Vertical:
                    case StackOrientation.VerticalReverse:
                        rcChild.Top += previousChildSize;
                        previousChildSize = child.DesiredSize.Height;
                        rcChild.Height = previousChildSize;
                        rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width); 
                        break;
                    case StackOrientation.Horizontal:
                    case StackOrientation.HorizontalReverse:
                        rcChild.Left += previousChildSize;
                        previousChildSize = child.DesiredSize.Width;
                        rcChild.Width = previousChildSize;
                        rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height); 
                        break;
                }
                child.Arrange(rcChild);
            }
            return arrangeSize;
        }
    }
}