using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.CancelReservation
{
    public interface ICancelReservationService
    {
        Task<string> CancelReservation(string token, string requestData, string sessionId);
    }
}
