using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UAWSPNRServiceERes;

namespace United.Mobile.DataAccess.FlightReservation
{
    public interface IPNRServiceEResService
    {
         Task<COWSReturnType> GetEmployeePNRs(string employeeID, string accessCode, string activePNROnly);
    }
}
