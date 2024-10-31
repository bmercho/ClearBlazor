using System.ComponentModel.DataAnnotations.Schema;
using LoremNET;

namespace TestData
{
    [Table("TreeRow")]
    public class TreeRow: IEquatable<TreeRow>
    {
        [Column]
        public string LevelId { get; set; } = string.Empty;

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

        public virtual bool Equals(TreeRow? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static TreeRow GetNewTreeRow(string levelId)
        {
            return new TreeRow()
            {
                LevelId = levelId,

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
