using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warframe_Utils_.NET.Data;
using Warframe_Utils_.NET.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// DEPENDENCY INJECTION & SERVICE CONFIGURATION
// ============================================================================

// Retrieve the database connection string from appsettings.json
// Throws an exception if the connection string is not found (prevents silent failures)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register Entity Framework Core with SQL Server provider
// This configures the ApplicationDbContext to use the specified connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Enable detailed error pages for database operations during development
// Shows helpful information about database-related exceptions
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register ASP.NET Identity for user authentication
// Requires email confirmation before users can sign in
// Uses ApplicationDbContext for storing user data and roles
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register MVC controllers and Razor views for server-side rendering
builder.Services.AddControllersWithViews();

// Register HTTP clients for external API services
// These will be injected into controller constructors for dependency injection
// WarframeStatApiService: Handles API calls to https://api.warframestat.us/pc
builder.Services.AddHttpClient<WarframeStatApiService>();

// WarframeMarketApiService: Handles API calls to https://api.warframe.market/v1/
builder.Services.AddHttpClient<WarframeMarketApiService>();

// ============================================================================
// BUILD & CONFIGURE HTTP PIPELINE
// ============================================================================

var app = builder.Build();

// Configure the HTTP request pipeline
// Different configurations apply based on the environment (Development vs Production)
if (app.Environment.IsDevelopment())
{
    // In development, show migrations endpoint for running database migrations
    // This provides a UI to apply pending migrations
    app.UseMigrationsEndPoint();
}
else
{
    // In production, use centralized error handling
    // Routes all unhandled exceptions to /Home/Error
    app.UseExceptionHandler("/Home/Error");
    
    // Enable HSTS (HTTP Strict Transport Security)
    // Default is 30 days - informs browsers to only access via HTTPS
    // See https://aka.ms/aspnetcore-hsts for production recommendations
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS (secure connections)
app.UseHttpsRedirection();

// Enable serving static files from wwwroot directory (CSS, JavaScript, images, etc.)
app.UseStaticFiles();

// Enable routing - must be called before MapControllerRoute and MapRazorPages
app.UseRouting();

// Enable authorization - checks if users have permission to access resources
app.UseAuthorization();

// ============================================================================
// ROUTE CONFIGURATION
// ============================================================================

// Default MVC route for controller-based requests
// Pattern: {controller}/{action}/{id?}
// Defaults to HomeController, Index action if not specified
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages for Identity authentication pages
// Handles login, register, password reset, etc.
app.MapRazorPages();

// Start listening for HTTP requests
app.Run();
