using Microsoft.AspNetCore.SignalR;
using Data;

namespace ListsTest
{
    public class DemoHub : Hub
    {
        public static DatabaseManager? DatabaseManager { private get; set; }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task<ListEntryResult> GetListRows(int startIndex, int count)
        {
            if (DatabaseManager != null)
                return await DatabaseManager.GetListRows(startIndex, count);
            return new ListEntryResult();
        }
        public async Task<TreeEntryResult> GetTreeRows(int startIndex, int count)
        {
            if (DatabaseManager != null)
                return await DatabaseManager.GetTreeRows(startIndex, count);
            return new TreeEntryResult();
        }
    }
}