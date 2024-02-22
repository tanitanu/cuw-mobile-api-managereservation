using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightStatus
{
    public interface IFlifoScheduleService
    {
        Task<string> FlifoScheduleResponse(string inventoryRequest, string session);
    }
}
