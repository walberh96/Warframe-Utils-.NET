using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warframe_Utils_.NET.Models
{
    /// <summary>
    /// PriceAlert represents a user's price monitoring alert for a specific item.
    /// When the market price falls to or below the AlertPrice, the user will be notified.
    /// </summary>
    public class PriceAlert
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the user who created this alert
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Name of the item/mod to track (e.g., "Serration", "Excalibur Prime Set")
        /// </summary>
        [Required]
        [StringLength(500)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// The API ID/slug for the item from Warframe Market API (optional, for automated lookup)
        /// </summary>
        [StringLength(500)]
        public string? ItemId { get; set; }

        /// <summary>
        /// The price threshold - alert triggers when market price <= AlertPrice
        /// </summary>
        [Required]
        [Range(0, 999999, ErrorMessage = "Alert price must be between 0 and 999,999")]
        public decimal AlertPrice { get; set; }

        /// <summary>
        /// Current market price (cached from last check)
        /// </summary>
        public decimal? CurrentPrice { get; set; }

        /// <summary>
        /// Whether this alert is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Whether the alert has been triggered
        /// </summary>
        public bool IsTriggered { get; set; } = false;

        /// <summary>
        /// Timestamp when the alert was triggered (null if not triggered)
        /// </summary>
        public DateTime? TriggeredAt { get; set; }

        /// <summary>
        /// Whether the user has acknowledged/dismissed this alert
        /// </summary>
        public bool IsAcknowledged { get; set; } = false;

        /// <summary>
        /// When the alert was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the alert was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last time the price was checked from the API
        /// </summary>
        public DateTime? LastCheckedAt { get; set; }
    }
}
