namespace Test.Angular.SignalR.Async.Model
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
    }
}
