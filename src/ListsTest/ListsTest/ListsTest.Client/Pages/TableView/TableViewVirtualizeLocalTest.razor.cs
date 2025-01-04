using ClearBlazor;
using Microsoft.AspNetCore.Components;
using Data;

namespace ListsTest
{
    public partial class TableViewVirtualizeLocalTest
        : ComponentBase
    {
        private TableView<TestListRow> _table = null!;
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _showHeader = false;
        private bool _stickyHeader = true;
        private int _rowSpacing = 10;
        private int _columnSpacing = 10;
        private bool _horizontalScrollbar = true;
        private GridLines _horizontalGridLines = GridLines.None;
        private GridLines _verticalGridLines = GridLines.None;

        private bool _atEnd = false;
        private bool _atStart = true;

        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

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
        async Task Refresh()
        {
            StateHasChanged();
            await _table.Refresh();
        }

    }
}