using Microsoft.EntityFrameworkCore;
using Warframe_Utils_.NET.Data;
using Warframe_Utils_.NET.Models;
using Warframe_Utils_.NET.Services;

namespace Warframe_Utils_.NET.Services
{
    /// <summary>
    /// Background service that periodically checks prices for active price alerts
    /// and creates notifications when prices fall to or below the alert threshold.
    /// 
    /// Runs every 5 minutes by default (configurable via settings)
    /// </summary>
    public class PriceAlertCheckService : BackgroundService
    {
        private readonly ILogger<PriceAlertCheckService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WarframeMarketApiService _marketApiService;
        private TimeSpan _checkInterval = TimeSpan.FromMinutes(5); // Default: Check every 5 minutes

        public PriceAlertCheckService(
            ILogger<PriceAlertCheckService> logger,
            IServiceProvider serviceProvider,
            WarframeMarketApiService marketApiService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _marketApiService = marketApiService;
        }

        /// <summary>
        /// Main execution method for the background service
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Price Alert Check Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckPriceAlerts(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in Price Alert Check Service");
                }

                // Wait before the next check
                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Price Alert Check Service stopped");
        }

        /// <summary>
        /// Check all active alerts and trigger notifications if thresholds are met
        /// </summary>
        private async Task CheckPriceAlerts(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Get all active alerts that haven't been triggered
                var activeAlerts = await context.PriceAlerts
                    .Where(a => a.IsActive && !a.IsTriggered)
                    .ToListAsync(stoppingToken);

                if (!activeAlerts.Any())
                {
                    _logger.LogDebug("No active alerts to check");
                    return;
                }
                _logger.LogInformation($"Checking {activeAlerts.Count} active price alerts");

                foreach (var alert in activeAlerts)
                {
                    try
                    {
                        // Attempt to get current price from Warframe Market API
                        decimal? currentPrice = null;

                        if (!string.IsNullOrEmpty(alert.ItemId))
                        {
                            // Try to get price by item ID (more reliable)
                            currentPrice = await _marketApiService.GetItemPrice(alert.ItemId);
                        }

                        if (!currentPrice.HasValue && !string.IsNullOrEmpty(alert.ItemName))
                        {
                            // Fallback: Try to search by item name
                            currentPrice = await _marketApiService.SearchAndGetPrice(alert.ItemName);
                        }

                        // Update the alert with the current price
                        alert.CurrentPrice = currentPrice;
                        alert.LastCheckedAt = DateTime.UtcNow;

                        // Check if price has fallen to or below the alert threshold
                        if (currentPrice.HasValue && currentPrice.Value <= alert.AlertPrice)
                        {
                            _logger.LogWarning(
                                $"Price alert triggered for {alert.ItemName}: Current price {currentPrice} <= Alert price {alert.AlertPrice}");

                            // Mark alert as triggered
                            alert.IsTriggered = true;
                            alert.TriggeredAt = DateTime.UtcNow;

                            // Create notification for the user
                            var notification = new AlertNotification
                            {
                                UserId = alert.UserId,
                                PriceAlertId = alert.Id,
                                Message = $"The price of {alert.ItemName} has dropped to {currentPrice:C} (alert threshold: {alert.AlertPrice:C})",
                                TriggeredPrice = currentPrice.Value,
                                CreatedAt = DateTime.UtcNow,
                                IsRead = false
                            };

                            context.AlertNotifications.Add(notification);
                        }

                        context.PriceAlerts.Update(alert);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error checking price for alert {alert.Id} ({alert.ItemName})");
                        // Continue checking other alerts even if one fails
                    }
                }

                // Save all changes at once
                await context.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
