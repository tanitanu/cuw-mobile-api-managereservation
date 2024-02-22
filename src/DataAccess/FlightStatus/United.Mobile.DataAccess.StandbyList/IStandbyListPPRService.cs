using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.StandbyList
{
    public interface IStandbyListPPRService
    {
        Task<string> GetStandbyListPPR(string token, string sessionId, string flightNumber, DateTime flightDate, string departureAirportCode);
    }
}
