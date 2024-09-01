namespace ClearBlazor
{ 
    public record ItemInfo<TItem>
    {
        public required TItem Item { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsSelected { get; set; }
    }
}
