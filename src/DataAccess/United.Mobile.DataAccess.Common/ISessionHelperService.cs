using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;

namespace United.Mobile.DataAccess.Common
{
    public interface ISessionHelperService
    {
        //Get Methods
        Task<string> GetSessionId(HttpContextValues contentType, string mpNumber);
        Task<Dictionary<string, dynamic>> GetAllSession<T>(HttpContextValues contentType, string mpNumber, string objectName);
        Task<(bool IsValidPersistData, Response<P> InvalidSessionData, T PersistData)> GetSessionResponse<T, P>(string sessionId, string transactionId, string deviceId, string appId);
        Task<string> GetSession<T>(HttpContextValues contextValues, string objectName, string sessionId);
        Task<T> GetSession<T>(HttpContextValues ContextValues, string objectName = "", List<string> listOfParams = null, string sessionID = "");
        Task<T> GetSession<T>(string sessionID, string objectName, List<string> vParams = null, bool isReadOnPrem = false);

        //Save Method
        Task<bool> SaveSession<T>(T data, HttpContextValues contextValues, string objectName = "", List<string> listOfParams = null, string sessionID = "");
        Task<bool> SaveSession<T>(T data, string sessionID, List<string> validateParams, string objectName = "", int sessionTimeSpanInSecs = 5400, bool saveJsonOnCloudXMLOnPrem = false);

        Task<T> GetSession<T>(string sessionID, string objectName, int temp, List<string> vParams = null);

        Task<bool> SaveSessions<T>(T data, string sessionID, List<string> validateParams, string objectName = "");
    }
}
