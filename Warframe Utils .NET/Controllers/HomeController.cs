using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warframe_Utils_.NET.Models;
using Warframe_Utils_.NET.Models.DTOS;
using Warframe_Utils_.NET.Models.ViewModels;
using Warframe_Utils_.NET.Services;
using static Warframe_Utils_.NET.Models.DTOS.ModsResponse;

namespace Warframe_Utils_.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WarframeStatApiService warframeStatApiService;
        private readonly WarframeMarketApiService warframeMarketApiService;
        public HomeController(ILogger<HomeController> logger,WarframeStatApiService warframeApiService, WarframeMarketApiService marketApiService)
        {
            _logger = logger;
            warframeStatApiService = warframeApiService;
            warframeMarketApiService = marketApiService;
        }

        public async Task<IActionResult> IndexAsync(string? modName)
        {
            var status = await warframeStatApiService.GetWarframeStatusAsync();
            var modsResponse = await warframeMarketApiService.GetAllModsAsync();

            Mod? modFound = null;
            OrdersResponse? orders = null;
            ModDetailResponse? modDetail = null;

            if (!string.IsNullOrWhiteSpace(modName))
            {
                modFound = modsResponse.Payload.Mods
                    .FirstOrDefault(m => m.ItemName.Equals(modName, StringComparison.OrdinalIgnoreCase));

                if (modFound != null)
                {
                    try
                    {
                        orders = await warframeMarketApiService.GetAllOrdersAsync(modFound.UrlName);
                        modDetail = await warframeMarketApiService.GetItemAsync(modFound.UrlName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            var viewModel = new WarframeHomeViewModel
            {
                Status = status,
                Mods = modsResponse,
                ModFound = modFound,
                Orders = orders,
                ModDetail = modDetail
            };

            return View(viewModel);
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
