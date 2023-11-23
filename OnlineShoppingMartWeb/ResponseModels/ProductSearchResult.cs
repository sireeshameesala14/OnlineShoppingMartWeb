using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OnlineShoppingMartWeb.ResponseModels
{
    [DataContract]
    public class ProductSearchResult
    {
        [JsonPropertyName("productId")]
        public long ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [JsonPropertyName("productType")]
        public string ProductType { get; set; }

        [JsonPropertyName("productPrice")]
        public decimal ProductPrice { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("productColor")]
        public string ProductColor { get; set; }

        [JsonPropertyName("images")]
        public List<ProductImagesModel> Images { get; set; }

        [JsonPropertyName("productReviews")]
        public List<ProductReviewModel> ProductReviews { get; set; }
    }
}