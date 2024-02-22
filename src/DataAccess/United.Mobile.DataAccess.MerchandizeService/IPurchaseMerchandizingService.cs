using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MerchandizeService
{
    public interface IPurchaseMerchandizingService
    {
        Task<(T response, long callDuration)> GetInflightPurchaseInfo<T>(string token, string action, string request, string sessionId);
        Task<T> GetMerchOfferInfo<T>(string token, string action, string request, string sessionId);
        Task<T> GetVendorOfferInfo<T>(string token, string request, string sessionId);
        Task<(T response, long callDuration)> GetInflightPurchaseEligibility<T>(string token, string request, string sessionId);
    }
}
