using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.RegularExpressions;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// </summary>
    public partial class Grid:ClearComponentBase,IContent,IBackground,IBoxShadow, IBorder, IBackgroundGradient, IDraggable
    {
        [Parameter]
        public string Rows { get; set; } = "*";

        [Parameter]
        public string Columns { get; set; } = "*";

        [Parameter]
        public double RowSpacing { get; set; } = 0;

        [Parameter]
        public double ColumnSpacing { get; set; } = 0;

        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColour { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }


        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

        [Parameter]
        public string? BackgroundGradient1 { get; set; } = null;
        [Parameter]
        public string? BackgroundGradient2 { get; set; } = null;

        [Parameter]
        public bool IsDraggable { get; set; } = false;

        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnElementMouseEnter { get; set; }

        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnElementMouseLeave { get; set; }


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