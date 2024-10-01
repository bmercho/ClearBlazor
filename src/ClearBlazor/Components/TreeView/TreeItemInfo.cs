namespace ClearBlazor
{
    public record TreeItemInfo<TItem>
    {
        public TItem? Parent { get; set; }
        public bool HasChildren { get; set; }
        public bool Expanded { get; set; }
        public bool Selected { get; set; }
    }
}
