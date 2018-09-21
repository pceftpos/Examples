using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.SignalR;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Interface;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Helpers
{
    /// <summary>
    /// 10 seconds background processor to check if there is any expired session(not finished transaction) and check it's status
    /// </summary>
    internal class GetTransactionProcessor : IHostedService
    {
        private const int TRX_RESPONSE_TIMEOUT_MINS = 3;
        private const int TRX_FAILURE_PING_SECS = 10;

        /// <summary>
        /// Sessions repository
        /// </summary>
        private ISessionRepository sessionRepository;

        /// <summary>
        /// SignalR hub
        /// </summary>
        private IHubContext<NotifyHub> notifyHub;

        /// <summary>
        /// Clients repository
        /// </summary>
        private IHttpClientRepository clients;

        /// <summary>
        /// Background processor timer to check if there is any failed sessions
        /// </summary>
        private Timer mainTimer;

        /// <summary>
        /// Get Transaction timer for failed session
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Current failed session (transaction)
        /// </summary>
        private ActiveSession failedSession;

        public GetTransactionProcessor(ISessionRepository sessionRepository, IHubContext<NotifyHub> notifyHub, IHttpClientRepository clients)
        {
            this.sessionRepository = sessionRepository;
            this.notifyHub = notifyHub;
            this.clients = clients;
        }

        /// <summary>
        /// Start lifetime background timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            mainTimer = new Timer(DoWorkAsync, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(TRX_FAILURE_PING_SECS));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Timer work: Check for new failed (expired) sessions
        /// If there is any start GetTransaction timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        private async void DoWorkAsync(object cancellationToken)
        {
            var apiClient = clients.GetAPIClient();
            var token = (await clients.GetTokenAsync())?.Token;

            if (apiClient != null && !string.IsNullOrEmpty(token) && failedSession == null)
            {
                failedSession = await sessionRepository.GetFailedSessionAsync();

                if (failedSession != null && failedSession.Expire < DateTime.Now.Ticks)
                {
                    await sessionRepository.DeleteFailedSessionAsync(failedSession.SessionId);

                    var cts = new CancellationTokenSource(TimeSpan.FromMinutes(TRX_RESPONSE_TIMEOUT_MINS));

                    timer = new Timer(StartGetTransactionAsync, cts, 0, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Timer work: call GetTransaction API 
        /// </summary>
        /// <param name="cancelerationToken"></param>
        private async void StartGetTransactionAsync(object cancellationToken)
        {
            CancellationTokenSource cts = (CancellationTokenSource)cancellationToken;

            var apiClient = clients.GetAPIClient();
            var token = (await clients.GetTokenAsync())?.Token;

            if (apiClient != null && !string.IsNullOrEmpty(token) && failedSession != null)
            {
                ApiResponse<EFTTransactionResponse> transaction = null;

                while (transaction == null && !cts.IsCancellationRequested)
                {
                    var url = apiClient.BaseAddress + $"sessions/{failedSession.SessionId}/transaction";

                    apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    try
                    {
                        var response = await apiClient.GetAsync(url);

                        if (response != null && response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsAsync<ApiResponse<EFTTransactionResponse>>();
                            if (apiResponse != null && apiResponse.Response != null)
                            {
                                transaction = apiResponse;

                                var message = new Message
                                {
                                    Response = apiResponse.Response,
                                    SessionId = failedSession.SessionId,
                                    Text = apiResponse?.Response?.ResponseText == null ? new string[] { "", "" } : apiResponse?.Response?.ResponseText.Split("\n"),
                                    Type = apiResponse?.Response?.ResponseType,
                                    CancelButton = true,
                                };

                                StopTimer(cts.Token);

                                await notifyHub.Clients.All.SendAsync("ReceiveMessage", message);
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                if (cts.IsCancellationRequested)
                {
                    var message = new Message
                    {
                        SessionId = failedSession.SessionId,
                        Text = new string[] { "NO RESPONSE", "PRESS CLOSE" },
                        Type = "transaction",
                        CancelButton = true,
                    };

                    await notifyHub.Clients.All.SendAsync("ReceiveMessage", message);

                    StopTimer(cts.Token);
                }
            }
        }

        /// <summary>
        /// Stop GetTransaction timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void StopTimer(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            failedSession = null;
        }

        /// <summary>
        /// Stop background lifetime timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            mainTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
