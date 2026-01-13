using Warframe_Utils_.NET.Models.DTOS;
using static Warframe_Utils_.NET.Models.DTOS.ModsResponse;

namespace Warframe_Utils_.NET.Models.ViewModels
{
    /// <summary>
    /// WarframeHomeViewModel - Aggregates all data needed for the home page view.
    /// 
    /// This viewmodel is passed from the HomeController.Index action to the Index.cshtml view.
    /// It combines data from multiple sources:
    /// - Game status (Void Trader, cycles)
    /// - Complete mod list (for autocomplete)
    /// - Search results (specific mod found)
    /// - Trading orders for the searched mod
    /// - Detailed item information
    /// 
    /// Using a single viewmodel keeps the view code cleaner and simpler.
    /// The view has access to all data it needs without parameter complications.
    /// </summary>
    public class WarframeHomeViewModel
    {
        /// <summary>
        /// Game server status information from Warframe Status API.
        /// Contains:
        /// - Void Trader (Baroo) active status
        /// - Arbitration mission
        /// - Cetus cycle (day/night)
        /// - Vallis cycle (warm/cold)
        /// - Cambion cycle (Fass/Vome)
        /// 
        /// Always populated on every page load.
        /// Displayed in the status bar at the top of the page.
        /// </summary>
        public WarframeStatus Status { get; set; }

        /// <summary>
        /// Complete list of all available mods from Warframe Market.
        /// Contains thousands of items for the autocomplete search.
        /// 
        /// Always populated on every page load.
        /// Used by JavaScript to populate the search suggestions dropdown.
        /// Allows fuzzy searching as user types.
        /// </summary>
        public ModsResponse Mods { get; set; }

        /// <summary>
        /// The specific mod found from the user's search query.
        /// Null if:
        /// - User hasn't performed a search
        /// - Search returned no results
        /// 
        /// When populated, displays:
        /// - Mod name and icon
        /// - Description
        /// - Trading tax
        /// - Link to wiki
        /// </summary>
        public Mod ModFound { get; set; }

        /// <summary>
        /// Trading orders for the searched mod.
        /// Contains buy and sell orders from all players.
        /// 
        /// Null if:
        /// - No search was performed
        /// - Searched mod doesn't exist
        /// - API call to fetch orders failed
        /// 
        /// View filters this to show only:
        /// - Sell orders (not buy orders)
        /// - From online players (status = "ingame")
        /// - Visible/active orders
        /// 
        /// Sorted by: mod rank, then price (ascending).
        /// Displayed in a table showing player name, status, price, quantity, rank.
        /// </summary>
        public OrdersResponse? Orders { get; set; }

        /// <summary>
        /// Detailed item information for the searched mod.
        /// Contains:
        /// - Full description
        /// - Rarity
        /// - Trading tax
        /// - Icon URLs
        /// - Tags
        /// - Wiki link
        /// - Drop locations
        /// 
        /// Null if:
        /// - No search was performed
        /// - Searched mod doesn't exist
        /// - API call to fetch details failed
        /// 
        /// Used to populate the mod detail card displayed above the orders table.
        /// </summary>
        public ModDetailResponse? ModDetail { get; set; }
    }
}
