using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerProfileService
    {
        Task<T> GetAccountStatus<T>(string mileagPlusNumber, string token, string sessionId);
    }
}
