namespace Test.Angular.SignalR.Async.Model
{
    /// <summary>
    /// Notification response from cloud API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiNotificationResponse<T> where T : EFTResponse
    {
        public ApiNotificationResponse() : this(null, null)
        {
        }

        public ApiNotificationResponse(T data, string sessionId)
        {
            Response = data;
            SessionId = sessionId;
        }

        /// <summary> Session id</summary>       
        public string SessionId { get; set; }

        /// <summary> Notification recponse</summary>
        public T Response { get; set; } 
    }
}
