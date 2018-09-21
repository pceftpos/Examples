namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    /// <summary>
    /// ActiveSession represents Transaction expire
    /// </summary>
    public class ActiveSession
    {
        /// <summary>
        /// Session Id
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Session expire time in ticks (3 minutes from session start)
        /// </summary>
        public long Expire { get; set; }
    }
}
