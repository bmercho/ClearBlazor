using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualizeDemo
{
    [Table("FeedEntry")]
    public class FeedEntry
    {
        [Key]
        public Guid FeedEntryId { get; set; }

        [Column]
        public FeedEntryType EntryType { get; set; }

        [Column]
        public long ElementId { get; set; }

        [Column]
        public string? Title { get; set; }

        [Column]
        public string? Message { get; set; }

        [Column]
        public Guid PosterUserId { get; set; }

        [Column]
        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return Title!.Replace("Message", "");
        }
    }
}
