using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TestData;

namespace Data
{
    [Table("TestTreeRow")]

    public class TestTreeRowFlat
    {
        [Key]
        public Guid ListItemId { get; set; }

        [Column]
        public Guid? ParentId { get; set; }

        [Column]
        public int Index { get; set; }

        [Column]
        public string NodeId { get; set; } = string.Empty;

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


        public static TestTreeRowFlat GetNewTestTreeRow(int parentId, string nodeId)
        {
            return new TestTreeRowFlat()
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
                Notes = RandomTestData.GetRandomText(),
            };
        }
    }
}
