using System.Text.Json.Serialization;

namespace SiparisAPI
{
    public class Product
    {
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

    }
}
