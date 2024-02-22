using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface IDPTokenService
    {   
        Task<string> GetTokenFromAWSDPAsync(string loggingContext, int applicationId, string deviceId, bool checkCache = true);
    }
}
