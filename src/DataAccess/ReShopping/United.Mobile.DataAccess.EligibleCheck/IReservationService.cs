using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ReShop
{
    public interface IReservationService
    {
        Task<string> ConfirmScheduleChange<T>(string token, string recordLocator, string sessionId);
    }
}