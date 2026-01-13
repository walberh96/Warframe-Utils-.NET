using System.Text.Json.Serialization;

/// <summary>
/// OrdersResponse - Top-level container for Warframe Market trading orders API response.
/// Maps to the JSON structure returned by: GET https://api.warframe.market/v1/items/{itemId}/orders
/// 
/// Contains all buy and sell orders for a specific mod/item from active players.
/// Orders are NOT pre-sorted; the application filters and sorts them by price and rank.
/// 
/// Example JSON Structure:
/// {
///   "payload": {
///     "orders": [
///       {
///         "quantity": 5,
///         "platinum": 150,
///         "order_type": "sell",
///         "user": {
///           "ingame_name": "PlayerName",
///           "platform": "pc",
///           "status": "ingame"
///         },
///         "visible": true,
///         "mod_rank": 0
///       },
///       ...
///     ]
///   }
/// }
/// </summary>
public class OrdersResponse
{
    /// <summary>
    /// The payload containing the actual orders list.
    /// Required for proper deserialization.
    /// </summary>
    [JsonPropertyName("payload")]
    public OrdersPayload Payload { get; set; }

    /// <summary>
    /// OrdersPayload - Container for the list of trading orders.
    /// Wraps the orders list for API consistency.
    /// </summary>
    public class OrdersPayload
    {
        /// <summary>
        /// List of all active trading orders for a specific item.
        /// Can be empty if no orders exist (unusual).
        /// Includes both buy and sell orders - application filters by OrderType.
        /// </summary>
        [JsonPropertyName("orders")]
        public List<Order>? Orders { get; set; }
    }

    /// <summary>
    /// Order - Represents a single trading order from a player.
    /// Can be either a sell order (player is selling) or buy order (player is buying).
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Number of items available in this order.
        /// For sell orders: how many the seller has
        /// For buy orders: how many the buyer wants to purchase
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price in Platinum (in-game premium currency).
        /// For mods, typically ranges from 5 to 500+ platinum.
        /// This is the per-item price when Quantity > 1.
        /// </summary>
        [JsonPropertyName("platinum")]
        public int Price { get; set; }

        /// <summary>
        /// Type of order: "sell" or "buy"
        /// - "sell": Player is selling the item
        /// - "buy": Player is buying the item
        /// 
        /// Application filters to show only "sell" orders to buyers.
        /// </summary>
        [JsonPropertyName("order_type")]
        public string? OrderType { get; set; }

        /// <summary>
        /// Information about the player making this order.
        /// Contains in-game name, platform, and online status.
        /// </summary>
        [JsonPropertyName("user")]
        public User? User { get; set; }

        /// <summary>
        /// Whether this order is currently visible and active.
        /// False orders should be hidden from users.
        /// A player might have hidden their order temporarily.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// The rank/level of the mod (if applicable).
        /// Mods can have ranks from 0 to 10+ depending on type.
        /// A rank 0 mod is cheaper than a maxed-out mod.
        /// Important for pricing comparison.
        /// </summary>
        [JsonPropertyName("mod_rank")]
        public int ModRank { get; set; }
    }

    /// <summary>
    /// User - Information about a player making a trading order.
    /// Used to identify the seller/buyer and establish contact.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The player's in-game Warframe username.
        /// This is how you identify and contact the player.
        /// Format: Case-sensitive, alphanumeric
        /// Used to send whisper messages: /w PlayerName
        /// </summary>
        [JsonPropertyName("ingame_name")]
        public string? InGameName { get; set; }

        /// <summary>
        /// The platform the player plays on.
        /// Values: "pc", "ps4", "xbox1", "switch"
        /// Important for cross-platform awareness.
        /// Trading only works within the same platform.
        /// </summary>
        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        /// <summary>
        /// Current online status of the player.
        /// Values:
        /// - "ingame": Player is actively playing Warframe
        /// - "offline": Player is not online
        /// - "away": Player is on the platform but not in Warframe
        /// 
        /// Application filters to show only "ingame" sellers.
        /// Only "ingame" players can complete trades immediately.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
