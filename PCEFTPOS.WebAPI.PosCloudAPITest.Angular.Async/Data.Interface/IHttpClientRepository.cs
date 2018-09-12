using System.Net.Http;
using System.Threading.Tasks;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data.Interface
{
    /// <summary>
    /// Http Clients repository
    /// </summary>
    public interface IHttpClientRepository
    {
        /// <summary>
        /// Get token
        /// </summary>
        /// <returns></returns>
        Task<TokenResponse> GetTokenAsync();

        /// <summary>
        /// Get API client
        /// </summary>
        /// <returns></returns>
        HttpClient GetAPIClient();

        /// <summary>
        /// Get authentication client
        /// </summary>
        /// <returns></returns>
        HttpClient GetAuthClient();
    }
}
