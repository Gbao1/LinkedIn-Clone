
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class FeedItem
    {
        public int Id { get; set; }

        // link to Identity user
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string Author { get; set; }

        public string AuthorTitle { get; set; }

        public string Text { get; set; }

        public string TimeAgo { get; set; }

        public string AvatarUrl { get; set; } = "/images/profilepictures/img2.webp";

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}