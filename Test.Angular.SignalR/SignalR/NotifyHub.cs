using Microsoft.AspNetCore.SignalR;
using Test.Angular.SignalR.Model;
using System.Threading.Tasks;

namespace Test.Angular.SignalR.SignalR
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
