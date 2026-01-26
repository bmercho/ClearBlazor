using Microsoft.AspNetCore.Components;
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

        internal bool DoRender { get; set; } = true;
        internal bool MouseOver { get; set; } = false;

        internal void SetRowData(TItem rowData)
        {
            RowData = rowData;
        }

        public void Refresh()
        {
            DoRender = true;
            StateHasChanged();
        }

        internal void Unhighlight()
        {
            MouseOver = false;
            Refresh();
        }


    }
}
