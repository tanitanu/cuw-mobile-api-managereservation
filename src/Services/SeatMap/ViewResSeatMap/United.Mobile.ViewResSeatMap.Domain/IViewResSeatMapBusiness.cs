using System.Threading.Tasks;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;

namespace United.Mobile.ViewResSeatMap.Domain
{
    public interface IViewResSeatMapBusiness
    {
        Task<MOBSeatChangeSelectResponse> SelectSeats(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, string catalogValues = null);
        Task<MOBSeatChangeInitializeResponse> SeatChangeInitialize(MOBSeatChangeInitializeRequest request);
    }
}
