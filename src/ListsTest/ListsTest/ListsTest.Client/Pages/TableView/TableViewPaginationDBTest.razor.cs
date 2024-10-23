using ClearBlazor;

namespace ListsTest
{
    public partial class TableViewPaginationDBTest
    {
        private TableView<TableRow> _table = null!;
        private TableRow? _selectedItem = null;
        private List<TableRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        private async Task<(int, IEnumerable<TableRow>)> GetItemsFromDatabaseAsync(DataProviderRequest request)
        {
            TableRowResult tableRows = await SignalRClient.Instance.GetTableRows(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (tableRows.TotalNumEntries, tableRows.TableRows);
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
            StateHasChanged();
        }

        async Task OnGotoStart()
        {
            await _table.GotoStart();
            StateHasChanged();
        }

        async Task OnNextPage()
        {
            await _table.NextPage();
            StateHasChanged();
        }

        async Task OnPrevPage()
        {
            await _table.PrevPage();
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
        private async Task ClearSelections()
        {
            await _table.RemoveAllSelections();
            StateHasChanged();
        }

    }
}