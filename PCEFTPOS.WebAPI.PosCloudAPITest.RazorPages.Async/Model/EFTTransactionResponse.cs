using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTTransactionResponse : EFTResponse
    {
        /// <summary>Constructs a default terminal transaction response object.</summary>
        public EFTTransactionResponse() : base(typeof(EFTTransactionRequest), "transaction")
        {
        }

        public string TxnType { get; set; }
        public string Merchant { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string RRN { get; set; }
        public DateTime DateSettlement { get; set; }
        public int AmtCash { get; set; }
        public int AmtPurchase { get; set; }
        public int AmtTip { get; set; }
        public int AuthCode { get; set; }
        public string TxnRef { get; set; }
        public string Pan { get; set; }
        public string DateExpiry { get; set; }
        public string Track2 { get; set; }
        public string AccountType { get; set; }
        //TODO
        //public string TxnFlags { get; set; }
        public bool? BalanceReceived { get; set; }
        public int? AvailableBalance { get; set; }
        public int? ClearedFundsBalance { get; set; }
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public DateTime Date { get; set; }
        public string Catid { get; set; }
        public string Caid { get; set; }
        public int Stan { get; set; }
        public Newtonsoft.Json.Linq.JObject PurchaseAnalysisData { get; set; }
    }
}
