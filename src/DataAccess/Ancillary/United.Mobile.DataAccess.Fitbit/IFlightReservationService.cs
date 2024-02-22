using System.Threading.Tasks;
using UAWSFlightReservation;

namespace United.Mobile.DataAccess.Fitbit
{
    public interface IFlightReservationService
    {
        Task<wsFlightResResponse> GetFlightReservation(string recordLoactor, string lastName, string accessCode, string version);
        Task<string> AddPNRRemark(string request, string token, string suffixUrl);
    }
}
