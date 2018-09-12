using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data.Interface;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Helpers;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Controllers
{
    [Authorize(Policy = "AuthenticatedPosNotifyUser")]
    [Route("api/v1/home")]
    public class HomeController : Controller
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

        public HomeController(IOptions<AppSettings> appSettings, ISessionRepository sessionRepository, IHttpClientRepository clients)
        {
            this.appSettings = appSettings.Value;
            this.sessionRepository = sessionRepository;
            this.clients = clients;
            token = string.Empty;
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <returns>Return token response</returns>
        [HttpGet("token")]
        public async Task<TokenResponse> GetTokenAsync()
        {
            var url = appSettings.TokenServer + "tokens/cloudpos";
            var body = new TokenRequest()
            {
                Username = appSettings.PinpadUsername,
                Password = appSettings.PinpadPassword,
                PairCode = appSettings.PinpadPairCode,
            };

            var authClient = clients.GetAuthClient();

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

            var url = appSettings.Server + $"sessions/{sessionId}/logon?async=true";

            var request = new ApiRequest<EFTLogonRequest>()
            {
                Request = new EFTLogonRequest()
                {                   
                    LogonType = " ",                    
                    ReceiptPrintMode = "0",
                    ReceiptCutMode = "0",
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application,
                    PosName = appSettings.PosName,
                    PosVersion = appSettings.PosVersion
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

            if (response != null && response.IsSuccessStatusCode)
            {
                return Accepted();
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

            var url = appSettings.Server + $"sessions/{sessionId}/status?async=true";

            var request = new ApiRequest<EFTStatusRequest>()
            {
                Request = new EFTStatusRequest()
                {
                    StatusType = "0",
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application,                    
                    PosName = appSettings.PosName,
                    PosVersion = appSettings.PosVersion
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

            if (response != null && response.IsSuccessStatusCode)
            {
                return Accepted();
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

            var url = appSettings.Server + $"sessions/{sessionId}/transaction?async=true";

            var request = new ApiRequest<EFTTransactionRequest>()
            {
                Request = new EFTTransactionRequest()
                {
                    TxnType = "P",
                    TxnRef = RandomStr.RandomString(TRX_RND_STR_LENGTH),
                    AmtPurchase = (int)(amount * DOLLAR_TO_CENT),
                    Merchant = appSettings.Merchant,
                    Application = appSettings.Application,
                    PosName = appSettings.PosName,
                    PosVersion = appSettings.PosVersion
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
                var actriveTxn = new ActiveSession() { SessionId = sessionId.ToString(), Expire = DateTime.Now.AddMinutes(TRX_EXPIRATION_MINS).Ticks };
                await sessionRepository.AddSessionAsync(actriveTxn);

                var response = await apiClient.PostAsync(url, content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    return Accepted(value: JsonConvert.SerializeObject(sessionId));
                }
            }
            catch
            {
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
                    Key = key.Key,
                    PosName = appSettings.PosName,
                    PosVersion = appSettings.PosVersion
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

            if (response != null && response.IsSuccessStatusCode)
            {
                return;
            }

        }
    }
}