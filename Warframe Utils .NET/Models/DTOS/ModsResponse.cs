using System.Text.Json.Serialization;
namespace Warframe_Utils_.NET.Models.DTOS
{
    public class ModsResponse
    {
        [JsonPropertyName("payload")]
        public ModsPayload Payload { get; set; }

        public class ModsPayload
        {
            [JsonPropertyName("items")]
            public List<Mod> Mods { get; set; }
        }

        public class Mod
        {
            [JsonPropertyName("item_name")]
            public string ItemName { get; set; }

            [JsonPropertyName("thumb")]
            public string Thumb { get; set; }

            [JsonPropertyName("url_name")]
            public string UrlName { get; set; }

        }
    }

}
