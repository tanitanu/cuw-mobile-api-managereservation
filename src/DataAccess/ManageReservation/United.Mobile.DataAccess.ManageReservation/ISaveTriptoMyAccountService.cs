using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ManageReservation
{
    public interface ISaveTriptoMyAccountService
    {
        Task<string> SaveTriptoMyAccount(string token, string action, string request, string sessionId);
    }
}
