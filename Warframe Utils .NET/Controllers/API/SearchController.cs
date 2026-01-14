using Microsoft.AspNetCore.Mvc;
using Warframe_Utils_.NET.Services;
using Warframe_Utils_.NET.Models.DTOS;

namespace Warframe_Utils_.NET.Controllers.API
{
    /// <summary>
    /// SearchController provides API endpoints for searching mods and items
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly WarframeMarketApiService _marketApiService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(
            WarframeMarketApiService marketApiService,
            ILogger<SearchController> logger)
        {
            _marketApiService = marketApiService;
            _logger = logger;
        }

        /// <summary>
        /// Search for a mod/item and return details with orders
        /// GET: api/Search?modName=serration
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Search([FromQuery] string modName)
        {
            if (string.IsNullOrWhiteSpace(modName))
            {
                return BadRequest(new { error = "modName parameter is required" });
            }

            try
            {
                // Get all items to find the matching one
                var allItemsResponse = await _marketApiService.GetAllModsAsync();
                if (allItemsResponse?.Payload?.Mods == null)
                {
                    return StatusCode(500, new { error = "Failed to fetch items list" });
                }

                // Find the item
                var item = allItemsResponse.Payload.Mods
                    .FirstOrDefault(i => i.ItemName.Equals(modName, StringComparison.OrdinalIgnoreCase));

                if (item == null)
                {
                    return NotFound(new { error = "Item not found" });
                }

                // Get item details
                var detailsResponse = await _marketApiService.GetItemAsync(item.UrlName);
                
                // Get orders
                var ordersResponse = await _marketApiService.GetAllOrdersAsync(item.UrlName);

                // Flatten the mod details structure for easier frontend consumption
                var firstItem = detailsResponse?.Payload?.Item?.ItemsInSet?.FirstOrDefault();
                var modDetails = firstItem != null ? new
                {
                    item_name = firstItem.En?.ItemName ?? item.ItemName,
                    description = firstItem.En?.Description ?? string.Empty,
                    thumb = firstItem.Thumb ?? item.Thumb,
                    icon = firstItem.Icon,
                    rarity = firstItem.Rarity,
                    trading_tax = firstItem.TradingTax,
                    wiki_link = firstItem.En?.WikiLink,
                    url_name = firstItem.UrlName ?? item.UrlName
                } : null;

                var response = new
                {
                    modDetails = modDetails,
                    orders = ordersResponse?.Orders?
                        .Where(o => o.User != null)
                        .OrderBy(o => o.Type == "sell" ? o.Platinum : -o.Platinum)
                        .Select(o => new
                        {
                            order_type = o.Type,
                            platinum = o.Platinum,
                            quantity = o.Quantity,
                            user = new
                            {
                                ingame_name = o.User!.InGameName,
                                status = o.User.Status
                            }
                        })
                        .ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for mod: {ModName}", modName);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get all available items for autocomplete
        /// GET: api/Search/items
        /// </summary>
        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var response = await _marketApiService.GetAllModsAsync();
                
                if (response?.Payload?.Mods == null)
                {
                    return StatusCode(500, new { error = "Failed to fetch items" });
                }

                var items = response.Payload.Mods
                    .Select(i => new
                    {
                        url_name = i.UrlName,
                        item_name = i.ItemName
                    })
                    .ToList();

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all items");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
