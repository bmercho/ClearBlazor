using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace VirtualizeDemo
{
    public partial class Home : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        private HubConnection? hubConnection;
        FeedEntryResult _feedEntries = new();
        FeedEntryResult _localFeedEntries = new();
        bool _addDelay = true;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri("/demohub"))
                .Build();

            await base.OnInitializedAsync();
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
            });

            await hubConnection.StartAsync();
            if (hubConnection != null)
            {
                _localFeedEntries =
                   await hubConnection.InvokeAsync<FeedEntryResult>("GetFeedEntries", 0, 500);
                StateHasChanged();
            }
        }

        async Task<IEnumerable<FeedEntry>> GetItemsLocally(ClearBlazor.ItemsProviderRequest request)
        {
            await Task.CompletedTask;
            return _localFeedEntries.FeedEntries.Skip(request.StartIndex).Take(request.Count);
        }

        async Task<IEnumerable<FeedEntry>> GetItemsFromDatabase(ClearBlazor.ItemsProviderRequest request)
        {
            if (hubConnection == null)
                return new List<FeedEntry>();

            if (_addDelay)
                await Task.Delay(1000);

            _feedEntries =
                  await hubConnection.InvokeAsync<FeedEntryResult>("GetFeedEntries", request.StartIndex, request.Count);
            return _feedEntries.FeedEntries;
        }

        private void Refresh()
        {
            StateHasChanged();
        }
    }
}