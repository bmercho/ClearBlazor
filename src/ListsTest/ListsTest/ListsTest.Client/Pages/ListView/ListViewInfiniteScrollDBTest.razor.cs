using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class ListViewInfiniteScrollDBTest
    {
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private ListView<TestListRow> _list = null!;
        private bool _atEnd = false;
        private bool _atStart = true;
        private bool _addDelay = false;

        private async Task<(int, IEnumerable<TestListRow>)> GetItemsFromDatabase(ClearBlazor.DataProviderRequest request)
        {
            if (_addDelay)
                await Task.Delay(1000, request.CancellationToken);

            var feedEntries = await SignalRClient.Instance.GetListRows(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (feedEntries.TotalNumEntries, feedEntries.ListRows);
        }

        async Task CheckAtStart()
        {
            if (_list == null)
                return;
            _atStart = await _list.AtStart();
            StateHasChanged();
        }

        private async Task OnGotoStart()
        {
            if (_list == null)
                return;
            await _list.GotoStart();
        }

        private async Task SelectionModeChanged()
        {
            if (_list == null)
                return;
            await _list.RemoveAllSelections();

            StateHasChanged();
        }
        private void Refresh()
        {
            StateHasChanged();
        }
        private async Task ClearSelections()
        {
            await _list.RemoveAllSelections();
            StateHasChanged();
        }

    }
}