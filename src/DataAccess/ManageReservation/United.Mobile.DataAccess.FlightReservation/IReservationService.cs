using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightReservation
{
    public interface IReservationService
    {
        Task<string> GetCheckInStatus(string token, string mileagePlusNumber, string sessionId);
    }
}
