using Microsoft.Extensions.Hosting;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Helpers
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private const int TIMER_TICK_SEC = 1;

        private Timer timer;
        private Guid sessionId;
        private EFTTransactionResponse transaction;
        private HttpClient apiClient;
        private string token;

        public TimedHostedService(Guid sessionId, HttpClient apiClient, string token)
        {
            this.sessionId = sessionId;
            this.apiClient = apiClient;
            this.token = token;
        }

        public EFTTransactionResponse GetTransaction()
        {
            return transaction;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(TIMER_TICK_SEC));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            if (sessionId != null)
            {
                var url = apiClient.BaseAddress + $"sessions/{sessionId}/transaction";

                apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var response = await apiClient.GetAsync(url);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsAsync<ApiResponse<EFTTransactionResponse>>();
                        if (apiResponse?.Response != null)
                        {
                            transaction = apiResponse.Response;
                        }
                    }
                }
                catch
                {
                }
            }
            else
            {
                await StopAsync((CancellationToken)state);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (transaction == null)
            {
                transaction = new EFTTransactionResponse() { Success = false };
            }

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}

