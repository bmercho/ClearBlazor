using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class TableViewPaginationLocalTest
    {
        private TableView<TestListRow> _table = null!;
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

        async Task GotoIndexAsync(int row, Alignment alignment)
        {
            if (_table == null)
                return;
            await _table.GotoIndex(row, alignment);
        }
        async Task OnAddNewItemAsync()
        {
            var count = _localListData.Count;
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _table.Refresh();
        }
        async Task OnAddNewItemGotoEndIfAtEndAsync()
        {
            var atEnd = await _table.AtEnd();
            var count = _localListData.Count;
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _table.Refresh();
            if (atEnd)
                await _table.GotoEnd();
        }

        void ChangeItem()
        {
            _localListData[0].FirstName = "Bla bla bla";
            _table.Refresh(_localListData[0]);
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