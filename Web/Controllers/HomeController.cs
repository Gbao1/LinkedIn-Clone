using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Link.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // sample view model
            var vm = new HomeIndexViewModel
            {
                CurrentUser = new UserProfile
                {
                    FullName = "Brandon",
                    Headline = "Systems Engineer + Networking",
                    AvatarUrl = "/images/profilepictures/img2.webp"
                },
                Suggestions = new List<UserProfile>
                {
                    new UserProfile { FullName = "Jian", Headline = "Systems Engineer", AvatarUrl= "/images/profilepictures/img2.webp" },
                    new UserProfile { FullName = "Bao", Headline = "Data Scientist", AvatarUrl= "/images/profilepictures/img3.jpg" },
                    new UserProfile { FullName = "Emelee", Headline = "Data Scientist", AvatarUrl= "/images/profilepictures/img5.jpg" }
                },
                FeedItems = new List<FeedItem>
                {
                    new FeedItem { Id = 1, Author = "Jian", AuthorTitle = "Systems Engineer", TimeAgo = "2h", Text = "Hello", AvatarUrl="/images/profilepictures/img2.webp" },
                    new FeedItem { Id = 2, Author = "Bao", AuthorTitle = "Data Scientist", TimeAgo = "6h", Text = "Sup guys", AvatarUrl="/images/profilepictures/img3.jpg" },
                    new FeedItem { Id = 3, Author = "Emelee", AuthorTitle = "Data Scientist", TimeAgo = "1d", Text = "Hey Guys.", AvatarUrl="/images/profilepictures/img2.webp" }
                }
            };

            return View(vm);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
