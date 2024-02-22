using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface IPersistToken
    {
        Task<string> GetDpToken(string sessionId, string transactionId, string deviceId, string appId);
    }
}
