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

        // The ItemIndex in the full item list.
        // The ItemIndexes are set when GetItems is called. 
        // If items are added then the indexes are re-calculated which means that a particular
        // item is not associated with a given index. Use ListItemId to identify a particular item.
        [NotMapped]
        public int ItemIndex { get; set; }

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
