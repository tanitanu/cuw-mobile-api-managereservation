using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ScheduleChange
{
    public interface IReservationService
    {
        Task<string> ConfirmScheduleChange<T>(string token, string recordLocator, string sessionId);
    }
}