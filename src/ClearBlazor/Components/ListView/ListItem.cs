using System.ComponentModel.DataAnnotations;

namespace ClearBlazor
{ 
    public abstract class ListItem:IEquatable<ListItem>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public bool IsSelected { get; set; }
        public int Index { get; set; }

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
