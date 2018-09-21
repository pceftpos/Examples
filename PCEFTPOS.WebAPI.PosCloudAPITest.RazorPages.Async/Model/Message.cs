namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class Message
    {
        public EFTResponse Response { get; set; }
        public string Type { get; set; }
        public string[] Text { get; set; }
        public string SessionId { get; set; }
        public bool YesButton { get; set; }
        public bool NoButton { get; set; }
        public bool CancelButton { get; set; }
        public bool OkButton { get; set; }
        public bool AuthButton { get; set; }
    }
}
