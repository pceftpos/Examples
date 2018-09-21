namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    /// <summary> Token response</summary>
    public class TokenResponse
    {
        /// <summary> Token</summary>
        public string Token { get; set; }

        /// <summary> Token expire in seconds</summary>
        public int ExpirySeconds { get; set; }
    }
}
