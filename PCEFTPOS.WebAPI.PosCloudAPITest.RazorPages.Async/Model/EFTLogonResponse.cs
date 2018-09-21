using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTLogonResponse : EFTResponse
    {
        /// <summary>Constructs a default terminal logon response object.</summary>
        public EFTLogonResponse() : base(typeof(EFTLogonRequest), "logon")
        {
        }

        public string PinPadVersion { get; set; }
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
