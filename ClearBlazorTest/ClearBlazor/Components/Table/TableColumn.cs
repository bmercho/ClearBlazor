﻿using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class TableColumn<TItem>:ClearComponentBase
    {
        [CascadingParameter]
        public Table<TItem>? Table { get; set; } = null;

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public Func<TItem, object> Field { get; set; } = null!;

        [Parameter]
        public RenderFragment<string>? HeaderTemplate { get; set; }
    }
}
