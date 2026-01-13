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
        private const string ApiUrl = "https://api.warframe.market/v1/";

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
                var response = await _httpClient.GetAsync($"{ApiUrl}items");

                // Ensure the HTTP request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON string to ModsResponse object
                    // PropertyNameCaseInsensitive allows JSON snake_case to map to C# PascalCase
                    var result = JsonSerializer.Deserialize<ModsResponse>(jsonResponse, _jsonOptions);

                    // Validate that deserialization was successful
                    if (result == null || result.Payload == null || result.Payload.Mods == null)
                    {
                        throw new Exception("Failed to deserialize mods from API response - response structure invalid.");
                    }

                    // Log the number of mods retrieved (for debugging)
                    Console.WriteLine($"[INFO] Successfully loaded {result.Payload.Mods.Count} mods from Warframe Market API");

                    return result;
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
        /// API Endpoint: GET https://api.warframe.market/v1/items/{itemId}/orders
        /// Example: https://api.warframe.market/v1/items/energy_nexus/orders
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
                var response = await _httpClient.GetAsync($"{ApiUrl}items/{itemId}/orders");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON to OrdersResponse object
                    var result = JsonSerializer.Deserialize<OrdersResponse>(jsonResponse, _jsonOptions);

                    if (result == null)
                    {
                        throw new Exception($"Failed to deserialize orders for item '{itemId}' - response structure invalid.");
                    }

                    Console.WriteLine($"[INFO] Retrieved {result.Payload.Orders?.Count ?? 0} orders for item: {itemId}");

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
                var response = await _httpClient.GetAsync($"{ApiUrl}items/{itemId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON to ModDetailResponse object
                    var result = JsonSerializer.Deserialize<ModDetailResponse>(jsonResponse, _jsonOptions);

                    if (result == null)
                    {
                        throw new Exception($"Failed to deserialize item details for '{itemId}' - response structure invalid.");
                    }

                    Console.WriteLine($"[INFO] Retrieved item details for: {itemId}");

                    return result;
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
    }
}
