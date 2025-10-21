using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Link;

namespace Web.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<FeedItem> FeedItems { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Like> Likes { get; set; }
}
