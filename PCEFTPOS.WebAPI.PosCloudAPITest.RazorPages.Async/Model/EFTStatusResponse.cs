using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTStatusResponse : EFTResponse
    {
        public EFTStatusResponse()
            : base(typeof(EFTStatusRequest), "status")
        {
        }

        public string Merchant { get; set; }
        public string AIIC { get; set; }
        public int NII { get; set; }
        public string Catid { get; set; }
        public string Caid { get; set; }
        public int Timeout { get; set; }
        public bool LoggedOn { get; set; }
        public string PinPadSerialNumber { get; set; }
        public string PinPadVersion { get; set; }
        public char BankCode { get; set; }
        public string BankDescription { get; set; }
        public string KVC { get; set; }
        public int SAFCount { get; set; }
        public string NetworkType { get; set; }
        public string HardwareSerial { get; set; }
        public string RetailerName { get; set; }
        public int SAFCreditLimit { get; set; }
        public int SAFDebitLimit { get; set; }
        public int MaxSAF { get; set; }
        public string KeyHandlingScheme { get; set; }
        public int CashoutLimit { get; set; }
        public int RefundLimit { get; set; }
        public string CPATVersion { get; set; }
        public string NameTableVersion { get; set; }
        public string TerminalCommsType { get; set; }
        public int CardMisreadCount { get; set; }
        public int TotalMemoryInTerminal { get; set; }
        public int FreeMemoryInTerminal { get; set; }
        public string EFTTerminalType { get; set; }
        public int NumAppsInTerminal { get; set; }
        public int NumLinesOnDisplay { get; set; }
        public DateTime HardwareInceptionDate { get; set; }
        public bool Success { get; set; } = false;
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
    }
}
