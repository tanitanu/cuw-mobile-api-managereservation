using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MicrositeServices
{
    public interface IPrepaidBagsService
    {
        Task<(string response, long callDuration)> GetMicrositeService(string token, string sessionId, string action);
        Task<(string response, long callDuration)> GetDynamicOffers(string token, string sessionId, string action, string request);
    }
}
