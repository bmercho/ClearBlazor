namespace Data
{
    public class TreeEntryResult
    {
        public int TotalNumEntries { get; set; }
        public int FirstIndex { get; set; }
        public List<TestTreeRowFlat> TreeRows { get; set; } = new();
    }
}
