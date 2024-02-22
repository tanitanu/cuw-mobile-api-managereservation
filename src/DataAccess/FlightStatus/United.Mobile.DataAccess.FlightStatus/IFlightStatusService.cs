using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightStatus
{
    public interface IFlightStatusService
    {
        Task<string> GetFlightStatusDetails(string token, string transactionId, string flightNumber, string flightDate, string origin);
        Task<string> GetFlightStatusDetails(string token, string transactionId, string flightNumber, string flightDate, string origin, string carrierCode);
    }
}
