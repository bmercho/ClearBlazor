using ClearBlazor;
using System.ComponentModel.DataAnnotations.Schema;
using LoremNET;

namespace ListsTest
{
    [Table("TableRow")]
    public class TableRow :ListItem, IEquatable<TableRow>
    {
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

        public bool Equals(TableRow? other)
        {
            if (other == null)
                return false;
            if (other.Id == Id)
                return true;
            return false;
        }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static TableRow GetNewTableRow(int index)
        {
            return new TableRow()
            {
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
