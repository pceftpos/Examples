namespace Test.Angular.SignalR.Async.Model
{
    /// <summary>A PC-EFTPOS receipt response object.</summary>
    public class EFTReceiptResponse : EFTResponse
    {
        public EFTReceiptResponse() : base(typeof(EFTReceiptRequest), "receipt")
        {
        }

        /// <summary>The receipt type.</summary>
        public char Type { get; set; }

        /// <summary>Receipt text to be printed.</summary>
        public string[] ReceiptText { get; set; }

        /// <summary>Receipt response is a pre-print.</summary>
        public bool IsPrePrint { get; set; }
    }
}
