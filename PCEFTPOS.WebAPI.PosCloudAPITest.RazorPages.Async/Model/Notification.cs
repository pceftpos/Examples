using System.ComponentModel.DataAnnotations;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    /// <summary>
    /// Defines details used to call an event/notification back on the POS system
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// The notification uri that will be called when an event occurs. This must start with https://.
        /// If included, the tags {{sessionId}} and {{type}} will be replaced by the session ID of the request, and the request type. 
        /// e.g. https://
        /// </summary>
        [Required, Url, RegularExpression("https", ErrorMessage = "Uri must be https")]
        public string Uri { get; set; }

        /// <summary>
        /// If not null, the content of this field will be used to populate the HTTP 
        /// authorization header when calling an event in the POS. 
        /// e.g. "Basic YWxhZGRpbjpvcGVuc2VzYW1l", OR "Bearer eyJhbGciOiJIUzI1NiJ9.eyJuYW1lIjoiSm9obiBEb2UifQ.K1lVDxQYcBTPnWMTGeUa3gYAgdEhMFFv38VmOyl95bA"
        /// </summary>
        public string AuthorizationHeader { get; set; }
    }
}
