namespace Warframe_Utils_.NET.Models.DTOS;
using System.Text.Json.Serialization;
using System.Collections.Generic;

public class ModDetailResponse
{
    [JsonPropertyName("payload")]
    public ModDetailPayload? Payload { get; set; }
}

public class ModDetailPayload
{
    [JsonPropertyName("item")]
    public ModDetailItem? Item { get; set; }
}

public class ModDetailItem
{

    [JsonPropertyName("items_in_set")]
    public List<ModDetailSetItem>? ItemsInSet { get; set; }
}

public class ModDetailSetItem
{
    [JsonPropertyName("url_name")]
    public string? UrlName { get; set; }

    [JsonPropertyName("rarity")]
    public string? Rarity { get; set; }

    [JsonPropertyName("thumb")]
    public string? Thumb { get; set; }

    [JsonPropertyName("trading_tax")]
    public int TradingTax { get; set; }

    [JsonPropertyName("sub_icon")]
    public string? SubIcon { get; set; }

    [JsonPropertyName("mod_max_rank")]
    public int ModMaxRank { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    [JsonPropertyName("en")]
    public ModDetailLocalization? En { get; set; }
}

public class ModDetailLocalization
{
    [JsonPropertyName("item_name")]
    public string? ItemName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("wiki_link")]
    public string? WikiLink { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("thumb")]
    public string? Thumb { get; set; }

    [JsonPropertyName("drop")]
    public List<string>? Drop { get; set; }
}
