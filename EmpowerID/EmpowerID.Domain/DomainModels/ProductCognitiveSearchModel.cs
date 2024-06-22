using System.Text.Json.Serialization;

namespace EmpowerID.Domain.DomainModels
{
    public class ProductSearchModel
    {
        [JsonPropertyName("product_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("product_name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = string.Empty;
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("date_added")]
        public DateTime DateAdded { get; set; }
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }
        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; } = string.Empty;
    }
}