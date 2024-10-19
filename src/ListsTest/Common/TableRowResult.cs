namespace ListsTest
{
    public class TableRowResult
    {
        public int TotalNumEntries { get; set; }
        public int FirstIndex { get; set; }
        public List<TableRow> TableRows { get; set; } = new();
    }
}
