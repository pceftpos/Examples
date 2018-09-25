using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Helpers;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Controllers
{
    [Authorize(Policy = "AuthenticatedPosNotifyUser")]
    [Route("api/v1/home")]
    public class HomeController : Controller
    {
        private const int TRX_EXPIRATION_MINS = 3;
        private const int DOLLAR_TO_CENT = 100;
        private const int TRX_RND_STR_LENGTH = 16;
        private const int WAIT_PERIOD_SEC = 16;

        private AppSettings appSettings;
        private HttpClient authClient;
        private HttpClient apiClient;
        private string token;

        public HomeController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            authClient = new HttpClient() { BaseAddress = new Uri(this.appSettings.TokenServer) };
            apiClient = new HttpClient() { BaseAddress = new Uri(this.appSettings.Server) };
            token = string.Empty;
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <returns>Return token response</returns>
        [HttpGet("token")]
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

            if (res != null && res.IsSuccessStatusCode)
            {
                var tokenResponse = await res.Content.ReadAsAsync<TokenResponse>();
                token = tokenResponse?.Token;
                return tokenResponse;
            }

            return null;
        }

        /// <summary>
        /// Do pinpad logon
        /// </summary>
        /// <returns>Return logon response</returns>
        [HttpGet("pinpad/logon")]
        public async Task<ActionResult<EFTLogonResponse>> GetPinpadLogonAsync()
        {
            var sessionId = Guid.NewGuid();

            var url = appSettings.Server + $"sessions/{sessionId}/logon";

            var request = new ApiRequest<EFTLogonRequest>()
            {
                Request = new EFTLogonRequest()
                {                   
                    LogonType = " ",                    
                    ReceiptAutoPrint = "0",
                    CutReceipt = "0",
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application
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

            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(url, content);

            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponse<EFTLogonResponse>>();
                if (apiResponse != null && apiResponse.Response != null)
                {
                    return apiResponse.Response;
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Get pinpad status
        /// </summary>
        /// <returns>Return pinpad status response</returns>
        [HttpGet("pinpad/status")]
        public async Task<ActionResult<EFTStatusResponse>> GetPinpadStatusAsync()
        {
            var sessionId = Guid.NewGuid();

            var url = appSettings.Server + $"sessions/{sessionId}/status";

            var request = new ApiRequest<EFTStatusRequest>()
            {
                Request = new EFTStatusRequest()
                {
                    StatusType = "0",
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application
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

            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(url, content);

            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponse<EFTStatusResponse>>();
                if (apiResponse != null && apiResponse.Response != null)
                {
                    return apiResponse.Response;
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Make transaction
        /// </summary>
        /// <param name="amount">Amount of transaction</param>
        /// <returns>Return transaction response</returns>
        [HttpPost("transaction")]
        public async Task<ActionResult<EFTTransactionResponse>> PostTransactionAsync([FromBody] float amount)
        {
            var sessionId = Guid.NewGuid();

            var url = appSettings.Server + $"sessions/{sessionId}/transaction";

            var request = new ApiRequest<EFTTransactionRequest>()
            {
                Request = new EFTTransactionRequest()
                {
                    TxnType = "P",
                    TxnRef = RandomStr.RandomString(TRX_RND_STR_LENGTH),
                    AmtPurchase = (int)(amount * DOLLAR_TO_CENT),
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application
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

            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            try
            {
                var response = await apiClient.PostAsync(url, content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsAsync<ApiResponse<EFTTransactionResponse>>();
                    if (apiResponse != null && apiResponse.Response != null)
                    {
                        return apiResponse.Response;
                    }

                }
            }
            catch
            {
                using (var s = new TimedHostedService(sessionId, apiClient, token))
                {
                    using (var cts = new CancellationTokenSource(TRX_EXPIRATION_MINS))
                    {
                        await s.StartAsync(cts.Token);

                        EFTTransactionResponse txn = null;

                        while (txn == null)
                        {
                            txn = s.GetTransaction();
                            await Task.Delay(WAIT_PERIOD_SEC); 

                            if (cts.IsCancellationRequested)
                            {
                                await s.StopAsync(cts.Token);
                            }
                        }

                        await s.StopAsync(cts.Token);

                        if (txn == null || !txn.Success)
                        {
                            return BadRequest();
                        }

                        return txn;
                    }                    
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Process button click on pinpad notification 
        /// </summary>
        /// <param name="key">Button key</param>
        /// <returns></returns>
        [HttpPost("sendkey")]
        public async Task PostSendKeyAsync([FromBody] EFTSendKeyRequest key)
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

            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(url, content);

            if (response != null && response.IsSuccessStatusCode)
            {
                return;
            }

        }
    }
}