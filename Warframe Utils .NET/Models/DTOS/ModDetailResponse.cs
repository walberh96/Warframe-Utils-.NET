namespace Warframe_Utils_.NET.Models.DTOS;
using System.Text.Json.Serialization;
using System.Collections.Generic;

/// <summary>
/// ModDetailResponse - Top-level container for Warframe Market item detail API response.
/// Maps to the JSON structure returned by: GET https://api.warframe.market/v1/items/{itemId}
/// 
/// Contains comprehensive information about a specific item including:
/// - Item description and stats
/// - Rarity and trading costs
/// - Icon URLs for display
/// - Tags and categorization
/// - Where to farm/drop locations
/// - Wiki links for additional information
/// 
/// Example JSON Structure:
/// {
///   "payload": {
///     "item": {
///       "items_in_set": [
///         {
///           "url_name": "energy_nexus",
///           "rarity": "Rare",
///           "icon": "https://...",
///           "trading_tax": 8000,
///           "mod_max_rank": 5,
///           "tags": ["tenno", "attack"],
///           "en": {
///             "item_name": "Energy Nexus",
///             "description": "Grants +...",
///             "wiki_link": "https://...",
///             "drop": ["Void Fissure", "..."]
///           }
///         }
///       ]
///     }
///   }
/// }
/// </summary>
public class ModDetailResponse
{
    /// <summary>
    /// The payload containing the actual item details.
    /// Required for proper deserialization.
    /// </summary>
    [JsonPropertyName("payload")]
    public ModDetailPayload? Payload { get; set; }
}

/// <summary>
/// ModDetailPayload - Container for the item information.
/// Wraps the item for API consistency.
/// </summary>
public class ModDetailPayload
{
    /// <summary>
    /// The actual item object containing all variants and details.
    /// Most items have multiple ranks/variants in the items_in_set.
    /// </summary>
    [JsonPropertyName("item")]
    public ModDetailItem? Item { get; set; }
}

/// <summary>
/// ModDetailItem - Contains all variants/ranks of a specific item.
/// An item can have multiple variants (e.g., unranked to max rank mods).
/// </summary>
public class ModDetailItem
{
    /// <summary>
    /// List of all variants/ranks for this item.
    /// Typically, the list contains one entry for the base item.
    /// Each entry represents a different rank or version of the item.
    /// The application uses the first entry ([0]) by default.
    /// </summary>
    [JsonPropertyName("items_in_set")]
    public List<ModDetailSetItem>? ItemsInSet { get; set; }
}

/// <summary>
/// ModDetailSetItem - Individual item variant with complete metadata.
/// Contains all information needed to display the item in the UI.
/// </summary>
public class ModDetailSetItem
{
    /// <summary>
    /// URL-safe identifier for this item variant.
    /// Example: "energy_nexus", "split_chamber"
    /// Use this when making API calls.
    /// </summary>
    [JsonPropertyName("url_name")]
    public string? UrlName { get; set; }

    /// <summary>
    /// Item rarity level affecting trading value.
    /// Possible values: "Common", "Uncommon", "Rare", "Legendary", etc.
    /// Rarer items are generally more expensive.
    /// Can be displayed with visual indicators (color codes).
    /// </summary>
    [JsonPropertyName("rarity")]
    public string? Rarity { get; set; }

    /// <summary>
    /// URL to the item's thumbnail image.
    /// Points to a CDN-hosted image on warframe.market
    /// Smaller version of the full icon, used in lists.
    /// </summary>
    [JsonPropertyName("thumb")]
    public string? Thumb { get; set; }

    /// <summary>
    /// NPC trading cost in Platinum (in-game currency).
    /// This is what it costs to buy from NPCs.
    /// Players often buy items cheaper on the market.
    /// Example: 8000 platinum for Energy Nexus
    /// </summary>
    [JsonPropertyName("trading_tax")]
    public int TradingTax { get; set; }

    /// <summary>
    /// Secondary icon URL (used in certain contexts).
    /// Sometimes provides an alternative icon representation.
    /// May be similar or different from the main icon.
    /// </summary>
    [JsonPropertyName("sub_icon")]
    public string? SubIcon { get; set; }

    /// <summary>
    /// Maximum rank/level for this mod.
    /// Mods can typically be ranked from 0 to this value.
    /// Example: 5 means the mod can be ranked 0-5 (6 ranks total)
    /// Higher rank mods have better stats but cost more to rank up.
    /// </summary>
    [JsonPropertyName("mod_max_rank")]
    public int ModMaxRank { get; set; }

    /// <summary>
    /// URL to the full-size item icon/image.
    /// Points to a CDN-hosted image on warframe.market
    /// Used for displaying in the detail card.
    /// Higher resolution than thumb.
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    /// <summary>
    /// List of category tags for this item.
    /// Examples: ["melee", "tenno"], ["rifle", "primacy"], etc.
    /// Used for categorization and filtering.
    /// Helps players understand what the item is used for.
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    /// <summary>
    /// English language details for this item.
    /// Contains the description, name, wiki link, and drop locations.
    /// </summary>
    [JsonPropertyName("en")]
    public ModDetailLocalization? En { get; set; }
}

/// <summary>
/// ModDetailLocalization - English language text and links for an item.
/// Contains all human-readable information about the item.
/// </summary>
public class ModDetailLocalization
{
    /// <summary>
    /// Display name of the item.
    /// Example: "Energy Nexus", "Split Chamber", "Serration"
    /// This is what players call the item in-game.
    /// </summary>
    [JsonPropertyName("item_name")]
    public string? ItemName { get; set; }

    /// <summary>
    /// Item description including stats and effects.
    /// Example: "Increases Exilus Weapon Fire Rate by 15%"
    /// Contains important gameplay information.
    /// Displayed in the mod detail card on the UI.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Link to the official Warframe Wiki page for this item.
    /// Provides additional community-curated information.
    /// Players can click this for comprehensive guides.
    /// Example: "https://wiki.warframe.com/w/Energy Nexus"
    /// </summary>
    [JsonPropertyName("wiki_link")]
    public string? WikiLink { get; set; }

    /// <summary>
    /// URL to the full-size icon (similar to parent icon property).
    /// Redundant with ModDetailSetItem.Icon in most cases.
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    /// <summary>
    /// URL to the thumbnail image (similar to parent thumb property).
    /// Redundant with ModDetailSetItem.Thumb in most cases.
    /// </summary>
    [JsonPropertyName("thumb")]
    public string? Thumb { get; set; }

    /// <summary>
    /// List of locations where this item can be obtained (farmed/dropped).
    /// Examples: ["Void Fissure", "Orokin Void", "Lua", "Enemy: Charger"]
    /// Helps players find where to get the item.
    /// May contain mission types, enemies, or special locations.
    /// Useful for farming planning.
    /// </summary>
    [JsonPropertyName("drop")]
    public List<string>? Drop { get; set; }
}
