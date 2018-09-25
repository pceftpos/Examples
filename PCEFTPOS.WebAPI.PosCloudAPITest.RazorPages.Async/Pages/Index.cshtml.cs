using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Helpers;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Interface;
using Newtonsoft.Json.Linq;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Pages
{
    public class IndexModel : PageModel
    {
        private const int TRX_EXPIRATION_MINS = 3;
        private const int DOLLAR_TO_CENT = 100;
        private const int TRX_RND_STR_LENGTH = 16;

        /// <summary>
        /// App settings
        /// </summary>
        private AppSettings appSettings;

        /// <summary>
        /// Session repository
        /// </summary>
        private ISessionRepository sessionRepository;

        /// <summary>
        /// Http clients repository
        /// </summary>
        private IHttpClientRepository clients;

        /// <summary>
        /// Token
        /// </summary>
        private string token;

        public IndexModel(IOptions<AppSettings> appSettings, ISessionRepository sessionRepository, IHttpClientRepository clients)
        {
            token = string.Empty;

            this.appSettings = appSettings.Value;
            this.sessionRepository = sessionRepository;
            this.clients = clients;
            token = string.Empty;
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <returns>Return token response</returns>
        public async Task<TokenResponse> GetTokenAsync()
        {
            var authClient = clients.GetAuthClient();

            var url = new Uri(new Uri(appSettings.TokenServer), "tokens/cloudpos");
            var body = new TokenRequest()
            {
                PosId = new Guid(appSettings.PosId),
                PosName = appSettings.PosName,
                PosVersion = appSettings.PosVersion,
                Username = appSettings.PinpadUsername,
                Password = appSettings.PinpadPassword,
                PairCode = appSettings.PinpadPairCode
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

        /// <summary>
        /// Make transaction
        /// </summary>
        /// <param name="amount">Amount of transaction</param>
        public async Task OnPostDoTransactionAsync(string amount)
        {
            var sessionId = Guid.NewGuid();
            var actualAmount = float.Parse(amount);

            var url = appSettings.Server + $"sessions/{sessionId}/transaction?async=true";

            // Create a sample json 'basket' object which is sent as part of transaction request 
            JObject basket = JObject.FromObject(
                new
                {
                    id = (Guid.NewGuid()).ToString(),
                    amt = 18700,
                    tax = 1760,
                    dis = 650,
                    sur = 374,
                    items = new dynamic[] 
                    {
                        new { id = "t39kq002", sku = "k24086723", qty = 2, amt = 2145, tax = 200, dis = 50, name = "XData USB Drive" },
                        new { id = "t39kq003", sku = "s23475697", qty = 1, amt = 8910, tax = 810, name = "MSoft OSuite", srl = "ms7843k346j23" },
                        new { id = "t39kq004", sku = "m47060855", qty = 5, amt = 1100, tax = 110, dis = 110, name = "A4 Notepad"}
                    }
                }
            );

            var request = new ApiRequest<EFTTransactionRequest>()
            {
                Request = new EFTTransactionRequest()
                {
                    TxnType = "P",
                    TxnRef = RandomStr.RandomString(TRX_RND_STR_LENGTH),
                    AmtPurchase = (int)(actualAmount * DOLLAR_TO_CENT),
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application,
                    Basket = basket
                },
                Notification = new Notification
                {
                    Uri = appSettings.NotificationUri
                }
            };
            
            if (string.IsNullOrEmpty(token))
            {
                token = (await GetTokenAsync())?.Token;
            }

            var apiClient = clients.GetAPIClient();
            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            try
            {
                var activeTxn = new ActiveSession() { SessionId = sessionId.ToString(), Expire = DateTime.Now.AddMinutes(TRX_EXPIRATION_MINS).Ticks };
                await sessionRepository.AddSessionAsync(activeTxn);

                var response = await apiClient.PostAsync(url, content);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Process button click on pinpad notification 
        /// </summary>
        /// <param name="key">Button key</param>
        public async Task OnPostSendKeyAsync([FromBody] EFTSendKeyRequest key)
        {
            if (key == null)
            {
                return;
            }

            var url = appSettings.Server + $"sessions/{key.SessionId}/sendkey";

            var request = new ApiRequest<EFTSendKeyRequest>()
            {
                Request = new EFTSendKeyRequest()
                {
                    Data = key.Data,
                    Key = key.Key
                },
                Notification = new Notification
                {
                    Uri = appSettings.NotificationUri
                }
            };
            
            if (string.IsNullOrEmpty(token))
            {
                token = (await GetTokenAsync())?.Token;
            }

            var apiClient = clients.GetAPIClient();
            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(url, content);
        }
    }
}
