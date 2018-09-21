namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTReceiptRequest : EFTRequest
    {
        /// <summary>Constructs a default display response object.</summary>
		public EFTReceiptRequest() : base(false, typeof(EFTReceiptResponse))
        {
        }
    }
}
