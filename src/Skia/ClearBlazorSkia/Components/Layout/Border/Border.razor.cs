using Microsoft.AspNetCore.Components;
using SkiaSharp;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// By default a grid will occupy all of the available space given by its parent.
    /// In other words HorizontalAlignment and VerticalAlignment are both by default 'Stretch'.
    /// </summary>
    public partial class Border : PanelBase
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Console.WriteLine($"{Name}: Measure in:{constraint.Width}-{constraint.Height}");
            _measureIn = constraint;

            Size resultSize = new Size(0, 0);

            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            if (Children.Count == 1)
            {
                var child = Children[0] as PanelBase;
                if (child != null)
                {
                    Size childConstraint = new Size(Math.Max(0.0, constraint.Width),
                                                    Math.Max(0.0, constraint.Height));

                    child.Measure(childConstraint);
                    Size childSize = child.DesiredSize;

                    resultSize.Width = childSize.Width;
                    resultSize.Height = childSize.Height;
                }
                else
                {
                    // Combine into total decorating size
                    resultSize = new Size(0, 0);
                }
            }
            Console.WriteLine($"{Name}: Measure out:{resultSize.Width}-{resultSize.Height}");
            _measureOut = resultSize;

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Console.WriteLine($"{Name}: Arrange in:{finalSize.Width}-{finalSize.Height}");
            _arrangeIn = finalSize;
            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            Rect innerRect = new Rect(finalSize);

            if (Children.Count == 1)
            {
                var panel = Children[0] as PanelBase;
                if (panel != null)
                    panel.Arrange(innerRect);
            }
            Console.WriteLine($"{Name}: Arrange out:{finalSize.Width}-{finalSize.Height}");
            _arrangeOut = finalSize;

            return finalSize;
        }
    }
}
