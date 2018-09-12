using System.ComponentModel.DataAnnotations;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model
{
    /// <summary>A PC-EFTPOS terminal status request object.</summary>
    public class EFTStatusRequest : EFTRequest
    {
        public EFTStatusRequest() : base(true, typeof(EFTStatusResponse))
        {
        }

        /// <summary>
        /// Two digit merchant code. Defaults to "00" (EFTPOS)
        /// </summary>
        [StringLength(2)]
        public string Merchant { get; set; } = "00";

        /// <summary>
        /// Type of status to perform. Defaults to '0' (standard)
        /// </summary>
        [StringLength(1)]
        public string StatusType { get; set; } = "0";

        /// <summary>
        /// Indicates where the request is to be sent to. Defaults to "00" (EFTPOS)
        /// </summary>
        [StringLength(2)]
        public string Application { get; set; } = "00";
    }
}
