using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.PassRiderList
{
    public interface ITravelerMisConnectService
    {
        Task<string> GetPassengerMisConnectDetails(string token, string requestData, string sessionId);
    }
}
