using System.ComponentModel.DataAnnotations;

namespace Test.Angular.SignalR.Model
{
    /// <summary>A PC-EFTPOS terminal logon request object.</summary>
    public class EFTLogonRequest : EFTRequest
    {
        /// <summary>Constructs a default terminal logon request object.</summary>
        public EFTLogonRequest() : this(" ")
        {
        }

        /// <summary>Constructs a terminal logon request object.</summary>
        /// <param name="LogonType">The logon type to perform.</param>
        public EFTLogonRequest(string LogonType) : base(true, typeof(EFTLogonResponse))
        {
            this.LogonType = LogonType;
        }

        /// <summary>Two digit merchant code. Defaults to "00" (EFTPOS)</summary>
        [Required, StringLength(2)]
        public string Merchant { get; set; } = "00";

        /// <summary>Type of logon to perform. Defaults to ' ' (Standard Logon)</summary>
        [Required, StringLength(1)]
        public string LogonType { get; set; } = " ";

        /// <summary>Additional information sent with the response</summary>
        public dynamic PurchaseAnalysisData { get; set; }

        /// <summary>Indicates where the request is to be sent to. Defaults to "00" (EFTPOS)</summary>
        [Required, StringLength(2)]
        public string Application { get; set; } = "00";

        /// <summary>Indicates whether to trigger receipt events. Defaults to '0' (POSPrinter)</summary>
        [Required, StringLength(1)]
        public string ReceiptAutoPrint { get; set; } = "0";

        /// <summary>Indicates whether PC-EFTPOS should cut receipts. Defaults to '0' (DontCut)</summary>
        [Required, StringLength(1)]
        public string CutReceipt { get; set; } = "0";
    }
}
