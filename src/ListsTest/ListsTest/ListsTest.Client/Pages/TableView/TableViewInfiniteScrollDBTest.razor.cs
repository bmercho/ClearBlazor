using ClearBlazor;
using TestData;
using Microsoft.AspNetCore.Components;

namespace ListsTest
{
    public partial class TableViewInfiniteScrollDBTest : ComponentBase
    {
        private TableRow? _selectedItem = null;
        private List<TableRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private TableView<TableRow> _table = null!;
        private bool _atEnd = false;
        private bool _atStart = true;
        private bool _addDelay = false;

        private async Task<(int, IEnumerable<TableRow>)> GetItemsFromDatabaseAsync(DataProviderRequest request)
        {
            if (_addDelay)
                await Task.Delay(1000, request.CancellationToken);

            TableRowResult tableRows = await SignalRClient.Instance.GetTableRows(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (tableRows.TotalNumEntries, tableRows.TableRows);
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