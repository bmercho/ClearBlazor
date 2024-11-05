using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class ListViewPaginationDBTest
    {
        ListView<TestListRow> _list = null!;
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        private async Task<(int, IEnumerable<TestListRow>)> GetItemsFromDatabase(ClearBlazor.DataProviderRequest request)
        {
            var feedEntries = await SignalRClient.Instance.GetListRows(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (feedEntries.TotalNumEntries, feedEntries.ListRows);
        }

        async Task GotoIndex(int row, Alignment alignment)
        {
            if (_list == null)
                return;
            await _list.GotoIndex(row, alignment);
        }

        async Task OnGotoEnd()
        {
            await _list.GotoEnd();
            StateHasChanged();
        }

        async Task OnGotoStart()
        {
            await _list.GotoStart();
            StateHasChanged();
        }

        async Task OnNextPage()
        {
            await _list.NextPage();
            StateHasChanged();
        }

        async Task OnPrevPage()
        {
            await _list.PrevPage();
            StateHasChanged();
        }

        async Task CheckAtEnd()
        {
            _atEnd = await _list.AtEnd();
            StateHasChanged();
        }
        async Task CheckAtStart()
        {
            _atStart = await _list.AtStart();
            StateHasChanged();
        }
        private async Task SelectionModeChanged()
        {
            if (_list == null)
                return;
            await _list.RemoveAllSelections();
            StateHasChanged();
        }
        private async Task ClearSelections()
        {
            await _list.RemoveAllSelections();
            StateHasChanged();
        }

    }
}