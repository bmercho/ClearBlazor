using ClearBlazor;
using TestData;
using Microsoft.AspNetCore.Components;

namespace ListsTest
{
    public partial class ListViewVirtualizeDBTest
        : ComponentBase
    {
        private bool _addDelay = false;
        private ListView<FeedEntry> _list = null!;
        private FeedEntry? _selectedItem = null;
        private List<FeedEntry> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private bool _atEnd = false;
        private bool _atStart = true;

        private async Task<(int, IEnumerable<FeedEntry>)> GetItemsFromDatabase(DataProviderRequest request)
        {
            if (_addDelay)
                await Task.Delay(1000, request.CancellationToken);

            FeedEntryResult feedEntries = await SignalRClient.Instance.GetFeedEntries(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (feedEntries.TotalNumEntries, feedEntries.FeedEntries);
        }

        private void Refresh()
        {
            StateHasChanged();
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