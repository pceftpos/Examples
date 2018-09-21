using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Data.Interface
{
    public interface ITokenRepository
    {
        PosNotifyToken GetPosNotifyToken(string session);
    }
}
