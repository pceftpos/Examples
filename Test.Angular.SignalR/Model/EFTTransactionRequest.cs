using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Angular.SignalR.Model
{
    /// <summary>A PC-EFTPOS transaction request object.</summary>
	public class EFTTransactionRequest : EFTRequest
    {
        /// <summary>Constructs a default EFTTransactionRequest object.</summary>
        public EFTTransactionRequest() : base(true, typeof(EFTTransactionResponse))
        {
        }

        /// <summary>Two digit merchant code. Defaults to "00" (EFTPOS)</summary>
        [StringLength(2, MinimumLength = 2)]
        public string Merchant { get; set; }

        /// <summary>Indicates where the request is to be sent to. Defaults to "00" (EFTPOS)</summary>
        [StringLength(2, MinimumLength = 2)]
        public string Application { get; set; }

        /// <summary>Indicates whether to trigger receipt events. Defaults to '0' (POSPrinter)</summary>
        [StringLength(1)]
        public string ReceiptPrintMode { get; set; }

        /// <summary>Indicates whether PC-EFTPOS should cut receipts. Defaults to '0' (DontCut)</summary>
        [StringLength(1)]
        public string ReceiptCutMode { get; set; }

        /// <summary>Additional information sent with the response</summary>
        public Newtonsoft.Json.Linq.JObject PurchaseAnalysisData { get; set; }

        /// <summary>The type of transaction to perform. Required.</summary>
        [Required, StringLength(1)]
        public string TxnType { get; set; }

        /// <summary>The currency code for this transaction (e.g. AUD). A 3 digit ISO currency code. Defaults to "   " (NotSet)</summary>
        [StringLength(3)]
        public string CurrencyCode { get; set; }

        /// <summary>The original type of transaction for voucher entry. Defaults to 'P' (PurchaseCash)</summary>
        [StringLength(1)]
        public string OriginalTxnType { get; set; }

        /// <summary>Original transaction date. Used for voucher or completion only</summary>
        public DateTime? Date { get; set; }

        /// <summary>Original transaction time. Used for voucher or completion only</summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// Determines if the transaction is a training mode transaction.
        /// Set to TRUE if the transaction is to be performed in training mode. The default is FALSE.
        /// </summary>
        public bool? TrainingMode { get; set; } = false;

        /// <summary>
        /// Indicates if the transaction should be tipable.
        /// Set to TRUE if tipping is to be enabled for this transaction. The default is FALSE
        /// </summary>
        public bool? EnableTip { get; set; } = false;

        /// <summary>
        /// The cash amount for the transaction.
        /// This property is mandatory for a 'C' transaction type.
        /// Defaults to 0
        /// </summary>
        [Range(0, 999999999)]
        public int? AmtCash { get; set; }

        /// <summary>
        /// The purchase amount for the transaction.
        /// Defaults to 0
        /// </summary>
        [Range(0, 999999999)]
        public int? AmtPurchase { get; set; }

        /// <summary>The authorisation number for the transaction. Defaults to 0</summary>
        [Range(0, 999999)]
        public int? AuthCode { get; set; }

        /// <summary>The reference number to attach to the transaction. This will appear on the receipt.</summary>
        [Required, StringLength(16)]
        public string TxnRef { get; set; }

        /// <summary>
        /// Indicates the source of the card number. Use this property for card not present transactions.
        /// Defaults to ' ' (Default)
        /// </summary>
        [StringLength(1)]
        public string PanSource { get; set; }

        /// <summary>The card number to use when pan source of POS keyed is used. Use this property in conjunction with PanSource</summary>
        [StringLength(20)]
        public string Pan { get; set; }

        /// <summary>The expiry date of the card when of POS keyed is used. In MMYY format. Use this property in conjunction with PanSource when passing the card expiry date to PC-EFTPOS.</summary>
        [RegularExpression("(0[1-9]|1[0-2])[0-9][0-9]", ErrorMessage = "MMYY format")]
        public string DateExpiry { get; set; }

        /// <summary>The track 2 to use when of POS swiped is used. Use this property in conjunction with PanSource</summary>
        [StringLength(40)]
        public string Track2 { get; set; }

        /// <summary>The account to use for this transaction. Defaults to ' ' (prompt user)</summary>
        [StringLength(1)]
        public string CardAccountType { get; set; }

        /// <summary>The retrieval reference number for the transaction. Only required for some transactiont types</summary>
        [StringLength(12)]
        public string RRN { get; set; }

        /// <summary>CVV. Defaults to 0</summary>
        public int? CVV { get; set; }
    }
}
