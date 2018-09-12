using System.ComponentModel.DataAnnotations;

namespace Test.Angular.SignalR.Model
{
    /// <summary>
    /// Notification setup for API
    /// </summary>
    public class Notification
    {
        /// <summary>Notifications controller URL </summary>
        [Required, Url, RegularExpression("https", ErrorMessage = "Uri must be https")]
        public string Uri { get; set; }

        /// <summary>Authorization header (e.g. token) </summary>
        public string AuthorizationHeader { get; set; }
    }
}