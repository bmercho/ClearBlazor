using ClearBlazor;
using TestData;

namespace Data
{
    public class TableRow :ListItem, IEquatable<TableRow>
    {
        public int ImageId { get; set; }

        public string IconName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;

        public bool Available { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string Notes { get; set; } = string.Empty;

        public bool Equals(TableRow? other)
        {
            if (other == null)
                return false;
            if (other.ListItemId == ListItemId)
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
                ImageId = RandomTestData.GetRandomInt(1,80),
                IconName = RandomTestData.GetIconName(),
                FirstName = RandomTestData.GetRandomFirstName(),
                LastName = RandomTestData.GetRandomSurname(),
                Product = RandomTestData.GetRandomProduct(),
                Available = RandomTestData.GetRandomAvailable(),
                Quantity = RandomTestData.GetRandomQuantity(),
                UnitPrice = (decimal)RandomTestData.GetRandomPrice(),
                Notes = RandomTestData.GetRandomText(),
            };
        }
    }
}
