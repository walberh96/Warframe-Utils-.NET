using System.Text.Json.Serialization;

namespace Warframe_Utils_.NET.Models.DTOS
{
    /// <summary>
    /// ModsResponse - Top-level container for the Warframe Market "items" API endpoint response.
    /// Maps to the JSON structure returned by: GET https://api.warframe.market/v1/items
    /// 
    /// Example JSON:
    /// {
    ///   "payload": {
    ///     "items": [
    ///       {
    ///         "item_name": "Energy Nexus",
    ///         "thumb": "https://...",
    ///         "url_name": "energy_nexus"
    ///       },
    ///       ...
    ///     ]
    ///   }
    /// }
    /// </summary>
    public class ModsResponse
    {
        /// <summary>
        /// The payload containing the actual mod list.
        /// Required for proper deserialization.
        /// </summary>
        [JsonPropertyName("payload")]
        public required ModsPayload Payload { get; set; }

        /// <summary>
        /// ModsPayload - Container for the list of mods/items.
        /// Acts as a wrapper around the actual mod list.
        /// </summary>
        public class ModsPayload
        {
            /// <summary>
            /// List of all available mods/items on the Warframe Market.
            /// Can contain 5000+ items depending on market updates.
            /// Used for populating the autocomplete search suggestions.
            /// </summary>
            [JsonPropertyName("items")]
            public required List<Mod> Mods { get; set; }
        }

        /// <summary>
        /// Mod - Represents a single mod/item available on the Warframe Market.
        /// Contains essential information for search and display purposes.
        /// </summary>
        public class Mod
        {
            /// <summary>
            /// Display name of the mod (human-readable).
            /// Example: "Energy Nexus", "Split Chamber", "Serration"
            /// This is what users see in the UI.
            /// </summary>
            [JsonPropertyName("item_name")]
            public required string ItemName { get; set; }

            /// <summary>
            /// URL to the mod's thumbnail image.
            /// Points to a CDN-hosted image on warframe.market
            /// Used for displaying small mod icons in lists.
            /// </summary>
            [JsonPropertyName("thumb")]
            public required string Thumb { get; set; }

            /// <summary>
            /// URL-safe identifier for the mod.
            /// Uses snake_case format: "energy_nexus", "split_chamber", "serration"
            /// IMPORTANT: Use this when making API calls to get orders or item details!
            /// Do NOT use ItemName for API calls - it won't work.
            /// </summary>
            [JsonPropertyName("url_name")]
            public required string UrlName { get; set; }
        }
    }
}
