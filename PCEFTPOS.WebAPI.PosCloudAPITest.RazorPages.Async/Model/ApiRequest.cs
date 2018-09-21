namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    /// <summary>
    /// Defines a request into the API from the POS
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiRequest<T>
    {
        public ApiRequest() : this(default(T))
        {
        }

        public ApiRequest(T data)
        {
            Request = data;
        }

        /// <summary>
        /// The command to execute
        /// </summary>
        public T Request { get; set; } // TODO: should this be "Command" or something similar

        /// <summary>
        /// The notification details to use if the POS wants to be notified of events that occur during the request
        /// </summary>
        public Notification Notification { get; set; }
    }
}