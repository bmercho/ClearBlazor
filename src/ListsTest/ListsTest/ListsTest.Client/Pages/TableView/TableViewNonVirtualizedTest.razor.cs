using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class TableViewNonVirtualizedTest
    {
        private TableView<TestListRow> _table = null!;
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        List<TestListRow> _localListData = ClientData.LocalTestListRows100;
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

        async Task OnAddNewItem()
        {
            if (_table == null)
                return;
            var count = _localListData.Count();
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _table.Refresh();
        }
        async Task OnAddNewItemGotoEndIfAtEnd()
        {
            var atEnd = await _table.AtEnd();
            var count = _localListData.Count();
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _table.Refresh();
            if (atEnd)
                await _table.GotoEnd();
        }

        void ChangeItem()
        {
            _localListData[0].LastName = "Bla bla bla";
            _table.Refresh(_localListData[0]);
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

        private async Task ClearSelections()
        {
            await _table.RemoveAllSelections();
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