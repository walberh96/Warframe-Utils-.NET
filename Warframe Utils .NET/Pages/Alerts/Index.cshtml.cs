using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Warframe_Utils_.NET.Pages.Alerts
{
    /// <summary>
    /// Alerts Index Page - Displays price alerts dashboard for logged-in users.
    /// Requires authentication to access.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation($"User {User.Identity?.Name} accessed Alerts page");
        }
    }
}
