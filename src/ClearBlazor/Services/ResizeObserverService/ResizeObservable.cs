namespace ClearBlazor
{
    public class ResizeObservable
    {
        public string Id { get; set; }
        public IEnumerable<string> ElementIds { get; set; }

        public ResizeObservable(string id, IEnumerable<string> elementIds)
        {
            Id = id;
            ElementIds = elementIds;
        }
    }
}
