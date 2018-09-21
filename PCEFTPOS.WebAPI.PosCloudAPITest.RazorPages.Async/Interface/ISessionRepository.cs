using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Interface
{
    /// <summary>
    /// Session (represent transaction) repository
    /// </summary>
    public interface ISessionRepository
    {
        /// <summary>
        /// Add new session to the session list
        /// </summary>
        /// <param name="txn"></param>
        /// <returns></returns>
        Task AddSessionAsync(ActiveSession txn);

        /// <summary>
        /// Get the oldest session from the list
        /// </summary>
        /// <returns></returns>
        Task<ActiveSession> GetOldestSessionAsync();

        /// <summary>
        /// Delete completed sessions from session list
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task DeleteSessionAsync(string sessionId);

        /// <summary>
        /// Add expired session to the failed sessions list
        /// </summary>
        /// <param name="txn"></param>
        /// <returns></returns>
        Task AddFailedSessionAsync(ActiveSession txn);

        /// <summary>
        /// Get session from expired sessions list
        /// </summary>
        /// <returns></returns>
        Task<ActiveSession> GetFailedSessionAsync();

        /// <summary>
        /// Delete session from expired sessions list
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task DeleteFailedSessionAsync(string sessionId);
    }
}
