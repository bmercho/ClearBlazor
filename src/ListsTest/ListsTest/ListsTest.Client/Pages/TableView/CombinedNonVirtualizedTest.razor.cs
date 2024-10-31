using ClearBlazor;
using TestData;

namespace ListsTest
{
    public partial class CombinedNonVirtualizedTest
    {
        private TableView<FeedEntry> _table = null!;
        private FeedEntry? _selectedItem = null;
        private List<FeedEntry> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        List<FeedEntry> _localFeedEntries = new();

        protected override async Task OnInitializedAsync()
        {
            var result = await SignalRClient.Instance.GetFeedEntries(0, 1000);
            _localFeedEntries = result.FeedEntries;
            await _table.Refresh();
            StateHasChanged();
        }

        async Task<(int, IEnumerable<FeedEntry>)> GetItemsLocally(DataProviderRequest request)
        {
            if (_localFeedEntries == null)
                return (0, new List<FeedEntry>());

            await Task.CompletedTask;
            return (_localFeedEntries.Count, _localFeedEntries.Skip(request.StartIndex).Take(request.Count));
        }

        private async Task<(int, IEnumerable<FeedEntry>)> GetItemsFromDatabase(DataProviderRequest request)
        {
            //if (_addDelay)
            //    await Task.Delay(1000, request.CancellationToken);

            FeedEntryResult feedEntries = await SignalRClient.Instance.GetFeedEntries(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (feedEntries.TotalNumEntries, feedEntries.FeedEntries);
        }

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
            var count = _localFeedEntries.Count();
            _localFeedEntries.Add(FeedEntry.GetNewFeed(count));
            await _table.Refresh();
        }
        async Task OnAddNewItemGotoEndIfAtEnd()
        {
            var atEnd = await _table.AtEnd();
            var count = _localFeedEntries.Count();
            _localFeedEntries.Add(FeedEntry.GetNewFeed(count));
            await _table.Refresh();
            if (atEnd)
                await _table.GotoEnd();
        }

        void ChangeItem()
        {
            _localFeedEntries[0].Title = "Bla bla bla";
            _table.Refresh(_localFeedEntries[0]);
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
    }
}