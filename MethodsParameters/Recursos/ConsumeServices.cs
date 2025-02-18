using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ECD.Utilidades.Recursos
{
    public class ConsumeServices
    {
        private HttpClient Client;

        public async Task<T> GetAsync<T>(string url, object request)
        {
            CreateNewInstanClient();

            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);

            using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
            {
                var response = Client.GetAsync(Client.BaseAddress).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }

                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
        public async Task<T> PostAsync<T>(string url, object request, string username = null, string password = null, string channel = null)
        {
            CreateNewInstanClient();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            }
            if (!string.IsNullOrEmpty(channel))
            {
                Client.DefaultRequestHeaders.Add("ChannelAuthorization", "Authorization " + channel);
            }
            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);

            using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
            {
                var response = Client.PostAsync(Client.BaseAddress, stringContent).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }

                var responseString = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(responseString);
            }

        }
        public async Task<T> RestBearer<T>(string url, object request, string token = null, string channel = null)
        {
            CreateNewInstanClient();
            if (!string.IsNullOrEmpty(token))
            {
                if (string.IsNullOrEmpty(channel))
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                else
                {
                    Client.DefaultRequestHeaders.Add("Tokencustomer", "Authorization " + token);
                    Client.DefaultRequestHeaders.Add("ChannelAuthorization", "Authorization " + channel);
                }
            }

            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);

            using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
            {
                var response = Client.PostAsync(Client.BaseAddress, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(obj);
                }
                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
        public async Task<T> GetAsync<T>(string url, object request, string accessToken = null)
        {
            try
            {
                CreateNewInstanClient();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);
                }

                Client.BaseAddress = new Uri(url);
                var jsonRequest = JsonConvert.SerializeObject(request);

                using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
                {
                    var response = Client.GetAsync(Client.BaseAddress).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    }

                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<T>(responseString);

                    return result;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        private void CreateNewInstanClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Client = new HttpClient(clientHandler);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
        }
    }
}
