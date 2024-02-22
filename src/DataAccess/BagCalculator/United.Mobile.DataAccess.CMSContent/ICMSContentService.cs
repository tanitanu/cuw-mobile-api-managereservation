using System.Threading.Tasks;

namespace United.Mobile.DataAccess.CMSContent
{
    public interface ICMSContentService
    {
        Task<string> GetMobileCMSContentDetail(string token, string requestPayload, string sessionId);
        Task<string> GetMobileCMSContentMessages(string token, string requestData, string sessionId);
        Task<T> GetSDLContentByGroupName<T>(string token, string action, string request, string sessionId);
    }
}
