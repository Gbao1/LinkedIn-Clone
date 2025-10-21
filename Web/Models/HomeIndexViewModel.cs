using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models;
namespace Link
{
    public class HomeIndexViewModel
    {
        public UserProfile CurrentUser { get; set; }
        public List<UserProfile> Suggestions { get; set; }
        public List<FeedItem> FeedItems { get; set; }
    }

    public class FeedItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        public string AuthorTitle { get; set; }

        public string Text { get; set; }

        public string TimeAgo { get; set; }

        public string AvatarUrl { get; set; } = "/images/profilepictures/img2.webp";

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }  // who liked

        public int FeedItemId { get; set; }

        public virtual FeedItem FeedItem { get; set; }
    }
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Headline { get; set; }

        public string AvatarUrl { get; set; } = "/images/profilepictures/img2.webp";
    }
}