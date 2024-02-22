using System;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UpgradeList
{
    public interface IPPRStandbyUpgradeListService
    {
        Task<string> GetPPRStandbyUpgradeList(string token, string sessionId, string flightNumber, DateTime flightDateTime, string departureAirportCode);
    }
}