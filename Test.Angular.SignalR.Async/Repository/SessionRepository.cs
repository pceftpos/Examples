using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Test.Angular.SignalR.Async.Model;

namespace Test.Angular.SignalR.Async
{
    public class SessionRepository: ISessionRepository
    {
        private readonly List<ActiveSession> sessions = new List<ActiveSession>();
        private readonly List<ActiveSession> failedSessions = new List<ActiveSession>();
        private readonly SemaphoreSlim messagesLock = new SemaphoreSlim(1, 1);
        
        public async Task AddSessionAsync(ActiveSession txn)
        {
            await messagesLock.WaitAsync();
            try
            {
                if (sessions != null)
                {
                    sessions.Add(txn);
                }
            }
            finally
            {
                messagesLock.Release();
            }
        }
        
        public async Task DeleteSessionAsync(string sessionId)
        {
            await messagesLock.WaitAsync();
            try
            {
                if (sessions != null && sessions.Count() > 0)
                {
                    var idx = sessions.FindIndex(s => string.Equals(s.SessionId, sessionId) == true);
                    if (idx >= 0)
                    {
                        sessions.RemoveAt(idx);
                    }
                }
            }
            finally
            {
                messagesLock.Release();
            }
        }       

        public async Task<ActiveSession> GetOldestSessionAsync()
        {
            await messagesLock.WaitAsync();
            try
            {
                sessions.OrderBy(x => x.Expire);
                return sessions.FirstOrDefault();
            }
            finally
            {
                messagesLock.Release();
            }
        }

        public async Task AddFailedSessionAsync(ActiveSession session)
        {
            await messagesLock.WaitAsync();
            try
            {
                if (failedSessions != null)
                {
                    failedSessions.Add(session);
                }
            }
            finally
            {
                messagesLock.Release();
            }
        }

        public async Task<ActiveSession> GetFailedSessionAsync()
        {
            await messagesLock.WaitAsync();
            try
            {
                failedSessions.OrderBy(x => x.Expire);
                return failedSessions.FirstOrDefault();
            }
            finally
            {
                messagesLock.Release();
            }
        }

        public async Task DeleteFailedSessionAsync(string sessionId)
        {
            await messagesLock.WaitAsync();
            try
            {
                if (failedSessions != null && failedSessions.Count() > 0)
                {
                    var idx = failedSessions.FindIndex(s => string.Equals(s.SessionId, sessionId));

                    if (idx != -1)
                    {
                        failedSessions.RemoveAt(idx);
                    }
                }
            }
            finally
            {
                messagesLock.Release();
            }
        }
    }
}

