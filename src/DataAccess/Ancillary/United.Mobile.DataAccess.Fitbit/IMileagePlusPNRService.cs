using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Fitbit
{
    public interface IMileagePlusPNRService
    {
        Task<string> GetPNRStatus(string token, string accesscode, string transactionid, string PNRList, string ApplicationType, string messageformat, string sessionId);
        Task<string> GetBoardingPass(string token, string sessionId, string accessCode, string transactionId, string recordLocator, string applicationName, string applicationVersion);
        Task<string> GetCountryName(string token, string accesscode, string transactionid, string countryCode, string sessionId);
    }
}
