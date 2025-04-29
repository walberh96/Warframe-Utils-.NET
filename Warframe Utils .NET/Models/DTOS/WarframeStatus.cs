using System.Text.Json.Serialization;

namespace Warframe_Utils_.NET.Models.ViewModels
{
    public class WarframeStatus
    {
        //IS BAROO AROUND
        [JsonPropertyName("voidTrader")]
        public VoidTrader BarooData { get; set; }
        // ARBITRATION TYPE AND TIME LEFT
        [JsonPropertyName("arbitration")]
        public Arbitration ArbutrationData { get; set; }
        // CETUS CURRENT TIME
        [JsonPropertyName("cetusCycle")]
        public CetusCycle CetusData { get; set; }
        // VENUS CURRENT TIME
        [JsonPropertyName("vallisCycle")]
        public VallisCycle VenusData { get; set; }
        // DEIMOS CURRENT TIME
        [JsonPropertyName("cambionCycle")]
        public CambionCycle DeimosData { get; set; }

        public class VoidTrader
        {
            [JsonPropertyName("active")]
            public bool isActive { get; set; }
        }
        public class Arbitration
        {
            [JsonPropertyName("node")]
            public string node { get; set; }
        }
        public class CetusCycle {
            [JsonPropertyName("isDay")]
            public bool isDay { get; set; }
        }
        public class CambionCycle {
            [JsonPropertyName("state")]
            public string state { get; set; }
        }
        public class VallisCycle
        {
            [JsonPropertyName("state")]
            public string state { get; set; }
        }
    }
}
