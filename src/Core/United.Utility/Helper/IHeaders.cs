using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using United.Mobile.Model;

namespace United.Common.Helper
{
    public interface IHeaders
    {
        HttpContextValues ContextValues { get; set; }        
        Task<bool> SetHttpHeader(string deviceId, string applicationId, string appVersion, string transactionId, string languageCode, string sessionId);
        Task<bool> SetHttpHeader();
        void SetHeadersWhenNoHttpContext(string deviceId, string applicationId, string transactionId, string languageCode, string sessionId, string appVersion = "4.1.55");
    }
}
