using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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

// Register Entity Framework Core with PostgreSQL provider
// This configures the ApplicationDbContext to use the specified connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Enable detailed error pages for database operations during development
// Shows helpful information about database-related exceptions
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register ASP.NET Identity for user authentication
// Email confirmation is disabled for development (set to false)
// In production, implement a proper email service and set this to true
// Uses ApplicationDbContext for storing user data and roles
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure authentication cookies for cross-origin requests (Next.js frontend)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // Allow cross-origin
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
    options.Cookie.HttpOnly = true; // Prevent XSS attacks
    options.Events.OnRedirectToLogin = context =>
    {
        // For API requests, return 401 instead of redirecting to login page
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// Register MVC controllers and Razor views for server-side rendering
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Register HTTP clients for external API services
// These will be injected into controller constructors for dependency injection
// WarframeStatApiService: Handles API calls to https://api.warframestat.us/pc
builder.Services.AddHttpClient<WarframeStatApiService>();

// WarframeMarketApiService: Handles API calls to https://api.warframe.market/v1/
// IMPORTANT: The Warframe Market API requires proper headers to prevent 403 Forbidden errors
// Without User-Agent and Accept headers, the API will reject requests as potential automated abuse
builder.Services.AddHttpClient<WarframeMarketApiService>()
    .ConfigureHttpClient(client =>
    {
        // Set a descriptive User-Agent so the API knows this is a legitimate application
        // Format: AppName/Version (Platform; +Website)
        // This helps API providers identify and support the application
        client.DefaultRequestHeaders.Add("User-Agent", 
            "WarframeUtils/1.0 (ASP.NET Core 8.0; Windows; +https://github.com/)");
        
        // Add Accept header to indicate we expect JSON responses
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

// Register the background service for checking price alerts
// This service runs continuously in the background and checks active alerts every 5 minutes
builder.Services.AddHostedService<PriceAlertCheckService>();

// Register email sender service (development version - logs to console)
// For production, replace with a real email service implementation
builder.Services.AddTransient<IEmailSender, DevEmailSender>();

// Add CORS policy for Next.js frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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

// Enable CORS for Next.js frontend
app.UseCors("AllowNextJs");

// Enable routing - must be called before MapControllerRoute and MapRazorPages
app.UseRouting();

// Enable authentication - MUST come before authorization
// This processes authentication cookies and sets User identity
app.UseAuthentication();

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
