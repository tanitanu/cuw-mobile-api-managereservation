using System;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public interface IMileagePlusCSSTokenService
    {
        Task<bool> UpdateMileagePlusCSSToken(string sessionId, string transactionId, string mpNumber, string deviceID, int appID, string appVersion, string authToken, bool isAuthTokenValid, DateTime authTokenExpirationDateTime, double tokenExpireInSeconds, bool isTokenAnonymous = false, long customerID = 0);
    }
}
