using ClearBlazor;
using TestData;
using Microsoft.AspNetCore.Components;

namespace ListsTest
{
    public partial class TableViewVirtualizeDBTest
        : ComponentBase
    {
        private bool _addDelay = false;
        private TableView<TableRow> _table = null!;
        private TableRow? _selectedItem = null;
        private List<TableRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        private async Task<(int, IEnumerable<TableRow>)> GetItemsFromDatabase(DataProviderRequest request)
        {
            if (_addDelay)
                await Task.Delay(1000, request.CancellationToken);

            TableRowResult tableRows = await SignalRClient.Instance.GetTableRows(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (tableRows.TotalNumEntries, tableRows.TableRows);
        }

        private void Refresh()
        {
            StateHasChanged();
        }
        async Task GotoIndex(int row, Alignment alignment)
        {
            if (_table == null)
                return;
            await _table.GotoIndex(row, alignment);
        }

        async Task OnGotoEnd()
        {
            await _table.GotoEnd();
        }

        async Task OnGotoStart()
        {
            await _table.GotoStart();
        }

        private async Task ClearSelections()
        {
            await _table.RemoveAllSelections();
            StateHasChanged();
        }

        async Task CheckAtEnd()
        {
            _atEnd = await _table.AtEnd();
            StateHasChanged();
        }
        async Task CheckAtStart()
        {
            _atStart = await _table.AtStart();
            StateHasChanged();
        }

        private async Task SelectionModeChanged()
        {
            if (_table == null)
                return;
            await _table.RemoveAllSelections();
            StateHasChanged();
        }

    }
}