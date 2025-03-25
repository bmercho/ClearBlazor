namespace ClearBlazor
{
    public static class Logger
    {
        static List<LogItem> Messages = new List<LogItem>();
        static int Count = 0;

        public static void AddLog(string message)
        {
            Messages.Add(new LogItem()
            {
                Message = $"{++Count} : {DateTime.Now.ToString("dd MMM yy HH:mm:ss")} : {message}"
            });
        }

        public static List<LogItem> GetMessages()
        {
            return Messages;
        }
    }
    public class LogItem : ListItem
    {
        public string Message { get; set; } = string.Empty;
    }
}
