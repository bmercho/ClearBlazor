using ClearBlazor;
using TestData;

namespace ListsTest
{
    public partial class TableViewPaginationLocalTest
    {
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

        async Task<(int, IEnumerable<TableRow>)> GetItemsLocallyAsync(DataProviderRequest request)
        {
            if (_localTableRows == null)
                return (0, new List<TableRow>());

            await Task.CompletedTask;
            return (_localTableRows.Count, _localTableRows.Skip(request.StartIndex).Take(request.Count));
        }

        async Task GotoIndexAsync(int row, Alignment alignment)
        {
            if (_table == null)
                return;
            await _table.GotoIndex(row, alignment);
        }
        async Task OnAddNewItemAsync()
        {
            var count = _localTableRows.Count;
            _localTableRows.Add(TableRow.GetNewTableRow(count));
            await _table.Refresh();
        }
        async Task OnAddNewItemGotoEndIfAtEndAsync()
        {
            var atEnd = await _table.AtEnd();
            var count = _localTableRows.Count;
            _localTableRows.Add(TableRow.GetNewTableRow(count));
            await _table.Refresh();
            if (atEnd)
                await _table.GotoEnd();
        }

        void ChangeItem()
        {
            _localTableRows[0].FirstName = "Bla bla bla";
            _table.Refresh(_localTableRows[0]);
        }

        async Task OnGotoEndAsync()
        {
            await _table.GotoEnd();
            StateHasChanged();
        }

        async Task OnGotoStartAsync()
        {
            await _table.GotoStart();
            StateHasChanged();
        }

        async Task OnNextPageAsync()
        {
            await _table.NextPage();
            StateHasChanged();
        }

        async Task OnPrevPageAsync()
        {
            await _table.PrevPage();
            StateHasChanged();
        }

        async Task CheckAtEndAsync()
        {
            _atEnd = await _table.AtEnd();
            StateHasChanged();
        }
        async Task CheckAtStartAsync()
        {
            _atStart = await _table.AtStart();
            StateHasChanged();
        }
        private async Task SelectionModeChangedAsync()
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