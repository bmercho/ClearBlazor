namespace ClearBlazor
{
    public class TreeItem<TItem>
    {
        public Guid NodeId { get; set; } = Guid.NewGuid();
        public List<TItem> Children { get; set; } = new List<TItem>();
        public bool HasChildren => Children.Count > 0;
        internal TItem? Parent { get; set; }
        public bool IsExpanded { get; set; }
        internal bool IsSelected { get; set; }
        internal bool IsVisible { get; set; }
        internal int Level { get; set; }
    }
}
