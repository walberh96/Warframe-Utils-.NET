using System.Text.Json;
using Warframe_Utils_.NET.Models.DTOS;
using Warframe_Utils_.NET.Models.ViewModels;

namespace Warframe_Utils_.NET.Services
{
    public class WarframeMarketApiService
    {
        readonly HttpClient _httpClient;
        private const string apiUrl = "https://api.warframe.market/v1/";

        //https://api.warframe.market/v1/items
        //https://api.warframe.market/v1/items/secura_dual_cestra/orders
        //https://api.warframe.market/v1/items/energy_nexus

        public WarframeMarketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ModsResponse> GetAllModsAsync() {

            var response = await _httpClient.GetAsync($"{apiUrl}items");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<ModsResponse>(jsonResponse, options);
                Console.WriteLine(result.Payload.Mods.Count);
                if (result == null)
                {
                    throw new Exception("Failed to deserialize WarframeMarket from API response.");
                }
                return result;
            }
            else
            {
                throw new Exception("Failed to fetch data from Warframe Market API");
            }
        }

        public async Task<OrdersResponse> GetAllOrdersAsync(string itemId)
        {

            var response = await _httpClient.GetAsync($"{apiUrl}items/{itemId}/orders");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<OrdersResponse>(jsonResponse, options);

                if (result == null)
                {
                    throw new Exception("Failed to deserialize WarframeMarket from API response.");
                }
                return result;
            }
            else
            {
                throw new Exception("Failed to fetch data from Warframe Market API");
            }
        }

        public async Task<ModDetailResponse> GetItemAsync(string itemId)
        {

            var response = await _httpClient.GetAsync($"{apiUrl}items/{itemId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<ModDetailResponse>(jsonResponse, options);

                if (result == null)
                {
                    throw new Exception("Failed to deserialize WarframeMarket from API response.");
                }
                return result;
            }
            else
            {
                throw new Exception("Failed to fetch data from Warframe Market API");
            }
        }

    }
}
