using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Payment
{
    public interface IGooglePayAccessTokenService
    {
        Task<string> GetGooglePayAccessToken(string token, string requestData, string sessionId);
    }
}
