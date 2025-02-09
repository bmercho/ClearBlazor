using ClearBlazor;
using TestData;

namespace Data
{
    public class TestTreeRow :TreeItem<TestTreeRow>, IEquatable<TestTreeRow>
    {
        public string NodeId { get; set; } = string.Empty;

        public int ImageId { get; set; }

        public string IconName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;

        public bool Available { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string Notes { get; set; } = string.Empty;

        public bool Equals(TestTreeRow? other)
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

        public static TestTreeRow GetNewTestTreeRow(string nodeId)
        {
            return new TestTreeRow()
            {
                NodeId = nodeId,
                ImageId = RandomTestData.GetRandomInt(1,80),
                IconName = RandomTestData.GetIconName(),
                FirstName = RandomTestData.GetRandomFirstName(),
                LastName = RandomTestData.GetRandomSurname(),
                Product = RandomTestData.GetRandomProduct(),
                Available = RandomTestData.GetRandomAvailable(),
                Quantity = RandomTestData.GetRandomQuantity(),
                UnitPrice = (decimal)RandomTestData.GetRandomPrice(),
                Notes = RandomTestData.GetRandomText()
            };
        }
    }
}
