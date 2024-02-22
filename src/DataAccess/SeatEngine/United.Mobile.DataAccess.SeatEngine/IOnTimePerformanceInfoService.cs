using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.SeatEngine
{
    public interface IOnTimePerformanceInfoService
    {
        Task<string> GetOnTimePerformance(string token, string url, string transactionId, int applicationid, string appVersion, string deviceId, string carrierCode, string flightNumber, string origin, string destination, string flightDate, string sessionId);
    }
}
