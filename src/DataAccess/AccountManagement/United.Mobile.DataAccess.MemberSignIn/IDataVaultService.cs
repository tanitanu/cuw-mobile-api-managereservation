using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MemberSignIn
{
    public interface IDataVaultService
    {
        Task<string> GetPersistentToken(string token, string requestData,string url, string sessionId);
        Task<string> PersistentToken(string token, string requestData, string url, string sessionId);
        Task<string> GetCCTokenWithDataVault(string token, string requestData, string sessionId);
        Task<string> GetRSAWithDataVault(string token, string requestData, string sessionId);
        Task<(T response, long callDuration)> GetCSLWithDataVault<T>(string token, string action, string sessionId, string jsonRequest);
        Task<string> GetDecryptedTextFromDataVault(string token, string action, string sessionId, string jsonRequest);
    }
}
