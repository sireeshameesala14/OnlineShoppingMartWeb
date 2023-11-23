namespace OnlineShoppingMartWeb.Models
{
    public class UpdateTransectionDetail
    {
        public long TransectionId { get; set; }
        public string GatewayName { get; set; }
        public bool IsPaymentSuccessful { get; set; }
        public string PayerId { get; set; }
        public string PaymentId { get; set; }
        public string RefrenceId { get; set; }

    }
}