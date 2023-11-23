using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OnlineShoppingMartWeb.ResponseModels
{
    [DataContract]
    public class UserAuhDetail
    {
        [JsonPropertyName("userId")]
        public long UserId { get; set; }

        [JsonPropertyName("isAuthenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonPropertyName("userType")]
        public string UserType { get; set; }

        public string UserName { get; set; }
    }
}