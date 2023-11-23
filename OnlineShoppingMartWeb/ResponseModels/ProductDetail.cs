using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OnlineShoppingMartWeb.ResponseModels
{
    [DataContract]
    public class ProductDetail
    {
        [JsonPropertyName("productId")]
        public long ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [JsonPropertyName("productType")]
        public string ProductType { get; set; }

        [JsonPropertyName("productCatageory")]
        public string ProductCategory { get; set; }

        [JsonPropertyName("productCount")]
        public int ProductCount { get; set; }

        [JsonPropertyName("productSpecification")]
        public string ProductSpecification { get; set; }

        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }

        [JsonPropertyName("shipingCharge")]
        public decimal ShippingCharge { get; set; }

        [JsonPropertyName("productPrice")]
        public decimal ProductPrice { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("productColor")]
        public List<string> ProductColor { get; set; }

        [JsonPropertyName("images")]
        public List<ProductImagesModel> Images { get; set; }

        [JsonPropertyName("productReviews")]
        public List<ProductReviewModel> ProductReviews { get; set; }
    }
}