using System;

namespace Test.Angular.SignalR.Model
{
    /// <summary> Token request</summary>
    public class TokenRequest
    {
        /// <summary> Pinpad username</summary>
        public string Username { get; set; }

        /// <summary> Pinpad password</summary>
        public string Password { get; set; }

        /// <summary> Pinpad pairing code</summary>
        public string PairCode { get; set; }

        /// <summary>
        /// Name of the POS that sent the request
        /// </summary>
        public string PosName { get; set; }

        /// <summary>
        /// Version of the POS that sent the request
        /// </summary>
        public string PosVersion { get; set; }

        /// <summary>
        /// Unique identifier for the POS that sent the request
        /// </summary>
        public Guid? PosId { get; set; }
    }
}
