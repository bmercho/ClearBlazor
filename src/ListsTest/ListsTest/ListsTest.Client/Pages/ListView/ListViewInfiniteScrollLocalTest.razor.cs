using ClearBlazor;

namespace ListsTest
{
    public partial class ListViewInfiniteScrollLocalTest
    {
        private FeedEntry? _selectedItem = null;
        private List<FeedEntry> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private ListView<FeedEntry> _list = null!;
        private bool _atEnd = false;
        private bool _atStart = true;
        List<FeedEntry> _localFeedEntries = new();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            var result = await SignalRClient.Instance.GetFeedEntries(0, 500);
            _localFeedEntries = result.FeedEntries;
            StateHasChanged();
            await _list.Refresh();
        }

        async Task<(int, IEnumerable<FeedEntry>)> GetItemsLocally(DataProviderRequest request)
        {
            if (_localFeedEntries == null)
                return (0, new List<FeedEntry>());

            await Task.CompletedTask;
            return (_localFeedEntries.Count, _localFeedEntries.Skip(request.StartIndex).Take(request.Count));
        }
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
    }
}