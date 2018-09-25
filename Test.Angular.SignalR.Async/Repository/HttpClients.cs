using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test.Angular.SignalR.Async.Model;

namespace Test.Angular.SignalR.Async
{
    public class HttpClients : IHttpClientRepository
    {
        private AppSettings appSettings;
        private HttpClient authClient;
        private HttpClient apiClient;
        private string token;

        public HttpClients(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            
            authClient = new HttpClient() { BaseAddress = new Uri(this.appSettings.TokenServer) };
            apiClient = new HttpClient() { BaseAddress = new Uri(this.appSettings.Server) };
            
            token = string.Empty;
        }

        public async Task<TokenResponse> GetTokenAsync()
        {
            var url = new Uri(new Uri(appSettings.TokenServer), "tokens/cloudpos");

            var body = new TokenRequest()
            {
                Username = appSettings.PinpadUsername,
                Password = appSettings.PinpadPassword,
                PairCode = appSettings.PinpadPairCode,
                PosName = appSettings.PosName,
                PosVersion = appSettings.PosVersion,
                PosId = new Guid(appSettings.PosId)
            };

            authClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var str = JsonConvert.SerializeObject(body);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(body), UTF8Encoding.UTF8, "application/json");

            var res = await authClient.PostAsync(url, content);

            if (res.IsSuccessStatusCode)
            {
                var tokenResponse = await res.Content.ReadAsAsync<TokenResponse>();
                token = tokenResponse?.Token;
                return tokenResponse;
            }

            return null;
        }

        public HttpClient GetAPIClient()
        {
            return apiClient;
        }

        public HttpClient GetAuthClient()
        {
            return authClient;
        }
    }
}
