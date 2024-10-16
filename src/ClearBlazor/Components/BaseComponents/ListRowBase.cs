﻿using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class ListRowBase<TItem> : ClearComponentBase
           where TItem : ListItem
    {
        [Parameter]
        public required TItem RowData { get; set; }

        [Parameter]
        public int RowIndex { get; set; }

        internal bool _doRender = true;

        public void Refresh()
        {
            _doRender = true;
            StateHasChanged();
        }

    }
}
