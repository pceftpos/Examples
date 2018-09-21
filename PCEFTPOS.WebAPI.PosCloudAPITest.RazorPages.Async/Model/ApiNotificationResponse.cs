namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class ApiNotificationResponse
    {
        public ApiNotificationResponse() : this(null, null)
        {
        }

        public ApiNotificationResponse(EFTResponse data, string sessionId)
        {
            Response = data;
            SessionId = sessionId;
        }

        public string SessionId { get; set; }
        public EFTResponse Response { get; set; } // TODO: should this be "Event" or something similar?
    }

    public class ApiNotificationResponse<T> where T: EFTResponse
    {
        public ApiNotificationResponse() : this(null, null)
        {
        }

        public ApiNotificationResponse(T data, string sessionId)
        {
            Response = data;
            SessionId = sessionId;
        }

        public string SessionId { get; set; }
        public T Response { get; set; } // TODO: should this be "Event" or something similar?
    }
}
