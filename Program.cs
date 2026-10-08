using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoxisAuthApp.Data;
using NeoxisAuthApp.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Database — reads from environment variable DATABASE_URL (set in Render)
// Falls back to appsettings.json for local development.
// ---------------------------------------------------------------------------
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No database connection string found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ---------------------------------------------------------------------------
// ASP.NET Core Identity
// ---------------------------------------------------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ---------------------------------------------------------------------------
// External OAuth providers — credentials are read from environment variables.
// The app starts fine without them; buttons simply won't work until the env
// vars are configured in Render (or local user secrets).
// ---------------------------------------------------------------------------
var googleClientId     = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
var msClientId         = Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID");
var msClientSecret     = Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_SECRET");

// AddIdentity already calls AddAuthentication internally; we just chain onto it.
if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
{
    builder.Services.AddAuthentication().AddGoogle(options =>
    {
        options.ClientId     = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/signin-google";    // matches redirect URI in Google Console
    });
}

if (!string.IsNullOrEmpty(msClientId) && !string.IsNullOrEmpty(msClientSecret))
{
    builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
    {
        options.ClientId     = msClientId;
        options.ClientSecret = msClientSecret;
        options.CallbackPath = "/signin-microsoft"; // matches redirect URI in Azure
    });
}

// ---------------------------------------------------------------------------
// Cookie configuration
// ---------------------------------------------------------------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath        = "/Account/Login";
    options.LogoutPath       = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly  = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ---------------------------------------------------------------------------
// Auto-apply pending migrations on startup (safe for cloud deployments)
// ---------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// ---------------------------------------------------------------------------
// Middleware pipeline
// ---------------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configure Forwarded Headers for reverse proxies like Render
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
