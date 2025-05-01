using System.Text.Json.Serialization;

public class OrdersResponse
{
    [JsonPropertyName("payload")]
    public OrdersPayload Payload { get; set; }

    public class OrdersPayload
    {
        [JsonPropertyName("orders")]
        public List<Order>? Orders { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("platinum")]
        public int Price { get; set; }

        [JsonPropertyName("order_type")]
        public string? OrderType { get; set; }

        [JsonPropertyName("user")]
        public User? User { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }
        [JsonPropertyName("mod_rank")]
        public int ModRank { get; set; }
    }

    public class User
    {
        [JsonPropertyName("ingame_name")]
        public string? InGameName { get; set; }

        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
