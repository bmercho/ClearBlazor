using ClearBlazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LoremNET;

namespace ListsTest
{
    [Table("TableRow")]
    public class TableRow : ListItem
    {
        [Key]
        public Guid TableRowId { get; set; }

        [Column]
        public int Index { get; set; }

        [Column]
        public int ImageId { get; set; }

        [Column]
        public string IconName { get; set; } = string.Empty;

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

        public static TableRow GetNewTableRow(int index)
        {
            return new TableRow()
            {
                TableRowId = new Guid(),
                Index = index,
                ImageId = TestData.GetRandomInt(1,80),
                IconName = TestData.GetIconName(),
                FirstName = TestData.GetRandomFirstName(),
                LastName = TestData.GetRandomSurname(),
                Product = TestData.GetRandomProduct(),
                Available = TestData.GetRandomAvailable(),
                Quantity = TestData.GetRandomQuantity(),
                UnitPrice = (decimal)TestData.GetRandomPrice(),
                Notes = Lorem.Words(2, 40)
            };
        }
    }
}
