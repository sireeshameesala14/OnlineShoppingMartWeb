using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OnlineShoppingMartWeb.ResponseModels
{
    [DataContract]
    public class CheckoutSaveResponse
    {
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }

        [JsonPropertyName("transectionId")]
        public long TransectionId { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("orderStatus")]
        public string OrderStatus { get; set; }

        [JsonPropertyName("shippingStatus")]
        public string ShippingStatus { get; set; }

        [JsonPropertyName("paymentMode")]
        public string PaymentMode { get; set; }

        [JsonPropertyName("isPaymentSuccessful")]
        public bool IsPaymentSuccessful { get; set; }

        [JsonPropertyName("totalOrderPrice")]
        public decimal TotalOrderPrice { get; set; }
    }
}