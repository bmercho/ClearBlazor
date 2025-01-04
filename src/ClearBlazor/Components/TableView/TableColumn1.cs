using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class TableColumn1<TItem>: ClearComponentBase where TItem : ListItem

    {
        [CascadingParameter]
        public TableView<TItem>? Table { get; set; } = null;

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public string ColumnDefinition { get; set; } = "auto";

        [Parameter]
        public Func<TItem, object> Field { get; set; } = null!;

        [Parameter]
        public RenderFragment<string>? HeaderTemplate { get; set; }

        [Parameter]
        public RenderFragment<TItem>? DataTemplate { get; set; }

        [Parameter]
        public Alignment HorizontalHeaderAlignment { get; set; } = Alignment.Start;

        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Start;

        [Parameter]
        public Alignment VerticalContentAlignment { get; set; } = Alignment.Start;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }


    }
}
