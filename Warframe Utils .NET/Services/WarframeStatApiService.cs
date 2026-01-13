using System.Text.Json;
using Warframe_Utils_.NET.Models.ViewModels;

namespace Warframe_Utils_.NET.Services
{
    /// <summary>
    /// WarframeStatApiService handles all communication with the Warframe Status API.
    /// 
    /// API Documentation: https://api.warframestat.us/
    /// This service provides real-time game status information including:
    /// - Void Trader (Baroo) location and arrival/departure times
    /// - Current arbitration mission type and location
    /// - Day/night cycles for different planets (Cetus, Vallis, Cambion Drift)
    /// 
    /// The Warframe Status API is a public API maintained by the community.
    /// No authentication required. Responses are JSON-formatted.
    /// PC platform is used here - can be changed to ps4, xbox1, or switch for other platforms.
    /// </summary>
    public class WarframeStatApiService
    {
        // HTTP client for making API requests - provided by dependency injection
        private readonly HttpClient _httpClient;

        // Base URL for Warframe Status API targeting PC platform
        // Change "pc" to "ps4", "xbox1", or "switch" for other platforms
        private const string ApiUrl = "https://api.warframestat.us/pc";

        // JSON serializer options for flexible deserialization
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true  // Allows matching snake_case JSON to PascalCase C#
        };

        /// <summary>
        /// Constructor - receives HttpClient via dependency injection from Program.cs.
        /// The framework manages the HttpClient lifecycle and connection pooling.
        /// </summary>
        public WarframeStatApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// GetWarframeStatusAsync - Fetches current game server status for PC platform.
        /// 
        /// Returns information about:
        /// - Void Trader (Baroo) - active/inactive status
        /// - Arbitration - current mission type and location
        /// - Cetus Cycle (Plains of Eidolon) - day/night cycle
        /// - Vallis Cycle (Orb Vallis) - warm/cold cycle
        /// - Cambion Cycle (Deimos) - fass/vome cycle
        /// 
        /// API Endpoint: GET https://api.warframestat.us/pc
        /// Response Size: Small (~10KB)
        /// Update Frequency: Real-time, updates frequently throughout the day
        /// 
        /// Useful for:
        /// - Displaying game status bar on home page
        /// - Notifying players of current events
        /// - Planning gameplay around cycles and events
        /// 
        /// Example Response Properties:
        /// - voidTrader.active - boolean, true if Void Trader is currently available
        /// - cetusCycle.isDay - boolean, true for daytime, false for nighttime
        /// - vallisCycle.state - string, either "warm" or "cold"
        /// - cambionCycle.state - string, either "fass" or "vome"
        /// - arbitration.node - string, mission location and type
        /// </summary>
        /// <returns>WarframeStatus containing all game server status information</returns>
        /// <exception cref="Exception">Thrown if API call fails or response cannot be deserialized</exception>
        public async Task<WarframeStatus> GetWarframeStatusAsync()
        {
            try
            {
                // Make HTTP GET request to the PC platform endpoint
                var response = await _httpClient.GetAsync(ApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON to WarframeStatus object
                    // PropertyNameCaseInsensitive allows JSON snake_case to map to C# PascalCase
                    var result = JsonSerializer.Deserialize<WarframeStatus>(jsonResponse, _jsonOptions);

                    if (result == null)
                    {
                        throw new Exception("Failed to deserialize WarframeStatus from API response - response structure invalid.");
                    }

                    Console.WriteLine($"[INFO] Successfully retrieved Warframe server status at {DateTime.UtcNow:O}");

                    return result;
                }
                else
                {
                    // API returned an error status code
                    throw new Exception($"Failed to fetch data from Warframe Status API. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Network error while fetching Warframe status from API.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize Warframe status JSON response.", ex);
            }
        }
    }
}
