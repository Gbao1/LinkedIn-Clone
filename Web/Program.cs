using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 🔧 Use SQL Server in prod (Azure) when the connection string looks like SQL Server;
//    otherwise fall back to Sqlite for local dev.
//    This lets you keep Sqlite locally and switch to Azure SQL via App Service settings.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Initial Catalog", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains(".database.windows.net", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// ✅ Turn on AI telemetry (reads ApplicationInsights:ConnectionString from config)
builder.Services.AddApplicationInsightsTelemetry();

// (Optional) friendly role name
builder.Services.AddSingleton<ITelemetryInitializer, RoleNameTelemetryInitializer>();

var app = builder.Build();

// 🔐 Ensure authentication middleware is enabled for Identity pages/sign-in.
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// 🔧 Optional: auto-apply EF Core migrations on startup (useful in Azure).
//    If you prefer running migrations from your machine/CI, you can comment this out.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();

// --------------------------------------------------------
// initializer: tags telemetry with a role name
// --------------------------------------------------------
public sealed class RoleNameTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = "linkedin-web";
    }
}
