using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data.Interface;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data
{
#pragma warning disable 1591
    public class NotificationRepository : INotificationRepository
    {
        private readonly List<Message> messages = new List<Message>();
        private readonly SemaphoreSlim messagesLock = new SemaphoreSlim(1, 1);

        public async Task AddMessageAsync(Message message)
        {
            await messagesLock.WaitAsync();
            try
            {
                if (message != null)
                {
                    messages.Add(message);
                }
            }
            finally
            {
                messagesLock.Release();
            }
        }
        public async Task DeleteMessageAsync(Message message)
        {
            await messagesLock.WaitAsync();
            try
            {
                if (messages != null && messages.Count() > 0)
                {
                    var idx = messages.FindIndex(s => s.SessionId == message.SessionId &&
                                                 s.Text == message.Text &&
                                                 s.Type == message.Type);
                    if (idx >= 0)
                    {
                        messages.RemoveAt(idx);
                    }
                }
            }
            finally
            {
                messagesLock.Release();
            }
        }

        public async Task<Message> GetLastMessageAsync()
        {
            await messagesLock.WaitAsync();
            try
            {
                return messages.FirstOrDefault();
            }
            finally
            {
                messagesLock.Release();
            }
        }

    }
#pragma warning restore 1591
}
