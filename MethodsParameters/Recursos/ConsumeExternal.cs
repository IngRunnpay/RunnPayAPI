using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;

namespace ECD.Utilidades.Recursos
{
    public class ConsumeExternal
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
        public async Task<T> PostAsync<T>(string url, object request, string username = null, string password = null)
        {
            CreateNewInstanClient();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            }
            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);
            using (var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json"))
            {
                var response = new System.Net.Http.HttpResponseMessage();
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    response = Client.PostAsync(Client.BaseAddress, stringContent).Result;
                }
                catch (Exception ex)
                {
                    string[] linesIni4 = { "Exception", "Exception: " + ex.ToString() };
                    using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(System.IO.Path.Combine("C:/Logs", "consumeExternalException.txt")))
                    {
                        foreach (string line in linesIni4)
                            outputFile.WriteLine(line);
                    }
                }
                if (!response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
        public async Task<T> RestBearer<T>(string url)
        {
            CreateNewInstanClient();
            Client.BaseAddress = new Uri(url);

            using (var stringContent = new StringContent("", Encoding.UTF8, "application/json"))
            {
                var response = Client.PostAsync(Client.BaseAddress, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(obj);
                }
                var responseString = response.Content.ReadAsStringAsync().Result;
                var responseEnd = JsonConvert.DeserializeObject<T>(responseString);
                return responseEnd;
            }
        }
        public async Task<T> GetAsync<T>(string url, object request, string accessToken = null)
        {
            try
            {
                CreateNewInstanClient();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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
