using ClearBlazor;

namespace ListsTest
{
    public partial class ListViewPaginationDBTest
    {
        ListView<FeedEntry> _list = null!;
        private FeedEntry? _selectedItem = null;
        private List<FeedEntry> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        private async Task<(int, IEnumerable<FeedEntry>)> GetItemsFromDatabase(ClearBlazor.DataProviderRequest request)
        {
            FeedEntryResult feedEntries = await SignalRClient.Instance.GetFeedEntries(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (feedEntries.TotalNumEntries, feedEntries.FeedEntries);
        }

        async Task GotoIndex(int row, Alignment alignment)
        {
            if (_list == null)
                return;
            await _list.GotoIndex(row, alignment);
        }

        async Task OnGotoEnd()
        {
            await _list.GotoEnd();
            StateHasChanged();
        }

        async Task OnGotoStart()
        {
            await _list.GotoStart();
            StateHasChanged();
        }

        async Task OnNextPage()
        {
            await _list.NextPage();
            StateHasChanged();
        }

        async Task OnPrevPage()
        {
            await _list.PrevPage();
            StateHasChanged();
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

    }
}