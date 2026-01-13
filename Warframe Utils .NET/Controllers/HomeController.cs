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
    /// <summary>
    /// HomeController handles all user-facing HTTP requests for the main application UI.
    /// Manages mod search, displays game status, and coordinates API service calls.
    /// </summary>
    public class HomeController : Controller
    {
        // Logger for recording application events and debugging information
        private readonly ILogger<HomeController> _logger;

        // Service for fetching game server status (Void Trader, cycles, etc.)
        private readonly WarframeStatApiService _warframeStatApiService;

        // Service for fetching mod market data (items, orders, prices, etc.)
        private readonly WarframeMarketApiService _warframeMarketApiService;

        /// <summary>
        /// Constructor for dependency injection of required services.
        /// The framework automatically provides these when the controller is instantiated.
        /// </summary>
        public HomeController(
            ILogger<HomeController> logger,
            WarframeStatApiService warframeApiService,
            WarframeMarketApiService marketApiService)
        {
            _logger = logger;
            _warframeStatApiService = warframeApiService;
            _warframeMarketApiService = marketApiService;
        }

        /// <summary>
        /// Index action - Main page handler that displays game status and mod search results.
        /// 
        /// Process Flow:
        /// 1. Fetches current game server status (Baroo, cycles, arbitration)
        /// 2. Retrieves all available mods from market for autocomplete suggestions
        /// 3. If a mod name is provided, searches for that specific mod
        /// 4. If mod found, fetches current trading orders and detailed item information
        /// 5. Constructs viewmodel with all data and returns view
        /// </summary>
        /// <param name="modName">Optional: Name of the mod to search for. If null, only displays status and empty search.</param>
        /// <returns>IActionResult - The Index view with WarframeHomeViewModel containing all data</returns>
        public async Task<IActionResult> IndexAsync(string? modName)
        {
            try
            {
                // Fetch game status (Void Trader active, cycle times, etc.)
                var status = await _warframeStatApiService.GetWarframeStatusAsync();

                // Fetch all available mods for the autocomplete dropdown
                var modsResponse = await _warframeMarketApiService.GetAllModsAsync();

                // Variables to hold search results (remain null if no search is performed)
                Mod? modFound = null;
                OrdersResponse? orders = null;
                ModDetailResponse? modDetail = null;

                // If user entered a search term, process it
                if (!string.IsNullOrWhiteSpace(modName))
                {
                    // Search for the mod in the list (case-insensitive comparison)
                    // FirstOrDefault returns null if no match is found
                    modFound = modsResponse.Payload.Mods
                        .FirstOrDefault(m => m.ItemName.Equals(modName, StringComparison.OrdinalIgnoreCase));

                    // If the mod was found, fetch additional data about it
                    if (modFound != null)
                    {
                        try
                        {
                            // Fetch trading orders for this mod (shows buy/sell prices)
                            orders = await _warframeMarketApiService.GetAllOrdersAsync(modFound.UrlName);

                            // Fetch detailed item information (description, rarity, trading tax, wiki link)
                            modDetail = await _warframeMarketApiService.GetItemAsync(modFound.UrlName);
                        }
                        catch (Exception ex)
                        {
                            // Log the error for debugging but don't crash the page
                            // User will see the mod found but with empty orders/details
                            _logger.LogError(ex, "Error fetching orders or mod details for {ModName}", modFound.ItemName);
                            orders = null;
                            modDetail = null;
                        }
                    }
                }

                // Create the viewmodel with all gathered data
                var viewModel = new WarframeHomeViewModel
                {
                    Status = status,                    // Game server status
                    Mods = modsResponse,               // All mods for autocomplete
                    ModFound = modFound!,              // The searched mod (null if not found)
                    Orders = orders,                   // Trading orders for the mod
                    ModDetail = modDetail              // Detailed mod information
                };

                // Return the Index view with the populated viewmodel
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log critical errors and display error page
                _logger.LogError(ex, "An error occurred while loading the home page");
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Error action - Displays error information when an unhandled exception occurs.
        /// This action is configured in Program.cs as the centralized error handler.
        /// </summary>
        /// <returns>IActionResult - The Error view with error details</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create error view model with current request trace ID for debugging
            return View(new ErrorViewModel 
            { 
                // Either use the current activity ID (in distributed tracing)
                // or fall back to HttpContext trace identifier
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
