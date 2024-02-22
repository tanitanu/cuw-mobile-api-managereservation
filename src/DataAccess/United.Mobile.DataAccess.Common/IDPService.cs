using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public interface IDPService
    {
        public Task<string> GetAnonymousToken(int applicationId, string deviceId, IConfiguration configuration);
        public Task<string> GetAnonymousToken(int applicationId, string deviceId, IConfiguration configuration, string configSectionKey);

        Task<string> GetAnonymousTokenV2(int applicationId, string deviceId, IConfiguration configuration, string configSectionKey, bool saveToCache = false);
        Task<string> GetAndSaveAnonymousToken(int applicationId, string deviceId, IConfiguration configuration, string configSectionKey, Session session);
        Task<string> GetJWTToken(int applicationId, string deviceId, IConfiguration configuration, string configSectionKey);
        Task<DPTokenResponse> GetSSOToken(int applicationId, string mpNumber, IConfiguration configuration, string configSSOSectionKey = "dpSSOTokenOption", string dpSSOConfig = "dpSSOTokenConfig", IResilientClient resilientSSOClient = null);
        public string GetSSOTokenString(int applicationId, string mpNumber, IConfiguration configuration, string configSSOSectionKey = "dpSSOTokenOption", string dpSSOConfig = "dpSSOTokenConfig", IResilientClient resilientSSOClient = null);
    }
}