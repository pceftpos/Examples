using System.Threading.Tasks;
using Test.Angular.SignalR.Async.Model;

namespace Test.Angular.SignalR.Async
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
        Task AddSession(ActiveSession txn);

        /// <summary>
        /// Get the oldest session from the list
        /// </summary>
        /// <returns></returns>
        Task<ActiveSession> GetOldestSession();

        /// <summary>
        /// Delete completed sessions from session list
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task DeleteSession(string sessionId);
        
        /// <summary>
        /// Add expired session to the failed sessions list
        /// </summary>
        /// <param name="txn"></param>
        /// <returns></returns>
        Task AddFailedSession(ActiveSession txn);

        /// <summary>
        /// Get session from expired sessions list
        /// </summary>
        /// <returns></returns>
        Task<ActiveSession> GetFailedSession();

        /// <summary>
        /// Delete session from expired sessions list
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task DeleteFailedSession(string sessionId);
    }
}
