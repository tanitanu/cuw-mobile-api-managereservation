using System.Threading.Tasks;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public interface IValidateHashPinService
    {
        Task<T> ValidateHashPin<T>(string accountNumber, string hashPinCode, int applicationId, string deviceId, string appVersion, string transactionId, string sessionId);
    }
}
