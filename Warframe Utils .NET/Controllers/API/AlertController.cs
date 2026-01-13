using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warframe_Utils_.NET.Data;
using Warframe_Utils_.NET.Models;
using Warframe_Utils_.NET.Models.DTOS;

namespace Warframe_Utils_.NET.Controllers.API
{
    /// <summary>
    /// AlertController handles all API operations for price alerts.
    /// Only authenticated users can create, manage, and view their alerts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires user to be logged in
    public class AlertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AlertController> _logger;

        public AlertController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<AlertController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Get all price alerts for the current user
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<PriceAlertDto>>> GetUserAlerts()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var alerts = await _context.PriceAlerts
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Ok(alerts.Select(a => MapToDto(a)));
        }

        /// <summary>
        /// Get a specific price alert by ID (must belong to current user)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceAlertDto>> GetAlert(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var alert = await _context.PriceAlerts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (alert == null)
                return NotFound();

            return Ok(MapToDto(alert));
        }

        /// <summary>
        /// Create a new price alert for the current user
        /// </summary>
        [HttpPost("")]
        public async Task<ActionResult<PriceAlertDto>> CreateAlert([FromBody] CreatePriceAlertDto dto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            // Validate input
            if (string.IsNullOrWhiteSpace(dto.ItemName))
                return BadRequest("Item name is required");

            if (dto.AlertPrice < 0)
                return BadRequest("Alert price must be non-negative");

            var alert = new PriceAlert
            {
                UserId = userId,
                ItemName = dto.ItemName.Trim(),
                ItemId = dto.ItemId?.Trim(),
                AlertPrice = dto.AlertPrice,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PriceAlerts.Add(alert);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} created price alert for {alert.ItemName} at price {alert.AlertPrice}");

            return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, MapToDto(alert));
        }

        /// <summary>
        /// Update an existing price alert
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlert(int id, [FromBody] UpdatePriceAlertDto dto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var alert = await _context.PriceAlerts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (alert == null)
                return NotFound();

            // Update allowed fields
            if (!string.IsNullOrWhiteSpace(dto.ItemName))
                alert.ItemName = dto.ItemName.Trim();

            if (dto.AlertPrice.HasValue && dto.AlertPrice >= 0)
                alert.AlertPrice = dto.AlertPrice.Value;

            if (dto.IsActive.HasValue)
                alert.IsActive = dto.IsActive.Value;

            alert.UpdatedAt = DateTime.UtcNow;

            _context.PriceAlerts.Update(alert);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} updated price alert {id}");

            return Ok(MapToDto(alert));
        }

        /// <summary>
        /// Delete a price alert
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var alert = await _context.PriceAlerts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (alert == null)
                return NotFound();

            _context.PriceAlerts.Remove(alert);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} deleted price alert {id}");

            return NoContent();
        }

        /// <summary>
        /// Acknowledge/dismiss an alert notification
        /// </summary>
        [HttpPost("{id}/acknowledge")]
        public async Task<IActionResult> AcknowledgeAlert(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var alert = await _context.PriceAlerts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (alert == null)
                return NotFound();

            alert.IsAcknowledged = true;
            alert.UpdatedAt = DateTime.UtcNow;

            _context.PriceAlerts.Update(alert);
            await _context.SaveChangesAsync();

            return Ok(MapToDto(alert));
        }

        /// <summary>
        /// Get all unread notifications for the current user
        /// </summary>
        [HttpGet("notifications/unread")]
        public async Task<ActionResult<IEnumerable<AlertNotificationDto>>> GetUnreadNotifications()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var notifications = await _context.AlertNotifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .Include(n => n.PriceAlert)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notifications.Select(n => MapNotificationToDto(n)));
        }

        /// <summary>
        /// Mark a notification as read
        /// </summary>
        [HttpPost("notifications/{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var notification = await _context.AlertNotifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
                return NotFound();

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            _context.AlertNotifications.Update(notification);
            await _context.SaveChangesAsync();

            return Ok(MapNotificationToDto(notification));
        }

        // Helper methods
        private PriceAlertDto MapToDto(PriceAlert alert)
        {
            return new PriceAlertDto
            {
                Id = alert.Id,
                ItemName = alert.ItemName,
                ItemId = alert.ItemId,
                AlertPrice = alert.AlertPrice,
                CurrentPrice = alert.CurrentPrice,
                IsActive = alert.IsActive,
                IsTriggered = alert.IsTriggered,
                TriggeredAt = alert.TriggeredAt,
                IsAcknowledged = alert.IsAcknowledged,
                CreatedAt = alert.CreatedAt,
                UpdatedAt = alert.UpdatedAt,
                LastCheckedAt = alert.LastCheckedAt
            };
        }

        private AlertNotificationDto MapNotificationToDto(AlertNotification notification)
        {
            return new AlertNotificationDto
            {
                Id = notification.Id,
                PriceAlertId = notification.PriceAlertId,
                ItemName = notification.PriceAlert?.ItemName ?? "Unknown",
                Message = notification.Message,
                TriggeredPrice = notification.TriggeredPrice,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt,
                ReadAt = notification.ReadAt
            };
        }
    }
}
