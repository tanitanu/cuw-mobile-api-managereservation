using System.Collections.Generic;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightStatus
{
    public interface IFlightRequestSqlSpOnPremService
    {
        Task<List<string>> GetCabinCountByShipNumber(List<string> ship, string sessionId);
    }
}
