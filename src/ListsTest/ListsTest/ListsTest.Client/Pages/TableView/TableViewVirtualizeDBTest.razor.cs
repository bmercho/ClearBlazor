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
        List<TableRow> _localTableRows = new();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            var result = await SignalRClient.Instance.GetTableRows(0, 500);
            _localTableRows = result.TableRows;
            StateHasChanged();
            await _table.Refresh();
        }

        async Task<(int, IEnumerable<TableRow>)> GetItemsLocally(DataProviderRequest request)
        {
            if (_localTableRows == null)
                return (0, new List<TableRow>());

            if (_addDelay)
                await Task.Delay(400, request.CancellationToken);

            await Task.CompletedTask;
            return (_localTableRows.Count, _localTableRows.Skip(request.StartIndex).Take(request.Count));
        }

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

        private async Task Up1()
        {
            await _table.Scroll(1);
        }
        private async Task Up5()
        {
            await _table.Scroll(5);

        }
        private async Task Up10()
        {
            await _table.Scroll(10);

        }
        private async Task Down1()
        {
            await _table.Scroll(-1);

        }
        private async Task Down5()
        {
            await _table.Scroll(-5);

        }
        private async Task Down10()
        {
            await _table.Scroll(-10);

        }

    }
}