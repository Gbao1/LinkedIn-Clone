using System.Collections.Generic;


namespace Link
{
    public class HomeIndexViewModel
    {
        public UserSummary CurrentUser { get; set; }
        public List<UserSummary> Suggestions { get; set; }
        public List<FeedItem> FeedItems { get; set; }
    }


    public class UserSummary
    {
        public string FullName { get; set; }
        public string Headline { get; set; }
        public string AvatarUrl { get; set; } = "/images/profilepictures/img2.webp";
    }


    public class FeedItem
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string AuthorTitle { get; set; }
        public string TimeAgo { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; } = 0;
    }
}