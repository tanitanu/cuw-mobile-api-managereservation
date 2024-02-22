using System.Threading.Tasks;

namespace United.Mobile.DataAccess.SeatEngine
{
    public interface ISeatMapAvailabilityService
    {
        Task<string> GetCSL30SeatMap(string token, string channelId, string channelName, string flightNumber, string departureAirportCode, string arrivalAirportCode, string flightDate, string marketingCarrierCode, string OperatingCarrierCode, string sessionId, string transactionId);
    }
}
