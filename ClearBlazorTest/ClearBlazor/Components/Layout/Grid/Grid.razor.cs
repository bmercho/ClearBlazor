using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.RegularExpressions;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// By default a grid will occupy all of the available space given by its parent.
    /// In other words HorizontalAlignment and VerticalAlignment are both by default 'Stretch'.
    /// </summary>
    public partial class Grid:ClearComponentBase,IContent,IBackground,IBoxShadow, IBorder, IBackgroundGradient
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
        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColor { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }

        // IBoxShadow
        [Parameter]
        public int? BoxShadow { get; set; }

        // IBackground
        [Parameter]
        public Color? BackgroundColor { get; set; }

        // IBackgroundGradient
        [Parameter]
        public string? BackgroundGradient1 { get; set; }
        [Parameter]
        public string? BackgroundGradient2 { get; set; }


        private readonly Regex _proportionPattern = new Regex("^[0-9]*\\*$");
        private readonly Regex _fixedSizePattern = new Regex("^[0-9]*$");

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; {GetWidth()}{GetHeight()} {GetTemplateCols()} {GetTemplateRows()}".Trim();
            if (RowSpacing != 0)
                css += $"row-gap: {RowSpacing}px; ";
            if (ColumnSpacing != 0)
                css += $"column-gap: {ColumnSpacing}px; ";

            return css;
        }

        private string GetWidth()
        {
            if (double.IsNaN(Width))
                return "";
            return "width: " + Width.ToString() + "px; ";
        }

        private string GetHeight()
        {
            if (double.IsNaN(Height))
                return "";
            return "height: " + Height.ToString() + "px; ";
        }

        private string GetTemplateCols()
        {
            var columns = GetColumns();
            return columns == null ? string.Empty : $"grid-template-columns: " + string.Join(" ", columns) + "; ";
        }

        private string GetTemplateRows()
        {
            var rows = GetRows();
            return rows == null ? string.Empty : $"grid-template-rows: " + string.Join(" ", rows) + "; ";
        }

        private IEnumerable<string> GetColumns()
        {
            List<string> cols = new List<string>();
            if (Columns == null)
            {
                cols.Add("1fr");
                return cols;
            }

            var tokens = Columns.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
                cols.Add(GetLengths(token.Trim()));
            return cols;
        }

        private IEnumerable<string> GetRows()
        {
            List<string> rows = new List<string>();
            if (Rows == null)
            {
                rows.Add("1fr");
                return rows;
            }

            var tokens = Rows.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
                rows.Add(GetLengths(token.Trim()));
            return rows;
        }

        private string GetLengths(string lengths)
        {
            if (lengths.Length == 0)
                return "1fr";
            var tokens = lengths.Split(':');
            if (tokens.Length == 1)
                return GetLength(tokens[0].Trim());
            else if (tokens.Length == 2)
                return GetLength(tokens[0].Trim(), tokens[1].Trim());
            else if (tokens.Length == 3)
                return GetLength(tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim());
            else
                return "1fr";
        }

        private string GetLength(string length, string? min = null, string? max = null)
        {
            if (min != null || max != null)
                return $"minmax({ConvertToCss(string.IsNullOrEmpty(min) ? "1" : min)},{ConvertToCss(max != null ? max : ConvertLength(length))})";
            else
                return ConvertToCss(length);
        }

        private string ConvertToCss(string data)
        {
            if (string.IsNullOrEmpty(data))
                return "1fr";
            if (data == "1fr")
                return "1fr";
            if (data == "min-content")
                return "min-content";
            if (data == "*" || data == "1fr")
                return "1fr";
            if (data.ToLower() == "auto")
                return "auto";
            if (_fixedSizePattern.IsMatch(data))
                return data + "px";
            if (_proportionPattern.IsMatch(data))
                return data.Replace("*", "fr");
            throw new FormatException($"'{data}' is not a valid format for 'length'");
        }

        private string ConvertLength(string data)
        {
            if (string.IsNullOrEmpty(data))
                return "1fr";
            if (data == "*" || data == "1fr")
                return "1fr";
            if (data.ToLower() == "auto")
                return "min-content";
            if (_fixedSizePattern.IsMatch(data))
                return data + "px";
            if (_proportionPattern.IsMatch(data))
                return data.Replace("*", "fr");
            throw new FormatException($"'{data}' is not a valid format for 'length'");
        }

        protected override string UpdateChildStyle(ClearComponentBase child, string css)
        {
            return css + $"grid-area: {child.Row + 1} / {child.Column + 1} / span {child.RowSpan} / span {child.ColumnSpan};";
        }
    }
}