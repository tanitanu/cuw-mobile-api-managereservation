using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.BagTracking
{
    public interface IPassengersBagsDetailsService
    {
        Task<string> GetPassengersBagsDetails(string token, string PNRInfo, string BagTrackNumber, string sessionId);
    }
}
