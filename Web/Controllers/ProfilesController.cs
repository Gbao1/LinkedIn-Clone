using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;
[Authorize]
public class ProfilesController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _um;

    public ProfilesController(ApplicationDbContext db, UserManager<IdentityUser> um)
    {
        _db = db;
        _um = um;
    }

    // GET: /Profiles/Details
    [AllowAnonymous]
    public async Task<IActionResult> Details(string userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            var user = await _um.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");
            userId = user.Id;
        }

        var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return View("EmptyProfile");
        return View(profile);
    }

    // GET: /Profiles/Edit
    public async Task<IActionResult> Edit()
    {
        var user = await _um.GetUserAsync(User);
        if (user == null) return Challenge();

        var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id)
                      ?? new Profile { UserId = user.Id };

        return View(profile);
    }

    // POST: /Profiles/Edit
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Profile model)
    {
        var user = await _um.GetUserAsync(User);
        if (user == null) return Challenge();

        // collect sections from form arrays
        var titles = Request.Form["sectionsTitle"].ToArray();
        var contents = Request.Form["sectionsContent"].ToArray();
        var sections = new List<object>();
        for (int i = 0; i < Math.Min(titles.Length, contents.Length); i++)
        {
            var t = titles[i].Trim();
            var c = contents[i].Trim();
            if (string.IsNullOrEmpty(t) && string.IsNullOrEmpty(c)) continue;
            sections.Add(new { title = t, content = c });
        }

        var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile == null)
        {
            profile = new Profile { UserId = user.Id };
            _db.Profiles.Add(profile);
        }

        // update fields (bind only allowed fields)
        profile.FullName = model.FullName;
        profile.Headline = model.Headline;
        profile.Location = model.Location;
        profile.Summary = model.Summary;

        // ensure AvatarUrl is never null to avoid NOT NULL DB errors
        profile.AvatarUrl = string.IsNullOrWhiteSpace(model.AvatarUrl) ? "" : model.AvatarUrl;

        profile.SectionsJson = JsonSerializer.Serialize(sections);
        profile.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details));
    }
}