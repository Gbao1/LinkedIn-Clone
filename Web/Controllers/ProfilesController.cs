using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        // Get form data for sections
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

        // Try to find an existing profile for this user
        var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (profile == null)
        {
            // Create new if not exists
            profile = new Profile
            {
                UserId = user.Id,
                FullName = model.FullName,
                Headline = model.Headline,
                Location = model.Location,
                Summary = model.Summary,
                AvatarUrl = string.IsNullOrWhiteSpace(model.AvatarUrl) ? "" : model.AvatarUrl,
                SectionsJson = JsonSerializer.Serialize(sections),
                UpdatedAt = DateTime.UtcNow
            };

            _db.Profiles.Add(profile);
        }
        else
        {
            // Update existing
            profile.FullName = model.FullName;
            profile.Headline = model.Headline;
            profile.Location = model.Location;
            profile.Summary = model.Summary;
            profile.AvatarUrl = string.IsNullOrWhiteSpace(model.AvatarUrl) ? "" : model.AvatarUrl;
            profile.SectionsJson = JsonSerializer.Serialize(sections);
            profile.UpdatedAt = DateTime.UtcNow;

            _db.Profiles.Update(profile);
        }

        await _db.SaveChangesAsync();

        // Redirect to profile details to confirm save worked
        return RedirectToAction(nameof(Details));
    }
}