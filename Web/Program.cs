using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);

// --- Data & Identity ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Use SQL Server if the connection string looks like an Azure SQL/SQL Server string; otherwise use SQLite.
    if (connectionString.Contains("Initial Catalog", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains(".database.windows.net", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Data Source=tcp:", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        // (optional) tweak password requirements here if desired
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
// (optional) If you’re using Application Insights:
// builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// --- Error handling / security ---
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();            // .NET 8 way to serve wwwroot

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// --- Endpoints ---
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();

// --- Auto-apply EF Core migrations on startup (Level 3 · Step 3 Option A) ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // idempotent: applies pending migrations or no-ops
}

app.Run();
