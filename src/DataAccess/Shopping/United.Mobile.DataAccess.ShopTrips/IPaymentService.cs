using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopTrips
{
    public interface IPaymentService
    {
        Task<string> GetEligibleFormOfPayments(string token, string path, string request, string sessionId);
        Task<string> GetFFCByEmail(string token, string path, string request, string sessionId);
        Task<string> GetFFCByPnr(string token, string path, string request, string sessionId);
        Task<string> GetLookUpTravelCredit(string token, string path, string request, string sessionId);
    }
}
