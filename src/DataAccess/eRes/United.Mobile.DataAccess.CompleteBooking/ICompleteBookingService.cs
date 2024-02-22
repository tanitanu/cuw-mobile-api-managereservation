using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.CompleteBooking
{
    public interface ICompleteBookingService
    {
        Task<string> CompleteBooking(string token, string requestPayload, string sessionId);
    }
}
