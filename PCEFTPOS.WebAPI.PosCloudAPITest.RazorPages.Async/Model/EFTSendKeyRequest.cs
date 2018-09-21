namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTSendKeyRequest : EFTRequest
    {
        public EFTSendKeyRequest() : base(false, null)
        {
        }

        public string Key { get; set; }
        public string Data { get; set; }
        public string SessionId { get; set; }
    }
}
