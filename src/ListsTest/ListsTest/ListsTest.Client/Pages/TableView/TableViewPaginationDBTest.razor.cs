using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class TableViewPaginationDBTest
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

        protected override async Task OnInitializedAsync()
        {
            var tableRows = await SignalRClient.Instance.GetListRows(0, 1, null);
            _totalNumItems = tableRows.TotalNumEntries;
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

        private async Task<(int, IEnumerable<TestListRow>)> GetItemsFromDatabaseAsync(DataProviderRequest request)
        {
            var tableRows = await SignalRClient.Instance.GetListRows(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (tableRows.TotalNumEntries, tableRows.ListRows);
        }

        async Task GotoIndex(int row, Alignment alignment)
        {
            if (_table == null)
                return;
            await _table.GotoIndex(row, alignment);
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

    }
}