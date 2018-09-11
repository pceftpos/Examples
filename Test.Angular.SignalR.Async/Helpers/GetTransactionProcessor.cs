using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Test.Angular.SignalR.Async.Model;
using Test.Angular.SignalR.Async.SignalR;

namespace Test.Angular.SignalR.Async.Helpers
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
            mainTimer = new Timer(DoWork, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(TRX_FAILURE_PING_SECS)); 

            return Task.CompletedTask;
        }

        /// <summary>
        /// Timer work: Check for new failed (expired) sessions
        /// If there is any start GetTransaction timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void DoWork(object cancellationToken)
        {
            var apiClient = clients.GetAPIClient();
            var token = clients.GetToken()?.Result?.Token;

            if (apiClient != null && !string.IsNullOrEmpty(token) && failedSession == null)
            {
                failedSession = sessionRepository.GetFailedSession().Result;

                if (failedSession != null && failedSession.Expire < DateTime.Now.Ticks)
                {
                    sessionRepository.DeleteFailedSession(failedSession.SessionId);
                                        
                    var cts = new CancellationTokenSource(TimeSpan.FromMinutes(TRX_RESPONSE_TIMEOUT_MINS));

                    timer = new Timer(StartGetTransaction, cts, 0, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Timer work: call GetTransaction API 
        /// </summary>
        /// <param name="cancelerationToken"></param>
        private void StartGetTransaction(object cancelerationToken)
        {
            CancellationTokenSource cts = (CancellationTokenSource)cancelerationToken;

            var apiClient = clients.GetAPIClient();
            var token = clients.GetToken()?.Result?.Token;

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
                        var response = apiClient.GetAsync(url);

                        if (response.Result != null && response.Result.IsSuccessStatusCode)
                        {
                            var apiResponse = response.Result.Content.ReadAsAsync<ApiResponse<EFTTransactionResponse>>()?.Result;
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

                                notifyHub.Clients.All.SendAsync("Message", message);
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
                    
                    notifyHub.Clients.All.SendAsync("Message", message);

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
