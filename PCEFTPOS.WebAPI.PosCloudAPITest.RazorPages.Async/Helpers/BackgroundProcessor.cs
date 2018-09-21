using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.SignalR;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Interface;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Helpers
{
    /// <summary>
    /// Background Processor to check sessions and update the expired list (not finished transactions list) every 10 seconds
    /// </summary>
    public class BackgroundProcessor : IHostedService
    {
        private const int SESSION_TIMER_SECS = 10;

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
        /// Timer
        /// </summary>
        private Timer timer;

        public BackgroundProcessor(ISessionRepository sessionRepository, IHubContext<NotifyHub> notifyHub, IHttpClientRepository clients)
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
            timer = new Timer(DoWorkAsync, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(SESSION_TIMER_SECS));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Timer work: Check if there is any failed (expired) sessions
        /// </summary>
        /// <param name="state"></param>
        private async void DoWorkAsync(object state)
        {
            var session = await sessionRepository.GetOldestSessionAsync();

            if (session != null && session.Expire < DateTime.Now.Ticks)
            {
                await sessionRepository.AddFailedSessionAsync(session);
                await sessionRepository.DeleteSessionAsync(session.SessionId);
            }
        }

        /// <summary>
        /// Stop timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

    }
}
