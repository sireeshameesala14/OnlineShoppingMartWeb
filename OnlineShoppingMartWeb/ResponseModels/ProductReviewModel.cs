using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OnlineShoppingMartWeb.ResponseModels
{
    [DataContract]
    public class ProductReviewModel
    {
        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("userId")]
        public long UserId { get; set; }

        [JsonPropertyName("reviewComments")]
        public string ReviewComments { get; set; }
    }
}