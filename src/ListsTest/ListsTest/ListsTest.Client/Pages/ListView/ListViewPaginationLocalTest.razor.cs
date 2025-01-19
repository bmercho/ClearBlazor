using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class ListViewPaginationLocalTest
    {
        ListView<TestListRow> _list = null!;
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;
        private int _selectedPage = 3;

        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                await _list.GotoPage(_selectedPage);
        }

        async Task GotoIndex(int row, Alignment alignment)
        {
            if (_list == null)
                return;
            await _list.GotoIndex(row, alignment);
        }
        async Task OnAddNewItem()
        {
            var count = _localListData.Count;
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _list.Refresh();
        }
        async Task OnAddNewItemGotoEndIfAtEnd()
        {
            var atEnd = _list.AtEnd();
            var count = _localListData.Count;
            _localListData.Add(TestListRow.GetNewTestListRow(count));
            await _list.Refresh();
            if (await atEnd)
                await _list.GotoEnd();
        }

        async Task PageChanged(int page)
        {
            await _list.GotoPage(page);
            _selectedPage = page;
        }

        void ChangeItem()
        {
            _localListData[0].FirstName = "Bla bla bla";
            _list.Refresh(_localListData[0]);
        }

        async Task CheckAtEnd()
        {
            _atEnd = await _list.AtEnd();
            StateHasChanged();
        }
        async Task CheckAtStart()
        {
            _atStart = await _list.AtStart();
            StateHasChanged();
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

    }
}