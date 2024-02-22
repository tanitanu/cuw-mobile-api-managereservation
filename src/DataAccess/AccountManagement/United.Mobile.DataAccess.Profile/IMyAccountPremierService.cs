using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Profile
{
    public interface IMyAccountPremierService
    {
        Task<T> GetAccountPremier<T>(string token, string accountNumber, string accessCode, string sessionId);
    }
}
