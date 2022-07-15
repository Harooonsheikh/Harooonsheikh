using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Common
{
    public class HttpClientService
    {
        public async Task<string> GetAsync(string url, string authKey = "")
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(authKey))
                    client.DefaultRequestHeaders.Add("Authorization", authKey);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;

                return await client.GetStringAsync(url);
            }
        }

        public async Task<string> PostAsync<T>(string url, T model, string authKey = "")
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(authKey))
                    client.DefaultRequestHeaders.Add("Authorization", authKey);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;

                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                var response = client.PostAsync(url, content).Result;

                if (response.Content != null)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return "";
        }

        public async Task<string> PutAsync<T>(string url, T model, string authKey = "")
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(authKey))
                    client.DefaultRequestHeaders.Add("Authorization", authKey);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;

                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                var response = await client.PutAsync(url, content);

                if (response.Content != null)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return "";
        }
    }
}
