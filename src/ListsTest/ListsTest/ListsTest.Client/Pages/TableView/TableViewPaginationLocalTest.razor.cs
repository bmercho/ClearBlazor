using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class TableViewPaginationLocalTest
    {
        const int PageSize = 14;
        private TableView<TestListRow> _table = null!;
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;
        private int _selectedPage = 3;
        private int _totalNumItems = 0;
        private int _numPages = 0;

        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

        protected override async Task OnInitializedAsync()
        {
            _totalNumItems = _localListData.Count;
            _numPages = (int)Math.Ceiling(_totalNumItems / (Double)PageSize);

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                await _table.GotoPage(_selectedPage);
        }
        async Task PageChanged(int page)
        {
            await _table.GotoPage(page);
            _selectedPage = page;
        }

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