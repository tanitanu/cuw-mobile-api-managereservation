using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Loyalty
{
    public interface ILoyaltyMemberProfileService
    {
        Task<T> GetAccountMemberProfile<T>(string token, string mileagePlusNumber, string sessionId);
    }
}
