using System.Threading.Tasks;

namespace United.Mobile.DataAccess.BookingTrips
{
    public interface IBookingTripsService
    {
        Task<string> GetBookingTrips(string token, string requestData, string sessionId);
    }
}
