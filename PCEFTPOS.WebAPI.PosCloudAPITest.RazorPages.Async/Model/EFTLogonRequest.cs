using System.ComponentModel.DataAnnotations;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTLogonRequest : EFTRequest
    {
        public EFTLogonRequest() : this(" ")
        {
        }

        public EFTLogonRequest(string LogonType) : base(true, typeof(EFTLogonResponse))
        {
            this.LogonType = LogonType;
        }

        [Required, StringLength(2)]
        public string Merchant { get; set; } = "00";

        [Required, StringLength(1)]
        public string LogonType { get; set; } = " ";

        public dynamic PurchaseAnalysisData { get; set; }

        [Required, StringLength(2)]
        public string Application { get; set; } = "00";

        [Required, StringLength(1)]
        public string ReceiptAutoPrint { get; set; } = "0";

        [Required, StringLength(1)]
        public string CutReceipt { get; set; } = "0";
    }
}
