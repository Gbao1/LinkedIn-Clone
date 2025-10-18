using Microsoft.AspNetCore.Mvc;
using System;
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
                CurrentUser = new UserSummary { FullName = "Brandon", Headline = "Systems Engineer + Networking" },
                Suggestions = new List<UserSummary>
{
new UserSummary { FullName = "Jian", Headline = "Systems Engineer" },
new UserSummary { FullName = "Bao", Headline = "Data Scientist" },
new UserSummary { FullName = "Emelee", Headline = "Data Scientist" }
},
                FeedItems = new List<FeedItem>
{
new FeedItem { Id = 1, Author = "Jian", AuthorTitle = "Systems Engineer", TimeAgo = "2h", Text = "Dev Ops is cool" },
new FeedItem { Id = 2, Author = "Bao", AuthorTitle = "Data Scientist", TimeAgo = "6h", Text = "sup guys" },
new FeedItem { Id = 3, Author = "Emelee", AuthorTitle = "Data Scientist", TimeAgo = "1d", Text = "LinkedIn isn't as good or something." }
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