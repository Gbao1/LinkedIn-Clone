using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Link.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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
                CurrentUser = new UserProfile
                {
                    FullName = User.Identity.Name ?? "Guest",
                    Headline = "Developer",
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Check if user already liked the post
            var existingLike = await _db.Likes
                .FirstOrDefaultAsync(l => l.FeedItemId == id && l.UserId == user.Id);

            if (existingLike == null)
            {
                // Add new like
                var like = new Like
                {
                    FeedItemId = id,
                    UserId = user.Id,
                    UserName = user.UserName
                };
                _db.Likes.Add(like);
                await _db.SaveChangesAsync();
            }
            else
            {
                // Unlike
                _db.Likes.Remove(existingLike);
                await _db.SaveChangesAsync();
            }

            // Get all likes for this post
            var likeUsernames = await _db.Likes
                .Where(l => l.FeedItemId == id)
                .Select(l => l.UserName)
                .ToListAsync();

            // Format the string
            string formatted;
            if (likeUsernames.Count == 0)
            {
                formatted = "";
            }
            else if (likeUsernames.Count <= 3)
            {
                formatted = $"{string.Join(", ", likeUsernames)} liked this";
            }
            else
            {
                var firstThree = likeUsernames.Take(3);
                formatted = $"{string.Join(", ", firstThree)} +{likeUsernames.Count - 3} more liked this";
            }

            return Json(new { formatted });
        }




    }
}
