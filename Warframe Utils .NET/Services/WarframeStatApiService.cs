using System.Text.Json;
using Warframe_Utils_.NET.Models.ViewModels;

namespace Warframe_Utils_.NET.Services
{
    public class WarframeStatApiService
    {
        private readonly HttpClient _httpClient;
        private const string apiUrl = "https://api.warframestat.us/pc";

        public WarframeStatApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WarframeStatus> GetWarframeStatusAsync()
        {
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                //Console.Write(jsonResponse);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<WarframeStatus>(jsonResponse,options);

                if (result == null)
                {
                    throw new Exception("Failed to deserialize WarframeStatus from API response.");
                }
                //Console.WriteLine("Baroo is " + result.BarooData.isActive);
                return result;
            }
            else
            {
                throw new Exception("Failed to fetch data from Warframe API");
            }
        }
    }
}
