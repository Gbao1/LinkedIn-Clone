using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) REQUIRED for MapRazorPages()
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString.Contains("Initial Catalog", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains(".database.windows.net", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase))
        options.UseSqlServer(connectionString);
    else
        options.UseSqlite(connectionString);
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(o => o.SignIn.RequireConfirmedAccount = false) //disable email confirmation now
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// 2) Make auto-migrate non-fatal (so startup doesn’t crash)
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
        logger.LogInformation("EF migrations applied or already up to date.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "EF migration failed on startup (continuing so app can start).");
        // Do NOT throw; app should still run so you can see the error page/logs.
    }
}

app.Run();
