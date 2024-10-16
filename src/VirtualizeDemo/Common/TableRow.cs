using ClearBlazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VirtualizeDemo
{
    [Table("TableRow")]
    public class TableRow : ListItem
    {
        [Key]
        public Guid TableRowId { get; set; }

        [Column]
        public int Index { get; set; }

        [Column]
        public string FirstName { get; set; } = string.Empty;

        [Column]
        public string LastName { get; set; } = string.Empty;

        [Column]
        public string Product { get; set; } = string.Empty;

        [Column]
        public bool Available { get; set; }

        [Column]
        public int Quantity { get; set; }

        [Column]
        public decimal UnitPrice { get; set; }

        [Column]
        public string Notes { get; set; } = string.Empty;

        [Column]
        public string NotesVariableHeight { get; set; } = string.Empty;
    }
}
