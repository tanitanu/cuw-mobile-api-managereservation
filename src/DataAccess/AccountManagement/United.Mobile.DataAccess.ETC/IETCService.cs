using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UAWSMPTravelCertificateService.ETCServiceSoap;

namespace United.Mobile.DataAccess.ETC
{
    public interface IETCService
    {
        Task<ETCReturnType> ETCSearchByFreqFlyerNum(string freqFlyerNum, string stationID, string dutyCode, string agentSine, string lineIATA, string accessCode, string sessionID);
    }
}
