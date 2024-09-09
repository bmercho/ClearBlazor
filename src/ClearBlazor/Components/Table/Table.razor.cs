using ClearBlazor.Components.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Table<TItem> : ClearComponentBase, IBorder, IBackground,IBoxShadow
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public List<TItem> Items { get; set; } = new List<TItem>();

        [Parameter]
        public string ColumnDefs { get; set; } = "";

        [Parameter]
        public int RowSpacing { get; set; } = 5;

        [Parameter]
        public int ColumnSpacing { get; set; } = 5;

        [Parameter]
        public GridLines HorizontalGridLines { get; set; } = GridLines.None;

        [Parameter]
        public GridLines VerticalGridLines { get; set; } = GridLines.None;

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

        private List<TableColumn<TItem>> Columns { get; } = new List<TableColumn<TItem>>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; ";
            return css;
        }


        protected override void AddChild(ClearComponentBase child)
        {
            TableColumn<TItem>? column = child as TableColumn<TItem>;
            if (column != null && !Columns.Contains(column))
            {
                Columns.Add(column);
                StateHasChanged();
            }
        }

        private string[] GetLines(string content)
        {
            return content.Split('\r');
        }

        private string GetHeaderStyle(int column)
        {
            string justify = "start";
            switch (Columns[column-1].HeaderAlignment)
            {
                case Alignment.Stretch:
                    justify = "stretch";
                    break;
                case Alignment.Start:
                    justify = "start";
                    break;
                case Alignment.Center:
                    justify = "center";
                    break;
                case Alignment.End:
                    justify = "end";
                    break;
            }
            return $"display:grid; grid-area: 1 / {column} /span 1 /span 1; justify-self: {justify};";
        }

        private string GetRowStyle(int row, int column)
        {
            string justify = "start";
            switch (Columns[column-1].ContentAlignment)
            {
                case Alignment.Stretch:
                    justify = "stretch";
                    break;
                case Alignment.Start:
                    justify = "start";
                    break;
                case Alignment.Center:
                    justify = "center";
                    break;
                case Alignment.End:
                    justify = "end";
                    break;
            }
            return $"display:grid; grid-area: {row} / {column} /span 1 /span 1; justify-self: {justify};";
        }

        private string GetHorizontalGridLineStyle(int row, int columnCount)
        {
            return $"align-self:start; border-width:1px 0 0 0; border-style:solid; margin:{-RowSpacing/2}px 0 0 0; " +
                   $"grid-area: {row} / 1 / span 1 / span {columnCount}; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
        }

        private string GetVerticalGridLineStyle(int column, int rowCount)
        {
            return $"justify-self:start; border-width:0 0 0 1px; border-style:solid; margin:0 0 0 {-ColumnSpacing / 2}px; " +
                   $"grid-area: 1 / {column} / span {rowCount} / span 1; border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";
        }
    }
}