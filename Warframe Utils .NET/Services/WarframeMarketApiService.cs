using System.Text.Json;
using Warframe_Utils_.NET.Models.DTOS;
using Warframe_Utils_.NET.Models.ViewModels;

namespace Warframe_Utils_.NET.Services
{
    /// <summary>
    /// WarframeMarketApiService handles all communication with the Warframe Market API.
    /// 
    /// API Documentation: https://api.warframe.market/v1/
    /// This service provides methods to:
    /// - Get all available items/mods
    /// - Get trading orders for specific items
    /// - Get detailed item information (description, rarity, trading tax, etc.)
    /// 
    /// The Warframe Market API is a public API that requires no authentication.
    /// All responses are JSON-formatted and case-insensitive in property names.
    /// </summary>
    public class WarframeMarketApiService
    {
        // HTTP client for making API requests - provided by dependency injection
        private readonly HttpClient _httpClient;

        // Base URL for all Warframe Market API requests
        // Updated to v2 API: https://42bytes.notion.site/WFM-Api-v2-Documentation
        private const string ApiUrl = "https://api.warframe.market/v2/";

        // JSON serializer options for flexible deserialization
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true  // Allows matching snake_case JSON to PascalCase C#
        };

        /// <summary>
        /// Constructor - receives HttpClient via dependency injection from Program.cs.
        /// The framework manages the HttpClient lifecycle and connection pooling.
        /// </summary>
        public WarframeMarketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// GetAllModsAsync - Fetches the complete list of all available mods/items from Warframe Market.
        /// 
        /// This is the most expensive call (largest response) and is called on every page load.
        /// Consider implementing caching if performance becomes an issue.
        /// 
        /// API Endpoint: GET https://api.warframe.market/v1/items
        /// Response Size: ~500KB+ (contains thousands of items)
        /// 
        /// Useful for:
        /// - Populating the autocomplete search suggestions
        /// - Building the complete mod inventory
        /// </summary>
        /// <returns>ModsResponse containing list of all available mods</returns>
        /// <exception cref="Exception">Thrown if API call fails or response cannot be deserialized</exception>
        public async Task<ModsResponse> GetAllModsAsync()
        {
            try
            {
                // Make HTTP GET request to the items endpoint
                // v2 API: GET /v2/items returns items array in data field
                var response = await _httpClient.GetAsync($"{ApiUrl}items");

                // Ensure the HTTP request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // v2 API Response structure: { apiVersion: "x.x.x", data: [...], error: null }
                    // Parse JSON to extract the data array
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        var dataElement = doc.RootElement.GetProperty("data");
                        var itemsArray = new List<ModsResponse.Mod>();

                        // Iterate through each item in the data array
                        foreach (var item in dataElement.EnumerateArray())
                        {
                            // v2 API structure: item has i18n.en with name, icon, thumb
                            var id = item.GetProperty("id").GetString();
                            var slug = item.GetProperty("slug").GetString();
                            var i18n = item.GetProperty("i18n").GetProperty("en");
                            var name = i18n.GetProperty("name").GetString();
                            var thumb = i18n.GetProperty("thumb").GetString();

                            // Create a Mod object compatible with our existing model
                            itemsArray.Add(new ModsResponse.Mod
                            {
                                ItemName = name,
                                Thumb = thumb,
                                UrlName = slug
                            });
                        }

                        if (itemsArray.Count == 0)
                        {
                            throw new Exception("Failed to parse mods from API response - no items found.");
                        }

                        // Create a ModsResponse object to maintain compatibility with existing code
                        var result = new ModsResponse
                        {
                            Payload = new ModsResponse.ModsPayload { Mods = itemsArray }
                        };

                        // Log the number of mods retrieved (for debugging)
                        Console.WriteLine($"[INFO] Successfully loaded {result.Payload.Mods.Count} mods from Warframe Market API v2");

                        return result;
                    }
                }
                else
                {
                    // API returned an error status code
                    throw new Exception($"Failed to fetch data from Warframe Market API. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Network error while fetching mods from Warframe Market API.", ex);
            }
        }

        /// <summary>
        /// GetAllOrdersAsync - Fetches all trading orders for a specific item.
        /// 
        /// Returned orders include:
        /// - Sell orders (players selling items)
        /// - Buy orders (players buying items)
        /// - Player information (name, status, platform)
        /// - Price and quantity
        /// - Mod rank (for mods with ranks)
        /// 
        /// API Endpoint: GET https://api.warframe.market/v2/orders/item/{slug}
        /// Example: https://api.warframe.market/v2/orders/item/energy_nexus
        /// 
        /// Useful for:
        /// - Displaying current market prices
        /// - Finding online sellers/buyers
        /// - Market analysis
        /// </summary>
        /// <param name="itemId">URL-safe item identifier (e.g., "energy_nexus"). 
        /// Get this from Mod.UrlName from GetAllModsAsync()</param>
        /// <returns>OrdersResponse containing list of active orders for the item</returns>
        /// <exception cref="Exception">Thrown if API call fails or response cannot be deserialized</exception>
        public async Task<OrdersResponse> GetAllOrdersAsync(string itemId)
        {
            try
            {
                // Validate input to prevent API abuse
                if (string.IsNullOrWhiteSpace(itemId))
                {
                    throw new ArgumentException("Item ID cannot be null or empty.", nameof(itemId));
                }

                // Make HTTP GET request to the orders endpoint for the specific item
                // v2 API: Changed from /items/{itemId}/orders to /orders/item/{slug}
                var response = await _httpClient.GetAsync($"{ApiUrl}orders/item/{itemId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON to OrdersResponse object
                    var result = JsonSerializer.Deserialize<OrdersResponse>(jsonResponse, _jsonOptions);

                    if (result == null)
                    {
                        throw new Exception($"Failed to deserialize orders for item '{itemId}' - response structure invalid.");
                    }

                    // Check if orders list exists
                    if (result.Orders == null)
                    {
                        Console.WriteLine($"[INFO] Retrieved 0 orders for item: {itemId}");
                        result.Orders = new List<OrdersResponse.Order>();
                    }
                    else
                    {
                        Console.WriteLine($"[INFO] Retrieved {result.Orders.Count} orders for item: {itemId}");
                    }

                    return result;
                }
                else
                {
                    throw new Exception($"Failed to fetch orders for item '{itemId}'. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error while fetching orders for item '{itemId}'.", ex);
            }
        }

        /// <summary>
        /// GetItemAsync - Fetches detailed information about a specific item.
        /// 
        /// Returned information includes:
        /// - Item description and stats
        /// - Rarity level
        /// - Trading tax cost (NPC cost)
        /// - Icon URLs (for displaying item image)
        /// - Category tags
        /// - Wiki link
        /// - Drop locations (where to find/farm the item)
        /// - Item set variants (different ranks)
        /// 
        /// API Endpoint: GET https://api.warframe.market/v1/items/{itemId}
        /// Example: https://api.warframe.market/v1/items/energy_nexus
        /// 
        /// Useful for:
        /// - Displaying item description on search results
        /// - Showing trading tax information
        /// - Linking to wiki for more information
        /// - Displaying item icon/thumbnail
        /// </summary>
        /// <param name="itemId">URL-safe item identifier (e.g., "energy_nexus")</param>
        /// <returns>ModDetailResponse containing detailed item information</returns>
        /// <exception cref="Exception">Thrown if API call fails or response cannot be deserialized</exception>
        public async Task<ModDetailResponse> GetItemAsync(string itemId)
        {
            try
            {
                // Validate input to prevent API abuse
                if (string.IsNullOrWhiteSpace(itemId))
                {
                    throw new ArgumentException("Item ID cannot be null or empty.", nameof(itemId));
                }

                // Make HTTP GET request to the item detail endpoint
                // v2 API: GET /v2/item/{slug}
                var response = await _httpClient.GetAsync($"{ApiUrl}item/{itemId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // v2 API Response structure: { apiVersion: "x.x.x", data: Item, error: null }
                    // Parse JSON to extract the Item data
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        var dataElement = doc.RootElement.GetProperty("data");
                        var itemJson = JsonSerializer.Serialize(dataElement);
                        
                        // Parse the Item into our ModDetailSetItem structure
                        // v2 Item has i18n.en with name, description, icon, thumb
                        var itemData = JsonDocument.Parse(itemJson);
                        var itemRoot = itemData.RootElement;
                        
                        var en = default(JsonElement);
                        var icon = itemRoot.TryGetProperty("i18n", out var i18n) && 
                                  i18n.TryGetProperty("en", out en) &&
                                  en.TryGetProperty("icon", out var iconProp) 
                                  ? iconProp.GetString() : null;
                        
                        var thumb = en.ValueKind != JsonValueKind.Undefined && en.TryGetProperty("thumb", out var thumbProp) 
                                  ? thumbProp.GetString() : null;
                        
                        var description = en.ValueKind != JsonValueKind.Undefined && en.TryGetProperty("description", out var descProp) 
                                       ? descProp.GetString() : null;
                        
                        var name = en.ValueKind != JsonValueKind.Undefined && en.TryGetProperty("name", out var nameProp) 
                                ? nameProp.GetString() : null;

                        var tradingTax = itemRoot.TryGetProperty("tradingTax", out var taxProp) 
                                      ? taxProp.GetInt32() : 0;

                        // Create a ModDetailSetItem to match the view's expectations
                        var setItem = new ModDetailResponse.ModDetailPayload.ModDetailItem.ModDetailSetItem
                        {
                            Icon = icon,
                            Thumb = thumb,
                            TradingTax = tradingTax,
                            UrlName = itemId,
                            En = new ModDetailResponse.ModDetailPayload.ModDetailItem.ModDetailSetItem.ModDetailLocalization
                            {
                                ItemName = name,
                                Description = description
                            }
                        };

                        // Create wrapper structure to match view expectations
                        var result = new ModDetailResponse
                        {
                            Payload = new ModDetailResponse.ModDetailPayload
                            {
                                Item = new ModDetailResponse.ModDetailPayload.ModDetailItem
                                {
                                    ItemsInSet = new List<ModDetailResponse.ModDetailPayload.ModDetailItem.ModDetailSetItem> { setItem }
                                }
                            }
                        };

                        Console.WriteLine($"[INFO] Retrieved item details for: {itemId}");

                        return result;
                    }
                }
                else
                {
                    throw new Exception($"Failed to fetch item details for '{itemId}'. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error while fetching item details for '{itemId}'.", ex);
            }
        }

        /// <summary>
        /// GetItemPrice - Gets the current average price of an item by fetching its orders.
        /// Calculates average of the lowest sell orders.
        /// </summary>
        /// <param name="itemId">URL-safe item identifier (e.g., "energy_nexus")</param>
        /// <returns>Average price of the item, or null if not available</returns>
        public async Task<decimal?> GetItemPrice(string itemId)
        {
            try
            {
                var orders = await GetAllOrdersAsync(itemId);
                
                if (orders?.Orders == null || orders.Orders.Count == 0)
                {
                    return null;
                }

                // Get sell orders (ascending price) and take lowest prices
                var sellOrders = orders.Orders
                    .Where(o => o.Type?.ToLower() == "sell")
                    .OrderBy(o => o.Platinum)
                    .Take(5) // Average of 5 lowest prices
                    .ToList();

                if (!sellOrders.Any())
                {
                    return null;
                }

                var averagePrice = (decimal)sellOrders.Average(o => o.Platinum);
                return averagePrice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to get price for item '{itemId}': {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// SearchAndGetPrice - Searches for an item by name and retrieves its price.
        /// This is a fallback method when item ID is not available.
        /// </summary>
        /// <param name="itemName">The name of the item to search for</param>
        /// <returns>Price of the item, or null if not found</returns>
        public async Task<decimal?> SearchAndGetPrice(string itemName)
        {
            try
            {
                // First, get all mods/items to find a match
                var allMods = await GetAllModsAsync();
                
                if (allMods?.Payload?.Mods == null)
                {
                    return null;
                }

                // Search for the item (case-insensitive, partial match)
                var matchingMod = allMods.Payload.Mods
                    .FirstOrDefault(m => m.ItemName != null && 
                        m.ItemName.Contains(itemName, StringComparison.OrdinalIgnoreCase));

                if (matchingMod == null || string.IsNullOrEmpty(matchingMod.UrlName))
                {
                    return null;
                }

                // Get the price for the matching item
                return await GetItemPrice(matchingMod.UrlName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to search and get price for '{itemName}': {ex.Message}");
                return null;
            }
        }
    }
}
