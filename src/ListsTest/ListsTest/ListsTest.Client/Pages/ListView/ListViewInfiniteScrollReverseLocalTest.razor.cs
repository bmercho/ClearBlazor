using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class ListViewInfiniteScrollReverseLocalTest
    {
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private ListView<TestListRow> _list = null!;
        private bool _atEnd = false;
        private bool _atStart = true;

        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

        async Task CheckAtStart()
        {
            if (_list == null)
                return;
            _atStart = await _list.AtStart();
            StateHasChanged();
        }

        private async Task OnGotoStart()
        {
            if (_list == null)
                return;
            await _list.GotoStart();
        }

        private async Task SelectionModeChanged()
        {
            if (_list == null)
                return;
            await _list.RemoveAllSelections();

            StateHasChanged();
        }
        private async Task ClearSelections()
        {
            await _list.RemoveAllSelections();
            StateHasChanged();
        }

        async Task OnAddNewItem()
        {
            var count = _localListData.Count;
            _localListData.Insert(0, TestListRow.GetNewTestListRow(count));
            await _list.RowAdded(_localListData[0].ListItemId);
        }
        async Task OnAddNewItemGotoStartIfAtStart()
        {
            var atStart = await _list.AtStart();
            var count = _localListData.Count;
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _list.Refresh();
            if (atStart)
                await _list.GotoStart();
        }

        void ChangeItem()
        {
            _localListData[0].FirstName = "Bla bla bla";
            _list.Refresh(_localListData[0]);
        }
        private async Task Up1()
        {
            await _list.Scroll(1);
        }
        private async Task Up5()
        {
            await _list.Scroll(5);

        }
        private async Task Up10()
        {
            await _list.Scroll(10);

        }
        private async Task Down1()
        {
            await _list.Scroll(-1);

        }
        private async Task Down5()
        {
            await _list.Scroll(-5);

        }
        private async Task Down10()
        {
            await _list.Scroll(-10);

        }

    }
}