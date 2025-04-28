using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warframe_Utils_.NET.Models;
using Warframe_Utils_.NET.Services;

namespace Warframe_Utils_.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WarframeStatApiService warframeStatApiService;

        public HomeController(ILogger<HomeController> logger,WarframeStatApiService warframeApiService)
        {
            _logger = logger;
            warframeStatApiService = warframeApiService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // Fetch Warframe status data
            var status = await warframeStatApiService.GetWarframeStatusAsync();
            return View(status);
        }

        [Authorize]
        [Route("/About")]
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
