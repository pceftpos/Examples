namespace Test.Angular.SignalR.Async.Model
{
    /// <summary>A PC-EFTPOS client list request object.</summary>
    public class EFTSendKeyRequest : EFTRequest
    {
        public EFTSendKeyRequest() : base(false, null)
        {
        }

        /// <summary>
        /// The type of key to send. Required.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Data entered by the POS (e.g. for an 'input entry' dialog type)
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Session(transaction) Id 
        /// </summary>
        public string SessionId { get; set; }
    }
}
