namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class ApiResponse<T>
    {
        public ApiResponse() : this(default(T))
        {
        }

        public ApiResponse(T data)
        {
            Response = data;
        }

        public T Response { get; set; } // TODO: should this be "Event" or something similar?
    }
}