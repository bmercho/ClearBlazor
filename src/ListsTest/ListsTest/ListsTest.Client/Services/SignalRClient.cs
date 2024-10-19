using ClearBlazor;
using Microsoft.AspNetCore.SignalR.Client;

namespace ListsTest
{
    public class SignalRClient
    {
        public static SignalRClient Instance { get; private set; } = null!;

        public HubConnection? hubConnection;
        private string _url = string.Empty;

        public SignalRClient()
        {
            Instance = this;
        }

        public async Task Initialise(string baseAddress)
        {
            if (baseAddress.EndsWith('/'))
                _url = $"{baseAddress}demohub";
            else
                _url = $"{baseAddress}/demohub";

            hubConnection = new HubConnectionBuilder()
                                .WithUrl(_url)
                                .Build();

            hubConnection.Reconnected += HubConnection_Reconnected;
            hubConnection.Closed += HubConnection_Closed;

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
            });

            await hubConnection.StartAsync();
        }

        private async Task HubConnection_Closed(Exception? arg)
        {
        }

        private async Task HubConnection_Reconnected(string? arg)
        {
        }

        public async Task<FeedEntryResult> GetFeedEntries(int startIndex, int num, 
                                                          CancellationToken? cancellationToken = null)
        {
            if (hubConnection == null)
                return new ();

            return await hubConnection.InvokeAsync<FeedEntryResult>("GetFeedEntries", startIndex, num,
                                                                    new CancellationToken());

        }
        public async Task<TableRowResult> GetTableRows(int startIndex, int num,
                                                       CancellationToken? cancellationToken = null)
        {
            if (hubConnection == null)
                return new();

            return await hubConnection.InvokeAsync<TableRowResult>("GetTableRows", startIndex, num,
                                                                    new CancellationToken());

        }
    }
}
