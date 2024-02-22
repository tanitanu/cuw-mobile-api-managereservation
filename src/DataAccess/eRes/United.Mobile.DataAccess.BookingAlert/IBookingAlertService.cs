using System.Threading.Tasks;

namespace United.Mobile.DataAccess.BookingAlert
{
    public interface IBookingAlertService
    {
        Task<string> GetBookingAlert(string token, string requestData, string sessionId);
    }
}
