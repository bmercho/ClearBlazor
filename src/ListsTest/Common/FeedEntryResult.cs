namespace ListsTest
{
    public class FeedEntryResult
    {
        public int TotalNumEntries { get; set; }
        public int FirstIndex { get; set; }
        public List<FeedEntry> FeedEntries { get; set; } = new();
    }
}
