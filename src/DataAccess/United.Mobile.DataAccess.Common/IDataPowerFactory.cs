using System.Threading.Tasks;
using United.Foundations.Practices.Framework.Security.DataPower.Models;
using United.Mobile.Model.Common;

namespace United.Mobile.DataAccess.Common
{
    public interface IDataPowerFactory
    {
        Task<bool> CheckIsDPTokenValid(string _dpAccessToken, Session shopTokenSession, string transactionId, bool SaveToPersist = true);
        DPAccessTokenResponse GetDPAuthenticatedToken(int applicationID, string deviceId, string transactionId, string appVersion, Session TokenSessionObj, string username, string password, string usertype, string anonymousToken, DpRequest dpRequest, bool SaveToPersist = true);
    }
}
