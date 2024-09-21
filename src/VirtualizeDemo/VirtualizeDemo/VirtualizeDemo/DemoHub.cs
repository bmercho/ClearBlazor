using Microsoft.AspNetCore.SignalR;

namespace VirtualizeDemo
{
    public class DemoHub : Hub
    {
        public static DatabaseManager? DatabaseManager { private get; set; }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task<FeedEntryResult> GetFeedEntries(int startIndex, int count)
        {
            if (DatabaseManager != null)
                    return await DatabaseManager.GetFeeds(startIndex, count);
                return new FeedEntryResult();
        }
    }
}