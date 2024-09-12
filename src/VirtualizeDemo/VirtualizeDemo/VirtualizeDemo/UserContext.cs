using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VirtualizeDemo
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Column]
        public string UserName { get; set; } = null!;
    }
}
