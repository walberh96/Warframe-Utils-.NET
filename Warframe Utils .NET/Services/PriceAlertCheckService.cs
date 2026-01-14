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
    /// Runs every 30 seconds for real-time price monitoring
    /// </summary>
    public class PriceAlertCheckService : BackgroundService
    {
        private readonly ILogger<PriceAlertCheckService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WarframeMarketApiService _marketApiService;
        private TimeSpan _checkInterval = TimeSpan.FromSeconds(30); // Check every 30 seconds

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

                // Get all active alerts (keep checking even if already triggered, so we can create notifications)
                var activeAlerts = await context.PriceAlerts
                    .Where(a => a.IsActive)
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
                                $"Price condition met for {alert.ItemName}: Current price {currentPrice} <= Alert price {alert.AlertPrice}");

                            // Mark alert as triggered (just a status flag)
                            if (!alert.IsTriggered)
                            {
                                alert.IsTriggered = true;
                                alert.TriggeredAt = DateTime.UtcNow;
                            }

                            // Check if there's already an unread notification for this alert with the same price
                            // This prevents duplicate notifications for the same price, but allows new ones if price changes
                            var existingUnreadNotification = await context.AlertNotifications
                                .AnyAsync(n => n.PriceAlertId == alert.Id && !n.IsRead && n.TriggeredPrice == currentPrice.Value, stoppingToken);

                            if (!existingUnreadNotification)
                            {
                                // Create notification for the user
                                var notification = new AlertNotification
                                {
                                    UserId = alert.UserId,
                                    PriceAlertId = alert.Id,
                                    Message = $"The price of {alert.ItemName} has dropped to {currentPrice.Value} platinum (alert threshold: {alert.AlertPrice})",
                                    TriggeredPrice = currentPrice.Value,
                                    CreatedAt = DateTime.UtcNow,
                                    IsRead = false
                                };

                                context.AlertNotifications.Add(notification);
                                _logger.LogWarning($"Notification created for alert {alert.Id} ({alert.ItemName}) at price {currentPrice.Value}");
                            }
                            else
                            {
                                _logger.LogInformation($"Unread notification already exists for alert {alert.Id} at price {currentPrice.Value}, skipping");
                            }
                        }
                        else if (currentPrice.HasValue && currentPrice.Value > alert.AlertPrice)
                        {
                            // Price went back up above threshold - reset triggered status so we can notify again when it drops
                            if (alert.IsTriggered)
                            {
                                // Check if all notifications for this alert are read
                                var hasUnreadNotifications = await context.AlertNotifications
                                    .AnyAsync(n => n.PriceAlertId == alert.Id && !n.IsRead, stoppingToken);
                                
                                if (!hasUnreadNotifications)
                                {
                                    // All notifications are read and price is above threshold - reset so we can notify again
                                    alert.IsTriggered = false;
                                    alert.TriggeredAt = null;
                                    _logger.LogInformation($"Reset alert {alert.Id} - price above threshold and all notifications read");
                                }
                            }
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
