namespace ClearBlazor
{ 
    public abstract class ListItem:IEquatable<ListItem>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsHighlighted { get; set; }
        public bool IsSelected { get; set; }
        internal int Index { get; set; }

        public bool Equals(ListItem? other)
        {
            if (other == null) 
                return false;
            if (other.Id == Id) 
                return true;
            return false;
        }
    }
}
