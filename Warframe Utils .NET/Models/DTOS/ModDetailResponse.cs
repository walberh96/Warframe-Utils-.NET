namespace Warframe_Utils_.NET.Models.DTOS;
using System.Text.Json.Serialization;
using System.Collections.Generic;

/// <summary>
/// ModDetailResponse - Top-level container for Warframe Market item detail API response.
/// Maps to the JSON structure returned by: GET https://api.warframe.market/v2/item/{slug}
/// 
/// Contains comprehensive information about a specific item including:
/// - Item description and stats
/// - Rarity and trading costs
/// - Icon URLs for display
/// - Tags and categorization
/// - Wiki links for additional information
/// </summary>
public class ModDetailResponse
{
    /// <summary>
    /// The payload containing the actual item details.
    /// </summary>
    [JsonPropertyName("payload")]
    public ModDetailPayload? Payload { get; set; }

    /// <summary>
    /// ModDetailPayload - Container for the item information.
    /// </summary>
    public class ModDetailPayload
    {
        /// <summary>
        /// The actual item object containing all variants and details.
        /// </summary>
        [JsonPropertyName("item")]
        public ModDetailItem? Item { get; set; }

        /// <summary>
        /// ModDetailItem - Contains all variants/ranks of a specific item.
        /// </summary>
        public class ModDetailItem
        {
            /// <summary>
            /// List of all variants/ranks for this item.
            /// The application uses the first entry ([0]) by default.
            /// </summary>
            [JsonPropertyName("items_in_set")]
            public List<ModDetailSetItem>? ItemsInSet { get; set; }

            /// <summary>
            /// ModDetailSetItem - Individual item variant with complete metadata.
            /// </summary>
            public class ModDetailSetItem
            {
                /// <summary>
                /// URL-safe identifier for this item variant.
                /// Example: "energy_nexus", "split_chamber"
                /// </summary>
                [JsonPropertyName("url_name")]
                public string? UrlName { get; set; }

                /// <summary>
                /// Item rarity level affecting trading value.
                /// </summary>
                [JsonPropertyName("rarity")]
                public string? Rarity { get; set; }

                /// <summary>
                /// URL to the item's thumbnail image.
                /// Points to a CDN-hosted image on warframe.market
                /// </summary>
                [JsonPropertyName("thumb")]
                public string? Thumb { get; set; }

                /// <summary>
                /// NPC trading cost in Platinum (in-game currency).
                /// </summary>
                [JsonPropertyName("trading_tax")]
                public int TradingTax { get; set; }

                /// <summary>
                /// Secondary icon URL.
                /// </summary>
                [JsonPropertyName("sub_icon")]
                public string? SubIcon { get; set; }

                /// <summary>
                /// Maximum rank/level for this mod.
                /// </summary>
                [JsonPropertyName("mod_max_rank")]
                public int ModMaxRank { get; set; }

                /// <summary>
                /// URL to the full-size item icon/image.
                /// Used for displaying in the detail card.
                /// </summary>
                [JsonPropertyName("icon")]
                public string? Icon { get; set; }

                /// <summary>
                /// List of category tags for this item.
                /// Examples: ["melee", "tenno"], ["rifle", "primacy"]
                /// </summary>
                [JsonPropertyName("tags")]
                public List<string>? Tags { get; set; }

                /// <summary>
                /// English language details for this item.
                /// Contains the description, name, wiki link, and drop locations.
                /// </summary>
                [JsonPropertyName("en")]
                public ModDetailLocalization? En { get; set; }

                /// <summary>
                /// ModDetailLocalization - English language text and links for an item.
                /// Contains all human-readable information about the item.
                /// </summary>
                public class ModDetailLocalization
                {
                    /// <summary>
                    /// Display name of the item.
                    /// Example: "Energy Nexus", "Split Chamber", "Serration"
                    /// </summary>
                    [JsonPropertyName("item_name")]
                    public string? ItemName { get; set; }

                    /// <summary>
                    /// Item description including stats and effects.
                    /// </summary>
                    [JsonPropertyName("description")]
                    public string? Description { get; set; }

                    /// <summary>
                    /// Link to the official Warframe Wiki page for this item.
                    /// </summary>
                    [JsonPropertyName("wiki_link")]
                    public string? WikiLink { get; set; }

                    /// <summary>
                    /// URL to the full-size icon.
                    /// </summary>
                    [JsonPropertyName("icon")]
                    public string? Icon { get; set; }

                    /// <summary>
                    /// URL to the thumbnail image.
                    /// </summary>
                    [JsonPropertyName("thumb")]
                    public string? Thumb { get; set; }

                    /// <summary>
                    /// List of locations where this item can be obtained (farmed/dropped).
                    /// </summary>
                    [JsonPropertyName("drop")]
                    public List<string>? Drop { get; set; }
                }
            }
        }
    }
}
