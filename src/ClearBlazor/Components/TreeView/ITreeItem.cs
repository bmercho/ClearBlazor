namespace ClearBlazor
{
    public interface ITreeItem<TItem>
    {
        public List<TItem> Children { get; set; }
        public bool HasChildren { get; set; }
        public bool Expanded { get; set; }
        public bool Selected { get; set; }
    }
}
