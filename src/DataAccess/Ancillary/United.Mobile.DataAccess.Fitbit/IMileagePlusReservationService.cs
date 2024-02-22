using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UAWSMileagePlusReservation;

namespace United.Mobile.DataAccess.Fitbit
{
    public interface IMileagePlusReservationService
    {
        Task<wsReservationResponse> GetPNRsByMileagePlusNumber(string mileagePlusNumber, string accessCode, string version);
    }
}
