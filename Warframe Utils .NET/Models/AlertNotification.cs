using System.ComponentModel.DataAnnotations;

namespace Warframe_Utils_.NET.Models
{
    /// <summary>
    /// AlertNotification represents a notification sent to a user when a price alert is triggered.
    /// </summary>
    public class AlertNotification
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the user who should receive this notification
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to the price alert that triggered this notification
        /// </summary>
        [Required]
        public int PriceAlertId { get; set; }

        /// <summary>
        /// The alert that triggered this notification
        /// </summary>
        public virtual PriceAlert? PriceAlert { get; set; }

        /// <summary>
        /// Human-readable message for the notification
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Whether the user has read/viewed this notification
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// When the notification was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the notification was read (null if unread)
        /// </summary>
        public DateTime? ReadAt { get; set; }

        /// <summary>
        /// The price at which the alert was triggered
        /// </summary>
        public decimal TriggeredPrice { get; set; }
    }
}
