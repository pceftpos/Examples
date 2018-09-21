using System.ComponentModel.DataAnnotations;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTStatusRequest : EFTRequest
    {
        public EFTStatusRequest() : base(true, typeof(EFTStatusResponse))
        {
        }

        [StringLength(2)]
        public string Merchant { get; set; } = "00";

        [StringLength(1)]
        public string StatusType { get; set; } = "0";

        [StringLength(2)]
        public string Application { get; set; } = "00";
    }
}
