using ClearBlazor;
using Data;
using Microsoft.AspNetCore.Components;

namespace ListsTest
{
    public partial class TableViewInfiniteScrollDBTest : ComponentBase
    {
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private TableView<TestListRow> _table = null!;
        private bool _atEnd = false;
        private bool _atStart = true;
        private bool _addDelay = false;

        private async Task<(int, IEnumerable<TestListRow>)> GetItemsFromDatabaseAsync(DataProviderRequest request)
        {
            if (_addDelay)
                await Task.Delay(1000, request.CancellationToken);

            var tableRows = await SignalRClient.Instance.GetListRows(
                                                             request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (tableRows.TotalNumEntries, tableRows.ListRows);
        }

        async Task CheckAtStart()
        {
            if (_table == null)
                return;
            _atStart = await _table.AtStart();
            StateHasChanged();
        }

        private async Task OnGotoStart()
        {
            if (_table == null)
                return;
            await _table.GotoStart();
        }

        private async Task SelectionModeChanged()
        {
            if (_table == null)
                return;
            await _table.RemoveAllSelections();

            StateHasChanged();
        }
        private void Refresh()
        {
            StateHasChanged();
        }
        private async Task ClearSelections()
        {
            await _table.RemoveAllSelections();
            StateHasChanged();
        }

    }
}