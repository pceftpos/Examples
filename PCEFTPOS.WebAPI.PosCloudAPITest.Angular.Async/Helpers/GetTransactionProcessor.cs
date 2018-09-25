using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data.Interface;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Helpers
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
        /// Clients repository
        /// </summar
        private IHttpClientRepository clients;

        /// <summary>
        /// Notifications repository
        /// </summar
        private INotificationRepository notifications;

        /// <summary>
        /// Background processor 10 seconds timer to check if there is any failed sessions
        /// </summary>
        private Timer mainTimer;

        /// <summary>
        /// Get Transaction 3 minutes timer for failed session
        /// </summary>
        private Timer timer;  

        /// <summary>
        /// Active session object
        /// </summary>
        private ActiveSession failedSession;

        public GetTransactionProcessor(ISessionRepository sessionRepository, IHttpClientRepository clients, INotificationRepository notifications)
        {
            this.sessionRepository = sessionRepository;
            this.clients = clients;
            this.notifications = notifications;
        }

        /// <summary>
        /// Start lifetime background timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {            
            mainTimer = new Timer(DoWork, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(TRX_FAILURE_PING_SECS));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Timer work: Check for new failed (expired) sessions
        /// If there is any start GetTransaction timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        private async void DoWork(object cancellationToken)
        {
            var apiClient = clients.GetAPIClient();
            var token = (await clients.GetTokenAsync())?.Token;

            if (apiClient != null && !string.IsNullOrEmpty(token) && failedSession == null)
            {
                failedSession = await sessionRepository.GetFailedSessionAsync();

                if (failedSession != null && failedSession.Expire < DateTime.Now.Ticks)
                {
                    await sessionRepository.DeleteFailedSessionAsync(failedSession.SessionId);
                                        
                    var cts = new CancellationTokenSource(TRX_RESPONSE_TIMEOUT_MINS);

                    timer = new Timer(StartGetTransactionAsync, cts, 0, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Timer work: call GetTransaction API 
        /// </summary>
        /// <param name="cancellationToken"></param>
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
                                    Type = apiResponse?.ResponseType,
                                    CancelButton = true,
                                };

                                StopTimer(cts.Token);

                                await notifications.AddMessageAsync(message);
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

                    await notifications.AddMessageAsync(message);

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
        /// Stop background service
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
