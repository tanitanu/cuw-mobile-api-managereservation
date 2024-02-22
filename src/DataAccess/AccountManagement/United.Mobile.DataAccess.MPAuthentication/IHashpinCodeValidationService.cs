using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public interface IHashpinCodeValidationService
    {
        Task<bool> ValidateHashPinCode(string accountNumber, string hashPinCode, int applicationId, string deviceId, string appVersion, string sessionId);
    }
}
