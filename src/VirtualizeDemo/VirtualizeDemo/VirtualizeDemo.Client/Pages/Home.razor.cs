using ClearBlazor;
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
        }

        async Task<IEnumerable<FeedEntry>> GetItems(ClearBlazor.ItemsProviderRequest request)
        {
            if (hubConnection == null)
                return new List<FeedEntry>();

            return _feedEntries.FeedEntries.Skip(request.StartIndex).Take(request.Count);
        }

        //async Task<IEnumerable<FeedEntry>> GetItems(ClearBlazor.ItemsProviderRequest request)
        //{
        //    if (hubConnection == null)
        //        return new List<FeedEntry>();

        //    _feedEntries =
        //          await hubConnection.InvokeAsync<FeedEntryResult>("GetFeedEntries", request.StartIndex, request.Count);
        //    return _feedEntries.FeedEntries;
        //}

        private async Task OnClick()
        {
            if (hubConnection != null)
            {
                _feedEntries =
                   await hubConnection.InvokeAsync<FeedEntryResult>("GetFeedEntries", 0, 500);
                StateHasChanged();
            }
        }
    }
}