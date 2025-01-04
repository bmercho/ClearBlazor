using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class TableViewInfiniteScrollLocalTest1
    {
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private TableView1<TestListRow> _table = null!;
        private bool _atEnd = false;
        private bool _atStart = true;
        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

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

        private async Task AddToStart()
        {
            if (_table == null)
                return;
            _localListData.Insert(0, TestListRow.GetNewTestListRow(-1));
            await _table.Refresh();
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