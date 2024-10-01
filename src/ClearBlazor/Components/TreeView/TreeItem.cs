namespace ClearBlazor
{
    public class TreeItem<TItem>
    {
        public List<TItem> Children { get; set; } = new List<TItem>();
        public bool HasChildren => Children.Count > 0;
        public bool Expanded { get; set; }

        internal bool Selected { get; set; }

        internal TItem? Parent { get; set; }

        internal bool IsVisible { get; set; }
        internal int Level { get; set; }
    }
}
