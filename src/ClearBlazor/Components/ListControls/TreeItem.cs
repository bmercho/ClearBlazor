namespace ClearBlazor
{
    public class TreeItem<TItem>:ListItem
    {
        public List<TItem> Children { get; set; } = new List<TItem>();
        public bool HasChildren => Children.Count > 0;
        public TItem? Parent { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsVisible { get; set; }
        public int Level { get; set; }
    }
}
