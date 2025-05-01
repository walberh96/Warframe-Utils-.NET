using Warframe_Utils_.NET.Models.DTOS;
using static Warframe_Utils_.NET.Models.DTOS.ModsResponse;

namespace Warframe_Utils_.NET.Models.ViewModels
{
    public class WarframeHomeViewModel
    {
        public WarframeStatus Status { get; set; }
        public ModsResponse Mods { get; set; }
        public Mod ModFound { get; set; }
        public OrdersResponse? Orders { get; set; }
        public ModDetailResponse? ModDetail { get; set; }
    }
}
