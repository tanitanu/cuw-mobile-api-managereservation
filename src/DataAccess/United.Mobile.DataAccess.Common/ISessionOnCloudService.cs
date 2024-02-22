using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;

namespace United.Mobile.DataAccess.Common
{
    public interface ISessionOnCloudService
    {
        Task<string> GetSessionId(string transactionId, string mpNumber, string deviceId, string appId);

        Task<string> GetSession(string sessionId, string objectName, string transactionId, string mpNumber, string deviceId, string appId);
        Task<string> GetSession(string sessionID, string objectName, List<string> vParams = null, string transactionId = "Test", bool isReadOnPrem = false);
        Task<Dictionary<string, dynamic>> GetAllSession<T>(string sessionId, string transactionId, string mpNumber, string deviceId, string appId, string objectName);

        //Save Method
        Task<bool> SaveSessionONCloud<T>(T data, string sessionID, List<string> validateParams, string objectName, TimeSpan expiry, string transactionId = "Test");
        Task<bool> SaveSessionONCloudOnPrem(string data, string sessionID, List<string> validateParams, string objectName, TimeSpan expiry, string transactionId = "Test", bool SaveXMLFormatToOnPrem = false);
        Task<bool> SaveSessionOnPrem(string data, string sessionID, List<string> validateParams, string objectName, TimeSpan expiry, string transactionId = "Test");
        Task<(bool IsValidPersistData, Response<P> InvalidSessionData, T PersistData)> GetSessionResponse<T, P>(string sessionId, string transactionId, string deviceId, string appId);
    }
}
