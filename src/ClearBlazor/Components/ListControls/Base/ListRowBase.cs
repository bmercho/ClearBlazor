﻿using Microsoft.AspNetCore.Components;
using ClearBlazor;

namespace ClearBlazorInternal
{
    public class ListRowBase<TItem> : ClearComponentBase
           where TItem : ListItem
    {
        [Parameter]
        public required TItem RowData { get; set; }

        [Parameter]
        public int RowIndex { get; set; }

        internal bool _doRender = true;
        internal bool _mouseOver = false;

        internal void SetRowData(TItem rowData)
        {
            RowData = rowData;
        }

        public void Refresh()
        {
            _doRender = true;
            StateHasChanged();
        }

        internal void Unhighlight()
        {
            _mouseOver = false;
            Refresh();
        }


    }
}
