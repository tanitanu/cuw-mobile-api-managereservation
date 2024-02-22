using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Common.Helper.Profile
{
    public interface ISecurityQuestion
    {
        Task<List<Securityquestion>> GetMPPINPWDSecurityQuestions(string token, string sessionId, int appId, string appVersion, string deviceId);
        Task<List<Securityquestion>> GetMPPinPwdSavedSecurityQuestions(string token, int customerId, string mileagePlusNumber, string sessionId, int appId, string appVersion, string deviceId);
        Task<bool> MPPinPwdValidatePassowrd(string token, string langCode, string password, string mpdId, string username, string email, string sessionId, int appId, string appVersion, string deviceId);
        Task<bool> MPPinPwdUpdateCustomerPassword(string token, string oldPassword, string newPassword, int customerId, string mileagePlusNumber, string langCode, string sessionId, int appId, string appVersion, string deviceId);

        Task<bool> ValidateSecurityAnswer(string questionKey, string answerKey, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId);
        Task<bool> LockCustomerAccountWithSendEmailFlag(int customerId, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId, bool sendEmail);
        Task<bool> LockCustomerAccount(int customerId, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId);
        Task<bool> UnLockCustomerAccount(int customerId, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId);
    }
}
