using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ReShop
{
    public interface IRefundService
    {
        //Task<T> EligibleCheck<T>(string token, string sessionId, string recordLocator, string appId, bool isAllowAward, bool allowFFC);
        Task<string> PostEligibleCheck<T>(string token, string sessionId,  string path, string request);
        Task<string> GetEligibleCheck<T>(string token, string sessionId,  string path);
    }
}