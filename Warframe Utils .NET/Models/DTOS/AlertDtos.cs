namespace Warframe_Utils_.NET.Models.DTOS
{
    /// <summary>
    /// DTO for creating a new price alert
    /// </summary>
    public class CreatePriceAlertDto
    {
        /// <summary>
        /// Name of the item to track (required)
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Optional API ID/slug for the item from Warframe Market
        /// </summary>
        public string? ItemId { get; set; }

        /// <summary>
        /// The price threshold for the alert
        /// </summary>
        public decimal AlertPrice { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing price alert
    /// </summary>
    public class UpdatePriceAlertDto
    {
        /// <summary>
        /// Updated item name (optional)
        /// </summary>
        public string? ItemName { get; set; }

        /// <summary>
        /// Updated alert price threshold (optional)
        /// </summary>
        public decimal? AlertPrice { get; set; }

        /// <summary>
        /// Whether the alert is active (optional)
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO for returning price alert information
    /// </summary>
    public class PriceAlertDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemId { get; set; }
        public decimal AlertPrice { get; set; }
        public decimal? CurrentPrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsTriggered { get; set; }
        public DateTime? TriggeredAt { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastCheckedAt { get; set; }
    }

    /// <summary>
    /// DTO for returning alert notifications
    /// </summary>
    public class AlertNotificationDto
    {
        public int Id { get; set; }
        public int PriceAlertId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public decimal TriggeredPrice { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
