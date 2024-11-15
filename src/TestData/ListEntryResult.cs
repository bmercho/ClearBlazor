namespace Data
{
    public class ListEntryResult
    {
        public int TotalNumEntries { get; set; }
        public int FirstIndex { get; set; }
        public List<TestListRow> ListRows { get; set; } = new();
    }
}
