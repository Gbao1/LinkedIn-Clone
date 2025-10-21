using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Link.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            // Load feed items including likes
            var feedItems = await _db.FeedItems
                .Include(f => f.Likes)
                .ToListAsync();

            var vm = new HomeIndexViewModel
            {
                CurrentUser = new Profile
                {
                    FullName = User.Identity.Name ?? $"Guest{Guid.NewGuid().ToString("N").Substring(0, 12)}",
                    Headline = "-",
                    AvatarUrl = "/images/profilepictures/img2.webp"
                },
                Suggestions = await _db.UserProfiles
                    .Select(u => new UserProfile
                    {
                        FullName = u.FullName,
                        Headline = u.Headline,
                        AvatarUrl = u.AvatarUrl
                    })
                    .ToListAsync(),
                FeedItems = feedItems
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return RedirectToAction("Index");

            var post = new FeedItem
            {
                Author = User.Identity.Name,
                AuthorTitle = "Developer", // optionally pull from Profile
                Text = text,
                TimeAgo = "Just now",
                AvatarUrl = "/images/profilepictures/img2.webp"
            };

            _db.FeedItems.Add(post);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> LikePost(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var post = await _db.FeedItems
                .Include(f => f.Likes)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (post == null) return NotFound();

            // Avoid duplicate likes by same user
            if (!post.Likes.Any(l => l.UserName == User.Identity.Name))
            {
                post.Likes.Add(new Like
                {
                    UserName = User.Identity.Name
                });

                await _db.SaveChangesAsync();
            }

            // Return updated likes
            var likes = post.Likes.Select(l => l.UserName).ToList();
            return Json(likes);
        }


    }
}
