using Microsoft.AspNetCore.Mvc;
using Warframe_Utils_.NET.Services;
using Warframe_Utils_.NET.Models.DTOS;

namespace Warframe_Utils_.NET.Controllers.API
{
    /// <summary>
    /// GameStatusController provides API endpoints for the Next.js frontend
    /// to fetch game status and market data
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GameStatusController : ControllerBase
    {
        private readonly WarframeStatApiService _statApiService;
        private readonly ILogger<GameStatusController> _logger;

        public GameStatusController(
            WarframeStatApiService statApiService,
            ILogger<GameStatusController> logger)
        {
            _statApiService = statApiService;
            _logger = logger;
        }

        /// <summary>
        /// Get current game status including Cetus cycle, Void Trader, and Arbitration
        /// GET: api/GameStatus
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> GetGameStatus()
        {
            try
            {
                var status = await _statApiService.GetWarframeStatusAsync();
                
                if (status == null)
                {
                    return StatusCode(500, new { error = "Failed to fetch game status" });
                }

                // Helper function to calculate time remaining from expiry
                string CalculateTimeLeft(string expiry)
                {
                    if (string.IsNullOrEmpty(expiry))
                        return null;

                    try
                    {
                        var expiryTime = DateTime.Parse(expiry);
                        var timeLeft = expiryTime - DateTime.UtcNow;
                        
                        if (timeLeft.TotalMinutes < 0)
                            return "Just changed";
                        
                        if (timeLeft.TotalMinutes < 60)
                            return $"{timeLeft.Minutes}m {timeLeft.Seconds}s";
                        
                        return $"{timeLeft.Hours}h {timeLeft.Minutes}m";
                    }
                    catch
                    {
                        return null;
                    }
                }

                var response = new
                {
                    cetusCycle = status.CetusData != null ? new
                    {
                        state = status.CetusData.isDay ? "Day" : "Night",
                        timeLeft = CalculateTimeLeft(status.CetusData.expiry) ?? (status.CetusData.isDay ? "Daytime" : "Nighttime")
                    } : null,
                    voidTrader = status.BarooData != null ? new
                    {
                        active = status.BarooData.isActive,
                        character = "Baro Ki'Teer",
                        location = status.BarooData.isActive ? "Active" : "Away"
                    } : null,
                    vallisCycle = status.VenusData != null ? new
                    {
                        state = status.VenusData.state == "warm" ? "Warm" : "Cold",
                        timeLeft = CalculateTimeLeft(status.VenusData.expiry) ?? (status.VenusData.state == "warm" ? "Warm Period" : "Cold Period")
                    } : null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching game status");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
