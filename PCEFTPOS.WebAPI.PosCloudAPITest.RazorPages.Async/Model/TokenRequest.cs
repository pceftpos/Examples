using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class TokenRequest
    {
        public Guid PosId { get; set; }
        public string PosName { get; set; }
        public string PosVersion { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PairCode { get; set; }
    }
}
