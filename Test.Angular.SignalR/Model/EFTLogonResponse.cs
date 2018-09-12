using System;

namespace Test.Angular.SignalR.Model
{
    /// <summary>A PC-EFTPOS terminal logon response object.</summary>
    public class EFTLogonResponse : EFTResponse
    {
        /// <summary>Constructs a default terminal logon response object.</summary>
        public EFTLogonResponse() : base(typeof(EFTLogonRequest), "logon")
        {
        }

        /// <summary>PIN pad software version.</summary>
        public string PinPadVersion { get; set; }

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
