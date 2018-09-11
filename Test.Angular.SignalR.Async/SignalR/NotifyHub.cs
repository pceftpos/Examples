using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Test.Angular.SignalR.Async.Model;

namespace Test.Angular.SignalR.Async.SignalR
{
    /// <summary>
    /// Notifications Hub
    /// </summary>
    public class NotifyHub : Hub
    {
        /// <summary>
        /// Method to invoke from client application
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task Notify(Message message)
        {
            //Configure client application to listen for this
            return Clients.All.SendAsync("Message", message);
        }
    }
}
