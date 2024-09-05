using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class UniformGrid:ClearComponentBase,IBackground,IBorder,IBoxShadow
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColor { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }

        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        [Parameter]
        public int? NumRows { get; set; } = null;

        [Parameter]
        public int? NumColumns { get; set; } = null;

        [Parameter]
        public int? RowSpacing { get; set; } = null;

        [Parameter]
        public int? ColumnSpacing { get; set; } = null;

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