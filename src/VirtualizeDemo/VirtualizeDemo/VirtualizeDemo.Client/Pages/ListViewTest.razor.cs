using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace VirtualizeDemo
{
    public partial class ListViewTest
        : ComponentBase
    {
        FeedEntryResult? _localFeedEntries = new();
        bool _addDelay = false;



        protected override async Task OnInitializedAsync()
        {
            _localFeedEntries = await SignalRClient.Instance.GetFeedEntries(0, 500);
            StateHasChanged();
        }

        async Task<(int, IEnumerable<FeedEntry>)> GetItemsLocally(ClearBlazor.DataProviderRequest request)
        {
            if (_localFeedEntries == null) 
                return (0, new List<FeedEntry>());

            await Task.CompletedTask;
            return (0, _localFeedEntries.FeedEntries.Skip(request.StartIndex).Take(request.Count));
        }

        async Task<(int, IEnumerable<FeedEntry>?)> GetItemsFromDatabase(ClearBlazor.DataProviderRequest request)
        {
            if (_addDelay)
                await Task.Delay(1000, request.CancellationToken);

            FeedEntryResult feedEntries = await SignalRClient.Instance.GetFeedEntries(
                                                              request.StartIndex, request.Count,
                                                              request.CancellationToken);
            return (0, feedEntries.FeedEntries);
        }

        private void Refresh()
        {
            StateHasChanged();
        }
    }
}