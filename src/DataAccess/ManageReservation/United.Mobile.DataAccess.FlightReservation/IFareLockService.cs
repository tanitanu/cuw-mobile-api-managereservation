using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightReservation
{
    public interface IFareLockService
    {
        Task<string> GetPNRManagement(string token, string mileagePlusNumber, string sessionId);
    }
}
