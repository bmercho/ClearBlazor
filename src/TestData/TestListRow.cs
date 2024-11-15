using ClearBlazor;
using System.ComponentModel.DataAnnotations.Schema;
using TestData;

namespace Data
{
    [Table("TestListRow")]
    public class TestListRow :ListItem, IEquatable<TestListRow>
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

        public bool Equals(TestListRow? other)
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

        public static TestListRow GetNewTestListRow(int index)
        {
            return new TestListRow()
            {
                Index = index,
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
