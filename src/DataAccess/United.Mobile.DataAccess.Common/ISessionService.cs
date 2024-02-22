using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.DataAccess.Common
{
    public interface ISessionService
    {
        Task<string> GetSessionId<T>(string objectName, string transactionId, string deviceId, string appId, TimeSpan expiry);
        Task<string> GetSession<T>(string sessionId, string transactionId, string objectName, string deviceId, string appId, TimeSpan expiry);
        Task<bool> SaveSession<T>(string sessionId, string transactionId, string objectName, T data, string deviceId, string appId, TimeSpan expiry);
        Task<(bool IsValidPersistData, Response<P> InvalidSessionData, T PersistData)> GetSessionResponse<T, P>(string sessionId, string transactionId, string deviceId, string appId);
    }
}
