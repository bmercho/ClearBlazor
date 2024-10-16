﻿using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class TableColumn<TItem>: ClearComponentBase where TItem : ListItem

    {
        [CascadingParameter]
        public TableView<TItem>? Table { get; set; } = null;

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public Func<TItem, object> Field { get; set; } = null!;

        [Parameter]
        public RenderFragment<string>? HeaderTemplate { get; set; }

        [Parameter]
        public RenderFragment<TItem>? DataTemplate { get; set; }

        [Parameter]
        public Alignment HeaderAlignment { get; set; } = Alignment.Start;

        [Parameter]
        public Alignment ContentAlignment { get; set; } = Alignment.Start;
    }
}
