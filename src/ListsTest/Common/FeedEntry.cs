using ClearBlazor;
using LoremNET;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListsTest
{
    [Table("FeedEntry")]
    public class FeedEntry:ListItem, IEquatable<FeedEntry>  
    {
        [Key]
        public Guid FeedEntryId { get; set; }

        [Column]
        public int Index { get; set; }

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
            if (other.FeedEntryId == FeedEntryId)
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
                FeedEntryId = new Guid(),
                Index = index,
                Title = $"Message{index}",
                Message = Lorem.Words(2, 40),
                ImageId = TestData.GetRandomInt(1, 80),
                IconName = TestData.GetIconName(),
            };
        }

    }
}
