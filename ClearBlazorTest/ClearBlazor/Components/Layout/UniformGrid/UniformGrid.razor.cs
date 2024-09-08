using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// The UniformGrid control is a layout control which arranges items in a evenly-spaced set of rows or columns
    /// to fill the total available display space. Each cell in the grid, by default, will be the same size.
    /// If no value for Rows and Columns are provided, the UniformGrid will create a square layout
    /// based on the total number of visible items.
    /// If a fixed size is provided for Rows and Columns then additional children 
    /// that can't fit in the number of cells provided won't be displayed.
    /// </summary>
    public partial class UniformGrid:ClearComponentBase,IBackground,IBorder,IBoxShadow
    {
        /// <summary>
        /// The number of rows to display. If not provided the number of rows will be automatically determined. 
        /// </summary>
        [Parameter]
        public int? NumRows { get; set; } = null;

        /// <summary>
        /// The number of columns to display. If not provided the number of columns will be automatically determined. 
        /// </summary>
        [Parameter]
        public int? NumColumns { get; set; } = null;

        /// <summary>
        /// The spacing between rows.
        /// </summary>
        [Parameter]
        public int? RowSpacing { get; set; } = null;

        /// <summary>
        /// The spacing between columns.
        /// </summary>
        [Parameter]
        public int? ColumnSpacing { get; set; } = null;
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; } = null;

        /// <summary>
        /// See <a href=IBackgroundApi>IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        protected override string UpdateStyle(string css)
        {
            if (Children.Count > 0)
            {
                if (NumColumns == null && NumRows == null && Children.Count > 0)
                {
                    NumColumns = (int)Math.Ceiling(Math.Sqrt(Children.Count));
                    NumRows = (int)Math.Floor(Math.Sqrt(Children.Count));
                    NumColumns = Math.Max((int)NumColumns, (int)NumRows);
                    NumRows = NumColumns;
                }
                else if (NumColumns == null)
                    NumColumns = (int)Math.Ceiling(Children.Count / (double)NumRows);
                else if (NumRows == null)
                    NumRows = (int)Math.Ceiling(Children.Count / (double)NumColumns);

            }
            css +=  $"display: grid; grid-auto-rows:0; grid-auto-columns:0;" +
                       $" {GetRowSpacing()} {GetColumnSpacing()}" +
                       $"{GetTemplateCols()} {GetTemplateRows()}".Trim();
            return css;
        }

        protected override void AddChild(ClearComponentBase child)
        {
            base.AddChild(child);
            StateHasChanged();
        }

        private string GetRowSpacing()
        {
            if (RowSpacing == null)
                return string.Empty;

            return $"grid-row-gap: {RowSpacing}px; ";
        }
        private string GetColumnSpacing()
        {
            if (ColumnSpacing == null)
                return string.Empty;

            return $"grid-column-gap: {ColumnSpacing}px; ";
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
            for(int i=0;i< NumColumns;i++)
                cols.Add("1fr");

            return cols;
        }

        private IEnumerable<string> GetRows()
        {
            List<string> rows = new List<string>();
            for (int i = 0; i < NumRows; i++)
                rows.Add("1fr");

            return rows;
        }

        protected override string UpdateChildStyle(ClearComponentBase child, string css)
        {
            return css + $"grid-column: ; grid-row: ;";
        }
    }
}