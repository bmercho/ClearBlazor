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
        public string? BorderThickness { get; set; } = null;

        [Parameter]
        public Color? BorderColour { get; set; } = null;

        [Parameter]
        public string? CornerRadius { get; set; } = "0";

        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

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
            return $"display:grid; grid-area: 0 / {column} /span 1 /span 1; justify-self: {justify};";
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
    }
}