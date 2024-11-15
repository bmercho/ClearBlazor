using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClearBlazor
{
    public abstract class ListItem : IEquatable<ListItem>
    {
        [Key]
        public Guid ListItemId { get; set; } = Guid.NewGuid();

        [NotMapped]
        public bool IsSelected { get; set; }

        [NotMapped]
        public int Index { get; set; }

        public bool Equals(ListItem? other)
        {
            if (other == null)
                return false;
            if (other.ListItemId == ListItemId)
                return true;
            return false;
        }
    }
}
