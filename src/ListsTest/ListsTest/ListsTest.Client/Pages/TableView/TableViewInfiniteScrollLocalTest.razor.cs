using ClearBlazor;
using TestData;

namespace ListsTest
{
    public partial class TableViewInfiniteScrollLocalTest
    {
        private TableRow? _selectedItem = null;
        private List<TableRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private TableView<TableRow> _table = null!;
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

            await Task.CompletedTask;
            return (_localTableRows.Count, _localTableRows.Skip(request.StartIndex).Take(request.Count));
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
        private async Task ClearSelections()
        {
            await _table.RemoveAllSelections();
            StateHasChanged();
        }

    }
}