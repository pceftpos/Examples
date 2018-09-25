using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model
{
    /// <summary>A PC-EFTPOS terminal transaction response object.</summary>
    public class EFTTransactionResponse : EFTResponse
    {
        /// <summary>Constructs a default terminal transaction response object.</summary>
        public EFTTransactionResponse() : base(typeof(EFTTransactionRequest), "transaction")
        {
        }

        /// <summary>The type of transaction to perform.</summary>
        public string TxnType { get; set; }

        /// <summary>Two digit merchant code</summary>
        public string Merchant { get; set; }

        /// <summary>Indicates the card type that was used in the transaction.</summary>
        public string CardType { get; set; }

        /// <summary>Indicates the card type that was used in the transaction.</summary>
        public string CardName { get; set; }

        /// <summary>The retrieval reference number for the transaction.</summary>
        public string RRN { get; set; }

        /// <summary>Indicates which settlement batch this transaction will be included in.</summary>
        public DateTime DateSettlement { get; set; }

        /// <summary>The cash amount for the transaction. This property is mandatory for a 'C' transaction type.</summary>
        public int AmtCash { get; set; }

        /// <summary>The purchase amount for the transaction.</summary>
        public int AmtPurchase { get; set; }

        /// <summary>The tip amount for the transaction.</summary>
        public int AmtTip { get; set; }

        /// <summary>The authorisation number for the transaction.</summary>
        public int AuthCode { get; set; }

        /// <summary>The reference number to attach to the transaction. This will appear on the receipt.</summary>
        public string TxnRef { get; set; }

        /// <summary>The card number to use when pan source of POS keyed is used. Use this property in conjunction with PanSource</summary>
        public string Pan { get; set; }

        /// <summary>
        /// The expiry date of the card when of POS keyed is used. In MMYY format.
        /// Use this property in conjunction with PanSource when passing the card expiry date to PC-EFTPOS.
        /// </summary>
        public string DateExpiry { get; set; }

        /// <summary> The track 2 to use when of POS swiped is used. Use this property in conjunction with PanSource</summary>
        public string Track2 { get; set; }

        /// <summary>The account to use for this transaction. Use ' ' to prompt user to enter the account type</summary>
        public string AccountType { get; set; }

        /// <summary>Indicates if an available balance is present in the response.</summary>
        public bool? BalanceReceived { get; set; }

        /// <summary>Balance available on the processed account.</summary>
        public int? AvailableBalance { get; set; }

        /// <summary>Cleared balance on the processed account.</summary>
        public int? ClearedFundsBalance { get; set; }

        /// <summary>Indicates if the request was successful.</summary>
        public bool Success { get; set; }

        /// <summary>The response code of the request.</summary>
        public string ResponseCode { get; set; }

        /// <summary>The response text for the response code.</summary>
        public string ResponseText { get; set; }

        /// <summary>Date and time of the response returned by the bank.</summary>
        public DateTime Date { get; set; }

        /// <summary>Terminal ID configured in the PIN pad.</summary>
        public string Catid { get; set; }

        /// <summary>Merchant ID configured in the PIN pad.</summary>
        public string Caid { get; set; }

        /// <summary>System Trace Audit Number</summary>
        public int Stan { get; set; }

        /// <summary>Additional information sent with the response.</summary>
        public Newtonsoft.Json.Linq.JObject PurchaseAnalysisData { get; set; }
    }
}
