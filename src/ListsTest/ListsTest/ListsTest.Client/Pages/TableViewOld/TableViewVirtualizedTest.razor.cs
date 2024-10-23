using ClearBlazor;
using LoremNET;
using Microsoft.AspNetCore.Components;

namespace ListsTest
{
    public partial class TableViewVirtualizedTest : ComponentBase
    {
        private TableRow _selectedItem = null!;
        private List<TableRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        TableView<TableRow> _table = null!;
        List<TableRow> _localTableRows = new();
        bool _addDelay = false;
        private bool _atEnd = false;
        private bool _atStart = true;
        private bool _stickyHeader = true;
        private bool _showHeader = true;
        private GridLines _verticalGridLines = GridLines.None;
        private GridLines _horizontalGridLines = GridLines.None;

        protected override async Task OnInitializedAsync()
        {
            var result = await SignalRClient.Instance.GetTableRows(0, 200, new CancellationToken());
            _localTableRows = result.TableRows;
            StateHasChanged();
            await _table.Refresh();
        }

        async Task<(int, IEnumerable<TableRow>)> GetItemsLocally(ClearBlazor.DataProviderRequest request)
        {
            if (_localTableRows == null)
                return (0, new List<TableRow>());

            await Task.CompletedTask;
            return (0, _localTableRows.Skip(request.StartIndex).Take(request.Count));
        }

        async Task GotoIndex(int row, Alignment alignment)
        {
            if (_table == null)
                return;
            await _table.GotoIndex(row, alignment);
        }

        async Task OnAddNewItem()
        {
            if (_table == null)
                return;
            var count = _localTableRows.Count();
            _localTableRows.Add(GetNextRow());
            await _table.Refresh();
        }

        async Task OnGotoEnd()
        {
            if (_table == null)
                return;
            await _table.GotoEnd();
        }

        async Task OnGotoStart()
        {
            if (_table == null)
                return;
            await _table.GotoStart();
        }

        async Task OnAddNewItemGotoEndIfAtEnd()
        {
            if (_table == null)
                return;
            var atEnd = await _table.AtEnd();

            _localTableRows.Add(GetNextRow());
            await _table.Refresh();
            if (atEnd)
                await _table.GotoEnd();
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

        private async Task Changed()
        {
            if (_table == null)
                return;
            await _table.Refresh(true);
        }

        private TableRow GetNextRow()
        {
            var count = _localTableRows.Count();
            return new TableRow
            {
                TableRowId = Guid.NewGuid(),
                Index = count,
                FirstName = TestData.GetRandomFirstName(),
                LastName = TestData.GetRandomSurname(),
                Product = TestData.GetRandomProduct(),
                Available = TestData.GetRandomAvailable(),
                Quantity = TestData.GetRandomQuantity(),
                UnitPrice = (Decimal)TestData.GetRandomPrice(),
                Notes = Lorem.Words(2, 40),
            };
        }
    }
}