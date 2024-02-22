using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UpgradeList
{
    public interface ICabinBrandingService
    {
        Task<string> GetCabinBranding(string token, string sessionId, string jsonRequest);
    }
}