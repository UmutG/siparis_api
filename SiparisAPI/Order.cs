using System.Text.Json.Serialization;

namespace SiparisAPI
{
    public class Order
    {
        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public required int ItemId { get; set; }
        public int Unit { get; set; }
    }
}
