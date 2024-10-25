using ClearBlazor;
using TestData;

namespace ListsTest
{
    public partial class ListViewNonVirtualizedTest
    {
        ListView<FeedEntry> _list = null!;
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
            var result = await SignalRClient.Instance.GetFeedEntries(0, 100);
            _localFeedEntries = result.FeedEntries;
            await _list.Refresh();
            StateHasChanged();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        async Task<(int, IEnumerable<FeedEntry>)> GetItemsLocally(DataProviderRequest request)
        {
            if (_localFeedEntries == null)
                return (0, new List<FeedEntry>());

            await Task.CompletedTask;
            return (_localFeedEntries.Count, _localFeedEntries.Skip(request.StartIndex).Take(request.Count));
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
        }

        async Task OnGotoStart()
        {
            await _list.GotoStart();
        }

        async Task OnAddNewItem()
        {
            var count = _localFeedEntries.Count;
            _localFeedEntries.Add(FeedEntry.GetNewFeed(count));
            await _list.Refresh();
        }
        async Task OnAddNewItemGotoEndIfAtEnd()
        {
            var atEnd = await _list.AtEnd();
            var count = _localFeedEntries.Count;
            _localFeedEntries.Add(FeedEntry.GetNewFeed(count));
            await _list.Refresh();
            if (atEnd)
                await _list.GotoEnd();
        }

        void ChangeItem()
        {
            _localFeedEntries[0].Title = "Bla bla bla";
            _list.Refresh(_localFeedEntries[0]);
        }

        private async Task ClearSelections()
        {
            await _list.RemoveAllSelections();
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