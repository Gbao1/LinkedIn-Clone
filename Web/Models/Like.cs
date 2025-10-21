using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        // Foreign key
        public int FeedItemId { get; set; }

        // Navigation property
        [ForeignKey("FeedItemId")]
        public virtual FeedItem FeedItem { get; set; }
    }
}
