using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;

namespace OnlineShoppingMartWeb.Utility
{
    public class ApiClient
    {

        public static string GetAjaxResponse(string requestUrl, string methodType, string postData)
        {
            string apiResponse = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Configuration.OsmApiUrl + requestUrl);
            request.Method = methodType;
            if (!string.IsNullOrEmpty(postData))
            {
                byte[] byte1 = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byte1.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);
                newStream.Close();
            }
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "basic " + Configuration.ApiAuthToken);
            WebResponse response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                apiResponse = reader.ReadToEnd();
            }
            return apiResponse;
        }
    }
}