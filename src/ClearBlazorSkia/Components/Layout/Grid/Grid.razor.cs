using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SkiaSharp;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// By default a grid will occupy all of the available space given by its parent.
    /// In other words HorizontalAlignment and VerticalAlignment are both by default 'Stretch'.
    /// </summary>
    public partial class Grid : PanelBase, IBorder
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

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
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
            Size resultSize = new Size(0, 0);

            Size border = HelperCollapseThickness(_borderThickness);
            Size padding = HelperCollapseThickness(_paddingThickness);

            // Combine into total decorating size
            resultSize = new Size(border.Width + padding.Width, border.Height + padding.Height);

            foreach (ClearComponentBase child in Children)
            {
                // Combine into total decorating size
                Size combined = new Size(border.Width + padding.Width, border.Height + padding.Height);

                // Remove size of border only from child's reference size.
                Size childConstraint = new Size(Math.Max(0.0, availableSize.Width - combined.Width),
                                                Math.Max(0.0, availableSize.Height - combined.Height));

                child.Measure(childConstraint);
                Size childSize = child.DesiredSize;

                // Now use the returned size to drive our size, by adding back the margins, etc.
                resultSize.Width = childSize.Width + combined.Width;
                resultSize.Height = childSize.Height + combined.Height;
            }

            return resultSize;
        }

        protected override void ArrangeOverride(double left, double top)
        {
            foreach (ClearComponentBase child in Children)
            {
                child.Arrange(left, top);
            }
        }
    }
}