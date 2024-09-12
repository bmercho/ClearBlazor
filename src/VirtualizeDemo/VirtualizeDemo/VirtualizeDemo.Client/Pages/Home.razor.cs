using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace VirtualizeDemo
{
    public partial class Home:ComponentBase
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

        private async Task OnClick()
        {
            if (hubConnection != null)
            {
                _feedEntries =
                   await hubConnection.InvokeAsync<FeedEntryResult>("GetFeedEntries", 1, 20);
                StateHasChanged();
            }
        }

    }
}