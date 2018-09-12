namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model
{
    /// <summary> Message to client (client representation of notification from api) </summary>
    public class Message
    {
        /// <summary> Response body</summary>
        public EFTResponse Response { get; set; }

        /// <summary> Message type (transaction, display, etc)</summary>
        public string Type { get; set; }

        /// <summary> Message text in array of 2 string</summary>
        public string[] Text { get; set; }

        /// <summary> Session id</summary>
        public string SessionId { get; set; }

        /// <summary> Flag to show Yes button</summary>
        public bool YesButton { get; set; }

        /// <summary> Flag to show No button</summary>
        public bool NoButton { get; set; }

        /// <summary> Flag to show Cancel button</summary>
        public bool CancelButton { get; set; }

        /// <summary> Flag to show Ok button</summary>
        public bool OkButton { get; set; }

        /// <summary> Flag to show Auth button</summary>
        public bool AuthButton { get; set; }
    }
}
