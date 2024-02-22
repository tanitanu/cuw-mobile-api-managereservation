using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Loyalty
{
    public interface ILoyaltyAccountService
    {
        Task<string> GetCurrentMembershipInfo(string mPNumber);
        Task<T> GetAccountProfileInfo<T>(string token, string mileagePlusNumber, string sessionId);
        Task<T> GetAccountSummary<T>(string token, string mileagePlusNumber, string sessionId);
        Task<string> GetAccountProfile(string token, string mileagePlusNumber, string sessionId);
    }
}
