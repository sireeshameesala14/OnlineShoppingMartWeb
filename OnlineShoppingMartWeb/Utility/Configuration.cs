namespace OnlineShoppingMartWeb.Utility
{
    public class Configuration
    {
        public static string OsmApiUrl { get; set; }

        public static string ApiAuthToken { get; set; }
        public static decimal ShippingCharge { get; set; }
        public static string PaypalClientId { get; set; }

        public static string PaypalClientSecret { get; set; }

        public static string PaypalMode { get; set; }
    }
}