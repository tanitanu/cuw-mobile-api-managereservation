using System.Threading.Tasks;

namespace United.Mobile.DataAccess.StandbyList
{
    public interface IPolarisCabinBrandingService
    {
        Task<string> GetPolarisCabinBranding(string token, string request, string sessionId);
    }
}
