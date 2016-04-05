using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TwitterBot
{
    public class LuisClient
    {
        private static string url = String.Format("https://api.projectoxford.ai/luis/v1/application?id={0}&subscription-key={1}",
                            ConfigurationManager.AppSettings["luisApplicationId"], ConfigurationManager.AppSettings["luisSubscriptionKey"]);


        public static async Task<LuisResponse> GetLuisResponse(string message)
        {
            string query = "&q=" + message;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url + query);
                return await response.Content.ReadAsAsync<LuisResponse>();
            }
        }
    }
}