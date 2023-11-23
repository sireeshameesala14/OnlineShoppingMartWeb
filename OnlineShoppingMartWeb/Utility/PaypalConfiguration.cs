using PayPal.Api;
using System;

namespace OnlineShoppingMartWeb.Utility
{
    public static class PaypalConfiguration
    {

        //Constructor
        static PaypalConfiguration()
        {
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                {"mode",mode }
            };
        }

        private static string GetAccessToken(string clientId, string clientSecret, string mode)
        {
            // getting accesstocken from paypal                
            string accessToken = new OAuthTokenCredential
        (clientId, clientSecret, GetConfig(mode)).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext(string clientId, string clientSecret, string mode)
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken(clientId, clientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;
        }
    }
}