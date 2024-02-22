using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UnitedClub
{
    public interface IPersistentTokenByAccountNumberTokenService
    {
        Task<string> GetPersistentTokenUsingAccountNumberToken(string token, string accountNumberToken, string sessionId);
    }
}
