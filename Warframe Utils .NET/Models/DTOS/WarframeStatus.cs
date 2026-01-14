using System.Text.Json.Serialization;

namespace Warframe_Utils_.NET.Models.ViewModels
{
    /// <summary>
    /// WarframeStatus - Aggregates all real-time game server status information.
    /// Maps to the JSON structure returned by: GET https://api.warframestat.us/pc
    /// 
    /// Contains information about:
    /// - Void Trader (Baroo) - When will the trader arrive/leave
    /// - Arbitration - Current mission type and location
    /// - Cetus Cycle - Day/night cycle for Plains of Eidolon
    /// - Vallis Cycle - Warm/cold cycle for Orb Vallis
    /// - Cambion Cycle - Fass/Vome cycle for Deimos
    /// 
    /// All data is real-time and updates throughout the day.
    /// Used to display the status bar at the top of the home page.
    /// 
    /// Example JSON Structure:
    /// {
    ///   "voidTrader": { "active": true },
    ///   "arbitration": { "node": "Corpus Gas City on Venus" },
    ///   "cetusCycle": { "isDay": true },
    ///   "vallisCycle": { "state": "warm" },
    ///   "cambionCycle": { "state": "fass" }
    /// }
    /// </summary>
    public class WarframeStatus
    {
        /// <summary>
        /// Void Trader (Baroo) status information.
        /// Baroo visits a different relay location periodically (every ~24 hours).
        /// Players need to know if Baroo is currently active.
        /// Active = currently available for trading
        /// </summary>
        [JsonPropertyName("voidTrader")]
        public VoidTrader BarooData { get; set; }

        /// <summary>
        /// Arbitration mission information.
        /// Arbitration is a high-difficulty mission type available daily.
        /// Players rotate it to find the mission node they want.
        /// Contains mission type and planet/location.
        /// Note: Currently stored but not displayed on the UI.
        /// </summary>
        [JsonPropertyName("arbitration")]
        public Arbitration ArbutrationData { get; set; }

        /// <summary>
        /// Cetus Cycle information (Plains of Eidolon).
        /// Plains of Eidolon has a 100-minute cycle alternating between day and night.
        /// Day is good for certain activities, night for others.
        /// Players need to know the current time of day.
        /// </summary>
        [JsonPropertyName("cetusCycle")]
        public CetusCycle CetusData { get; set; }

        /// <summary>
        /// Vallis Cycle information (Orb Vallis).
        /// Orb Vallis alternates between warm and cold periods.
        /// Warm = day-like conditions, Cold = night-like conditions
        /// Different enemies and activities available in each state.
        /// </summary>
        [JsonPropertyName("vallisCycle")]
        public VallisCycle VenusData { get; set; }

        /// <summary>
        /// Cambion Cycle information (Deimos).
        /// Deimos alternates between Fass (day) and Vome (night) cycles.
        /// Different enemies spawn and resources available in each state.
        /// Fass = day, Vome = night (like Cetus).
        /// </summary>
        [JsonPropertyName("cambionCycle")]
        public CambionCycle DeimosData { get; set; }

        /// <summary>
        /// VoidTrader - Void Trader (Baroo) status information.
        /// Baroo is a mysterious NPC who sells rare items periodically.
        /// Simple boolean to indicate if currently active.
        /// </summary>
        public class VoidTrader
        {
            /// <summary>
            /// Is the Void Trader currently active and available for trading?
            /// True = Baroo is currently at a relay, can trade
            /// False = Baroo will arrive in X hours, cannot trade yet
            /// 
            /// UI Display: "⚡ Active" (if true) or "❌ Not Active" (if false)
            /// </summary>
            [JsonPropertyName("active")]
            public bool isActive { get; set; }
        }

        /// <summary>
        /// Arbitration - Current arbitration mission information.
        /// Arbitration rotates daily at a specific time.
        /// Players use this to plan which mission to run.
        /// </summary>
        public class Arbitration
        {
            /// <summary>
            /// The current arbitration mission node and type.
            /// Format: "Enemy Type on Planet (Mission Type)"
            /// Example: "Corpus Gas City on Venus (Spy)"
            /// Tells players where to go for the daily arbitration.
            /// </summary>
            [JsonPropertyName("node")]
            public string node { get; set; }
        }

        /// <summary>
        /// CetusCycle - Plains of Eidolon day/night cycle.
        /// 100-minute cycle alternating between day and night.
        /// Day = good for Eidolon hunts, night = good for resource farming.
        /// </summary>
        public class CetusCycle
        {
            /// <summary>
            /// Is it currently daytime in Plains of Eidolon?
            /// True = Day (bright, Eidolons cannot be hunted)
            /// False = Night (dark, Eidolons can be hunted)
            /// 
            /// UI Display: "☀️ Day" (if true) or "🌙 Night" (if false)
            /// </summary>
            [JsonPropertyName("isDay")]
            public bool isDay { get; set; }

            /// <summary>
            /// When the current cycle expires (ISO 8601 format)
            /// Used to calculate time remaining in the current cycle
            /// </summary>
            [JsonPropertyName("expiry")]
            public string expiry { get; set; }
        }

        /// <summary>
        /// CambionCycle - Deimos day/night equivalent cycle.
        /// Alternates between Fass (day) and Vome (night) states.
        /// Different enemies and resources in each state.
        /// Similar to Cetus cycle but with different naming.
        /// </summary>
        public class CambionCycle
        {
            /// <summary>
            /// Current state of Deimos cycle.
            /// "fass" = day-like state (bright)
            /// "vome" = night-like state (dark)
            /// 
            /// UI Display: "☀️ Fass" (if state=="fass") or "🌙 Vome" (if state=="vome")
            /// </summary>
            [JsonPropertyName("state")]
            public string state { get; set; }
        }

        /// <summary>
        /// VallisCycle - Orb Vallis temperature/state cycle.
        /// Alternates between warm and cold environmental states.
        /// Different than day/night - it's about temperature and weather.
        /// Affects enemy types and available resources.
        /// </summary>
        public class VallisCycle
        {
            /// <summary>
            /// Current environmental state of Orb Vallis.
            /// "warm" = warm/hot period
            /// "cold" = cold/freezing period
            /// 
            /// UI Display: "🔥 Heat" (if state=="warm") or "❄️ Cold" (if state=="cold")
            /// </summary>
            [JsonPropertyName("state")]
            public string state { get; set; }

            /// <summary>
            /// When the current cycle expires (ISO 8601 format)
            /// Used to calculate time remaining in the current cycle
            /// </summary>
            [JsonPropertyName("expiry")]
            public string expiry { get; set; }
        }
    }
}
