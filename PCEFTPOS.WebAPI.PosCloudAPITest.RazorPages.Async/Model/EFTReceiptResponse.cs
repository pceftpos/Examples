namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTReceiptResponse : EFTResponse
    {
        public EFTReceiptResponse() : base(typeof(EFTReceiptRequest), "receipt")
        {
        }

        public char Type { get; set; }
        public string[] ReceiptText { get; set; }
        public bool IsPrePrint { get; set; }
    }
}
