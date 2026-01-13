using System.Text.Json.Serialization;

/// <summary>
/// OrdersResponse - Top-level container for Warframe Market v2 trading orders API response.
/// Maps to the JSON structure returned by: GET https://api.warframe.market/v2/orders/item/{slug}
/// 
/// v2 API Response: { apiVersion: "...", data: [...], error: null }
/// 
/// Contains all buy and sell orders for a specific mod/item from active players.
/// Orders are NOT pre-sorted; the application filters and sorts them by price and rank.
/// </summary>
public class OrdersResponse
{
    /// <summary>
    /// API version of the response.
    /// </summary>
    [JsonPropertyName("apiVersion")]
    public string? ApiVersion { get; set; }

    /// <summary>
    /// List of all active trading orders for a specific item.
    /// Can be empty if no orders exist.
    /// Includes both buy and sell orders - application filters by Type.
    /// </summary>
    [JsonPropertyName("data")]
    public List<Order>? Orders { get; set; }

    /// <summary>
    /// Error message if the API call failed.
    /// Null if successful.
    /// </summary>
    [JsonPropertyName("error")]
    public object? Error { get; set; }

    /// <summary>
    /// Order - Represents a single trading order from a player.
    /// Can be either a sell order (player is selling) or buy order (player is buying).
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Unique identifier for this order.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Type of order: "sell" or "buy"
        /// - "sell": Player is selling the item
        /// - "buy": Player is buying the item
        /// 
        /// Application filters to show only "sell" orders to buyers.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Price in Platinum (in-game premium currency).
        /// For mods, typically ranges from 5 to 500+ platinum.
        /// This is the per-item price when Quantity > 1.
        /// </summary>
        [JsonPropertyName("platinum")]
        public int Platinum { get; set; }

        /// <summary>
        /// Number of items available in this order.
        /// For sell orders: how many the seller has
        /// For buy orders: how many the buyer wants to purchase
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Number of items per trade.
        /// </summary>
        [JsonPropertyName("perTrade")]
        public int PerTrade { get; set; }

        /// <summary>
        /// The rank/level of the mod (if applicable).
        /// Mods can have ranks from 0 to 10+ depending on type.
        /// A rank 0 mod is cheaper than a maxed-out mod.
        /// Important for pricing comparison.
        /// </summary>
        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Whether this order is currently visible and active.
        /// False orders should be hidden from users.
        /// A player might have hidden their order temporarily.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// The timestamp when this order was created.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public string? CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when this order was last updated.
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public string? UpdatedAt { get; set; }

        /// <summary>
        /// The ID of the item being traded.
        /// </summary>
        [JsonPropertyName("itemId")]
        public string? ItemId { get; set; }

        /// <summary>
        /// Information about the player making this order.
        /// Contains in-game name, platform, and online status.
        /// </summary>
        [JsonPropertyName("user")]
        public User? User { get; set; }
    }

    /// <summary>
    /// User - Information about a player making a trading order.
    /// Used to identify the seller/buyer and establish contact.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The player's in-game Warframe username.
        /// This is how you identify and contact the player.
        /// Format: Case-sensitive, alphanumeric
        /// Used to send whisper messages: /w PlayerName
        /// </summary>
        [JsonPropertyName("ingameName")]
        public string? InGameName { get; set; }

        /// <summary>
        /// URL-safe slug version of the username.
        /// </summary>
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        /// <summary>
        /// URL to the user's avatar image.
        /// </summary>
        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// User's reputation score.
        /// Higher values indicate more trusted traders.
        /// </summary>
        [JsonPropertyName("reputation")]
        public int Reputation { get; set; }

        /// <summary>
        /// The platform the player plays on.
        /// Values: "pc", "ps4", "xbox1", "switch"
        /// Important for cross-platform awareness.
        /// Trading only works within the same platform.
        /// </summary>
        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        /// <summary>
        /// Whether the player has crossplay enabled.
        /// </summary>
        [JsonPropertyName("crossplay")]
        public bool Crossplay { get; set; }

        /// <summary>
        /// Player's preferred locale/language.
        /// </summary>
        [JsonPropertyName("locale")]
        public string? Locale { get; set; }

        /// <summary>
        /// Current online status of the player.
        /// Values:
        /// - "ingame": Player is actively playing Warframe
        /// - "online": Player is on the platform but not in Warframe
        /// - "offline": Player is not online
        /// 
        /// Application filters to show only "ingame" sellers.
        /// Only "ingame" players can complete trades immediately.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Player's current activity information.
        /// </summary>
        [JsonPropertyName("activity")]
        public object? Activity { get; set; }

        /// <summary>
        /// Timestamp of when the player was last seen.
        /// </summary>
        [JsonPropertyName("lastSeen")]
        public string? LastSeen { get; set; }
    }
}
