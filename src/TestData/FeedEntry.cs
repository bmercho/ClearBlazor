using ClearBlazor;
using LoremNET;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestData
{
    [Table("FeedEntry")]
    public class FeedEntry: ListItem, IEquatable<FeedEntry>  
    {
        [Column]
        public int ImageId { get; set; }

        [Column]
        public string IconName { get; set; } = string.Empty;

        [Column]
        public string? Title { get; set; }

        [Column]
        public string? Message { get; set; }

        public bool Equals(FeedEntry? other)
        {
            if (other == null)
                return false;
            if (other.Id == Id)
                return true;
            return false;
        }

        public override string ToString()
        {
            return Title!.Replace("Message", "");
        }

        public static FeedEntry GetNewFeed(int index)
        {
            return new FeedEntry()
            {
                Title = $"Message{index}",
                Message = Lorem.Words(2, 40),
                ImageId = TestData.GetRandomInt(1, 80),
                IconName = TestData.GetIconName(),
            };
        }

    }
}
