using Microsoft.AspNetCore.DataProtection.KeyManagement;
using OnlineShoppingMartWeb.ResponseModels;
using System.Text.Json;

namespace OnlineShoppingMartWeb.Utility
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
        public static void AddWithTimeout(this ISession session,
        string name,
        string value,
        TimeSpan expireAfter)
        {
            lock (session)
            {
                session.SetString(name, value);
            }

            //add cleanup task that will run after "expire"
            Task.Delay(expireAfter).ContinueWith((task) => {
                lock (session)
                {
                    session.Remove(name);
                }
            });
        }
    }
}