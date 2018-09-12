namespace Test.Angular.SignalR.Model
{
    /// <summary> Application settings</summary>
    public class AppSettings
    {
        /// <summary> Token server URI</summary>
        public string TokenServer { get; set; }

        /// <summary> API server URI</summary>
        public string Server { get; set; }

        /// <summary> Point of sale name</summary>
        public string PosName { get; set; }

        /// <summary> Point of sale version</summary>
        public string PosVersion { get; set; }

        /// <summary> Notifications URI</summary>
        public string NotificationUri { get; set; }

        /// <summary> Merchant code</summary>
        public string Merchant { get; set; }

        /// <summary> Indicates where the request is to be sent to</summary>
        public string Application { get; set; }

        /// <summary> Path to configuration for POS Client (Angular App)</summary>
        public string ConfigFile { get; set; }

        /// <summary> Pinpad username</summary>
        public string PinpadUsername { get; set; }

        /// <summary> Pinpad password</summary>
        public string PinpadPassword { get; set; }

        /// <summary> Pinpad pairing code</summary>
        public string PinpadPairCode { get; set; }
    }
}

