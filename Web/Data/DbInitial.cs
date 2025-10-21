using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.Migrate(); // ensures DB and tables are created

            // Seed UserProfiles
            if (!context.UserProfiles.Any())
            {
                context.UserProfiles.AddRange(
                    new UserProfile { UserId = "1", FullName = "Brandon", Headline = "Systems Engineer + Networking" },
                    new UserProfile { UserId = "2", FullName = "Jian", Headline = "Systems Engineer" },
                    new UserProfile { UserId = "3", FullName = "Bao", Headline = "Data Scientist" },
                    new UserProfile { UserId = "4", FullName = "Emelee", Headline = "Data Scientist" }
                );
                context.SaveChanges();
            }

            // Seed FeedItems
            if (!context.FeedItems.Any())
            {
                context.FeedItems.AddRange(
                    new FeedItem { UserId = "2", Author = "Jian", AuthorTitle = "Systems Engineer", Text = "Hello", AvatarUrl = "/images/profilepictures/img2.webp", TimeAgo = "2h" },
                    new FeedItem { UserId = "3", Author = "Bao", AuthorTitle = "Data Scientist", Text = "Sup guys", AvatarUrl = "/images/profilepictures/img3.jpg", TimeAgo = "6h" },
                    new FeedItem { UserId = "4", Author = "Emelee", AuthorTitle = "Data Scientist", Text = "Hey Guys.", AvatarUrl = "/images/profilepictures/img2.webp", TimeAgo = "1d" }
                );
                context.SaveChanges();
            }

            // Seed Likes (optional)
            if (!context.Likes.Any())
            {
                context.Likes.AddRange(
                    new Like { FeedItemId = 1, UserId = "2", UserName = "Jian" },
                    new Like { FeedItemId = 1, UserId = "3", UserName = "Bao" }
                );
                context.SaveChanges();
            }
        }
    }
}
