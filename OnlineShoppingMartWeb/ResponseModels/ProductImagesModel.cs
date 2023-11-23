using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OnlineShoppingMartWeb.ResponseModels
{
    [DataContract]
    public class ProductImagesModel
    {
        [JsonPropertyName("imageSize")]
        public string ImageSize { get; set; }

        [JsonPropertyName("imageName")]
        public string ImageName { get; set; }
    }
}
