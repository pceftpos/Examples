using System;
using System.ComponentModel.DataAnnotations;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTTransactionRequest : EFTRequest
    {
        /// <summary>Constructs a default EFTTransactionRequest object.</summary>
        public EFTTransactionRequest() : base(true, typeof(EFTTransactionResponse))
        {
        }

        [StringLength(2, MinimumLength = 2)]
        public string Merchant { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string Application { get; set; }

        [StringLength(1)]
        public string ReceiptAutoPrint { get; set; }

        [StringLength(1)]
        public string CutReceipt { get; set; }

        public Newtonsoft.Json.Linq.JObject PurchaseAnalysisData { get; set; }

        [Required, StringLength(1)]
        public string TxnType { get; set; }

        [StringLength(3)]
        public string CurrencyCode { get; set; }

        [StringLength(1)]
        public string OriginalTxnType { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? Time { get; set; }

        public bool? TrainingMode { get; set; } = false;

        public bool? EnableTip { get; set; } = false;

        [Range(0, 999999999)]
        public int? AmtCash { get; set; }

        [Range(0, 999999999)]
        public int? AmtPurchase { get; set; }

        [Range(0, 999999)]
        public int? AuthCode { get; set; }

        [Required, StringLength(16)]
        public string TxnRef { get; set; }

        [StringLength(1)]
        public string PanSource { get; set; }

        [StringLength(20)]
        public string Pan { get; set; }

        [RegularExpression("(0[1-9]|1[0-2])[0-9][0-9]", ErrorMessage = "MMYY format")]
        public string DateExpiry { get; set; }

        [StringLength(40)]
        public string Track2 { get; set; }

        [StringLength(1)]
        public string CardAccountType { get; set; }

        [StringLength(12)]
        public string RRN { get; set; }

        public int? CVV { get; set; }

        /// <summary>
        /// Basket data to be sent as a part of this transaction
        /// </summary>
        public Newtonsoft.Json.Linq.JObject Basket { get; set; }
    }
}
