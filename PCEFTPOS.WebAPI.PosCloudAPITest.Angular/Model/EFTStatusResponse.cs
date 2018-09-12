using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model
{
    /// <summary>A PC-EFTPOS terminal status response object.</summary>
    public class EFTStatusResponse : EFTResponse
    {
        public EFTStatusResponse() : base(typeof(EFTStatusRequest), "status")
        {
        }

        /// <summary>Two digit merchant code</summary>
        public string Merchant { get; set; }

        /// <summary>The AIIC that is configured in the terminal.</summary>
        public string AIIC { get; set; }

        /// <summary>The NII that is configured in the terminal.</summary>
        public int NII { get; set; }

        /// <summary>Terminal ID configured in the PIN pad.</summary>
        public string Catid { get; set; }

        /// <summary>Merchant ID configured in the PIN pad.</summary>
        public string Caid { get; set; }

        /// <summary>The bank response timeout that is configured in the terminal.</summary>
        public int Timeout { get; set; }

        /// <summary>Indicates if the PIN pad is currently logged on.</summary>
        public bool LoggedOn { get; set; }

        /// <summary>The serial number of the terminal.</summary>
        public string PinPadSerialNumber { get; set; }

        /// <summary>PIN pad software version.</summary>
        public string PinPadVersion { get; set; }

        /// <summary>The bank acquirer code.</summary>
        public char BankCode { get; set; }

        /// <summary>The bank description.</summary>
        public string BankDescription { get; set; }

        /// <summary>Key verification code.</summary>
        public string KVC { get; set; }

        /// <summary>Current number of stored transactions.</summary>
        public int SAFCount { get; set; }

        /// <summary>The acquirer communications type.</summary>
        public string NetworkType { get; set; }

        /// <summary>The hardware serial number.</summary>
        public string HardwareSerial { get; set; }

        /// <summary>The merchant retailer name.</summary>
        public string RetailerName { get; set; }

        /// <summary>Store-and forward credit limit.</summary>
        public int SAFCreditLimit { get; set; }

        /// <summary>Store-and-forward debit limit.</summary>
        public int SAFDebitLimit { get; set; }

        /// <summary>The maximum number of store transactions.</summary>
        public int MaxSAF { get; set; }

        /// <summary>The terminal key handling scheme.</summary>
        public string KeyHandlingScheme { get; set; }

        /// <summary>The maximum cash out limit.</summary>
        public int CashoutLimit { get; set; }

        /// <summary>The maximum refund limit.</summary>
        public int RefundLimit { get; set; }

        /// <summary>Card prefix table version.</summary>
        public string CPATVersion { get; set; }

        /// <summary>Card name table version.</summary>
        public string NameTableVersion { get; set; }

        /// <summary>The terminal to PC communication type.</summary>
        public string TerminalCommsType { get; set; }

        /// <summary>Number of card mis-reads.</summary>
        public int CardMisreadCount { get; set; }

        /// <summary>Number of memory pages in the PIN pad terminal.</summary>
        public int TotalMemoryInTerminal { get; set; }

        /// <summary>Number of free memory pages in the PIN pad terminal.</summary>
        public int FreeMemoryInTerminal { get; set; }

        /// <summary>The type of PIN pad terminal.</summary>
        public string EFTTerminalType { get; set; }

        /// <summary>Number of applications in the terminal.</summary>
        public int NumAppsInTerminal { get; set; }

        /// <summary>Number of available display line on the terminal.</summary>
        public int NumLinesOnDisplay { get; set; }

        /// <summary>The date the hardware was incepted.</summary>
        public DateTime HardwareInceptionDate { get; set; }

        /// <summary>Indicates if the request was successful.</summary>
        public bool Success { get; set; } = false;

        /// <summary>The response code of the request.</summary>
        public string ResponseCode { get; set; }

        /// <summary>The response text for the response code.</summary>
        public string ResponseText { get; set; }
    }
}
