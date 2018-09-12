namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model
{
    /// <summary>A PC-EFTPOS receipt request object.</summary>
    public class EFTReceiptRequest : EFTRequest
    {
        /// <summary>Constructs a default display response object.</summary>
		public EFTReceiptRequest() : base(false, typeof(EFTReceiptResponse))
        {
        }
    }
}
