using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Data.Interface
{
    public interface INotificationRepository
    {
        /// <summary>
        /// Add new notification from REST API to the list
        /// </summary>
        /// <param name="message">New message</param>
        /// <returns></returns>
        Task AddMessageAsync(Message message);

        /// <summary>
        /// Add notification from list
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns></returns>
        Task DeleteMessageAsync(Message message);

        /// <summary>
        /// Get last message from the notifications list
        /// </summary>
        /// <returns>Message</returns>
        Task<Message> GetLastMessageAsync();
    }
}
