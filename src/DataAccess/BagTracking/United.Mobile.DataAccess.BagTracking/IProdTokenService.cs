using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.BagTracking
{
    public interface IProdTokenService
    {
        Task<string> GetProdToken(string token, string sessionId, int applicationId, int appVersion, string accessCode, string transactionId, string languageCode);
    }
}
