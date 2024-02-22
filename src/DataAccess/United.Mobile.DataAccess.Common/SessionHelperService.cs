using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Utility.Helper;
using Constants = United.Mobile.Model.Constants;

namespace United.Mobile.DataAccess.Common
{
    public class SessionHelperService : ISessionHelperService
    {
        private readonly ICacheLog<SessionHelperService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionOnCloudService _sessionOnCloudService;
        private readonly IApplicationEnricher _applicationEnricher;

        public string SessionID { get; set; }

        public SessionHelperService(ICacheLog<SessionHelperService> logger
            , IConfiguration configuration
            , ISessionOnCloudService sessionOnCloudService
            , IApplicationEnricher applicationEnricher)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionOnCloudService = sessionOnCloudService;
            _applicationEnricher = applicationEnricher;
        }


        public async Task<string> GetSessionId(HttpContextValues contentType, string mpNumber)
        {
            var transID = string.IsNullOrEmpty(contentType.TransactionId) ? "Trans01" : contentType.TransactionId;

            //GetSessionId  for requested MP number from OnCloud Service
            SessionID = await _sessionOnCloudService?.GetSessionId(transID, mpNumber, contentType.DeviceId, contentType.Application.Id.ToString());

            _applicationEnricher.Add(Constants.SessionId, SessionID);

            //Always return OnCloud value unless it is null otherwise it will return onPremise
            return SessionID;
        }

        public async Task<Dictionary<string, dynamic>> GetAllSession<T>(HttpContextValues contentType, string mpNumber, string objectName)
        {
            //GetAllSession for requested MPnumber from OnCloud ; there is no OnPremise Service
            return await _sessionOnCloudService?.GetAllSession<T>(contentType.SessionId, contentType.TransactionId, mpNumber, contentType.DeviceId, contentType.Application.Id.ToString(), objectName);
        }

        public async Task<(bool IsValidPersistData, Response<P> InvalidSessionData, T PersistData)> GetSessionResponse<T, P>(string sessionId, string transactionId, string deviceId, string appId)
        {
            return await _sessionOnCloudService?.GetSessionResponse<T, P>(sessionId, transactionId, deviceId, appId);
        }


        public async Task<T> GetSession<T>(HttpContextValues contextValues, string objectName = "", List<string> listOfParams = null, string sessionID = "")
        {
            var sessID = string.IsNullOrEmpty(sessionID) ? contextValues.SessionId : sessionID;
            return await GetSession<T>(sessID, contextValues.TransactionId, (listOfParams != null) ? listOfParams[0] : string.Empty,
                contextValues.DeviceId, contextValues.Application.Id.ToString(), objectName);
        }

        private async Task<T> GetSession<T>(string sessionId, string transactionId, string mpNumber, string deviceId, string appId, string objectName = "")
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return default;
            }

            transactionId = string.IsNullOrEmpty(transactionId) ? "test" : transactionId;

            try
            {
                //  getsessionValueOnCloud 
                var sessionResponse = await _sessionOnCloudService?.GetSession(sessionId, objectName, transactionId, sessionId, deviceId, appId);

                if (!string.IsNullOrEmpty(sessionResponse))
                {
                    var sessionData = JsonConvert.DeserializeObject<SessionResponse>(sessionResponse);
                    if (!string.IsNullOrEmpty(sessionData?.Data) && sessionData.Succeed)
                    {
                        try
                        {
                            var jsonValue = JsonConvert.DeserializeObject<T>(sessionData.Data);
                            if (!string.IsNullOrEmpty(jsonValue?.Data))
                            {
                                return jsonValue.Data;
                            }
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            var deserValue = (sessionData.Data.Contains(@"<?xml")) ?
                                           XmlSerializerHelper.GetObjectFromXmlData<T>(sessionData.Data) :
                                           JsonConvert.DeserializeObject<T>(sessionData.Data);
                            return deserValue;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }
            catch (Exception)
            { }

            return default;
        }

        public async Task<T> GetSession<T>(string sessionID, string objectName, List<string> vParams = null, bool isReadOnPrem = false)
        {
            if (string.IsNullOrEmpty(sessionID))
            {
                return default;
            }

            string sessionObjectName = (string.IsNullOrEmpty(objectName)) ? typeof(T).FullName : objectName;

            try
            {
                //  getsessionValueOnCloud 
                //isReadOnPrem = isReadOnPrem ? isReadOnPrem : _configuration.GetValue<bool>("loadSessionFromOnPremCouchbase");
                var sessionResponse = await _sessionOnCloudService?.GetSession(sessionID, objectName, vParams, sessionID, isReadOnPrem);
                bool isSessionDataSucceed = false;
                if (!string.IsNullOrEmpty(sessionResponse))
                {
                    var sessionData = JsonConvert.DeserializeObject<SessionResponse>(sessionResponse);

                    if (!string.IsNullOrEmpty(sessionData?.Data) && sessionData.Succeed)
                    {
                        try
                        {
                            if(typeof(T) == typeof(string))
                            {
                                return sessionData.Data;
                            }
                            return JsonConvert.DeserializeObject<T>(sessionData.Data); 
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            T typeInstance = default(T);
                            StringReader memoryStream = new StringReader(sessionData.Data);
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                            typeInstance = (T)xmlSerializer.Deserialize(memoryStream);
                            return typeInstance;
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            var deserValue = (sessionData.Data.Contains(@"<?xml")) ?
                                           XmlSerializerHelper.GetObjectFromXmlData<string>(sessionData.Data) :
                                           JsonConvert.DeserializeObject<T>(sessionData.Data);

                            return await Task.FromResult(JsonConvert.DeserializeObject<T>(deserValue));
                        }
                        catch (Exception)
                        {
                        }
                    }
                    isSessionDataSucceed = sessionData.Succeed;

                }

                ///This code is removed once we move all API's (Booking, MangRes,ResShop, MPSign)  to cloud
                //MOBILE-36814 : Removing GetSessionV2 (GetSessionFromOnPrem) from ManageRes Cloud repo services after SaveSessionOnCouchbase turned off
                if (_configuration.GetValue<bool>("EnableGetSessionFromOnPrem") && !isSessionDataSucceed && !isReadOnPrem)
                {
                    ///This code is removed once we move all API's (Booking, MangRes,ResShop, MPSign)  to cloud
                    return await GetSession<T>(sessionID, objectName, vParams, true);
                }
            }
            catch (Exception ex)
            { throw ex; }

            return default;
        }

        public async Task<T> GetSession<T>(string sessionID, string objectName, int temp, List<string> vParams = null)
        {

            if (string.IsNullOrEmpty(sessionID))
            {
                return default;
            }

            string sessionObjectName = (string.IsNullOrEmpty(objectName)) ? typeof(T).FullName : objectName;

            try
            {
                _logger.LogInformation("GetSession - request  {sessionID} {objectName} {validationParams}", sessionID, objectName, vParams);
                //  getsessionValueOnCloud 
                var sessionResponse = await _sessionOnCloudService?.GetSession(sessionID, objectName, vParams);

                _logger.LogInformation("GetSession - {response} {sessionID}", sessionResponse, sessionID);

                if (!string.IsNullOrEmpty(sessionResponse))
                {
                    var sessionData = JsonConvert.DeserializeObject<SessionResponse>(sessionResponse);

                    if (!string.IsNullOrEmpty(sessionData?.Data) && sessionData.Succeed)
                    {
                        try
                        {
                            T xmlResult = default(T);
                            StringReader memoryStream = new StringReader(sessionData.Data);
                            var xmlSerializer = new XmlSerializer(typeof(T));
                            xmlResult = (T)xmlSerializer.Deserialize(memoryStream);

                            //StringWriter myWriter = new StringWriter(sessionData.Data);
                            //XmlSerializer s = new XmlSerializer(typeof(T));
                            //xmlResult = (T)s.Deserialize(myWriter);
                            //return XmlSerializerHelper.GetObjectFromXmlData<T>(sessionData.Data);
                            return xmlResult;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }
            catch (Exception)
            { }

            return default;
        }


        public async Task<string> GetSession<T>(HttpContextValues contextValues, string objectName, string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return default;
            }

            var transactionId = string.IsNullOrEmpty(contextValues.TransactionId) ? "test" : contextValues.TransactionId;

            //GetSessionValue from OnCloud Service
            objectName = string.IsNullOrEmpty(objectName) ? typeof(T).FullName : objectName;
            return await _sessionOnCloudService?.GetSession(sessionId, objectName, new List<string> { sessionId, objectName });
            //return await _sessionOnCloudService?.GetSession(sessionId, objectName, transactionId, string.Empty, contextValues.DeviceId, contextValues.Application.Id.ToString());
        }

        public async Task<bool> SaveSession<T>(T data, HttpContextValues contextValues, string objectName = "", List<string> listOfParams = null, string sessionID = "")
        {
            var transactionId = string.IsNullOrEmpty(contextValues.TransactionId) ? "test" : contextValues.TransactionId;
            var sessionId = string.IsNullOrEmpty(sessionID) ? contextValues.SessionId : sessionID;
            var sessionTimeExpiry = TimeSpan.FromSeconds(_configuration.GetValue<double>("SessionDataExp") == 0 ? 10 : _configuration.GetValue<double>("SessionDataExp"));

            try
            {
                //SaveSessionOnCloud Service is called for Guid Generated by OnPrem Services
                var saveOnCloud = await _sessionOnCloudService.SaveSessionONCloud<T>(data, sessionId, new List<string> { sessionId, objectName }, objectName, sessionTimeExpiry, transactionId);
                return saveOnCloud;
            }
            catch (Exception)
            { }
            return false;
        }

        public async Task<bool> SaveSession<T>(T data, string sessionID, List<string> validateParams, string objectName = "", int sessionTimeSpanInSecs = 5400, bool saveJsonOnCloudXMLOnPrem = false)
        {
            var sessionObjectName = (string.IsNullOrEmpty(objectName)) ? typeof(T).FullName : objectName;
            string saveData = string.Empty;

            try
            {
                saveData = JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveSession Data serialize {ErrorMessage} {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                throw ex;
            }

            if (!_configuration.GetValue<bool>("SavePersistInXML"))
            {
                if (saveJsonOnCloudXMLOnPrem == true)
                {
                    //SaveSessionValue from OnCloud Service (CSLShop
                    return await _sessionOnCloudService?.SaveSessionONCloudOnPrem(saveData, sessionID, validateParams, sessionObjectName, TimeSpan.FromSeconds(sessionTimeSpanInSecs), sessionID, SaveXMLFormatToOnPrem: false);
                }
                else
                {
                    //SaveSessionValue from OnCloud Service
                    await _sessionOnCloudService?.SaveSessionONCloud<T>(data, sessionID, validateParams, sessionObjectName, TimeSpan.FromSeconds(sessionTimeSpanInSecs), sessionID);
                    try
                    {
                        saveData = XmlSerializerHelper.SaveObjectFromXmlData<T>(data);

                        await _sessionOnCloudService.SaveSessionOnPrem(saveData, sessionID, validateParams, sessionObjectName, TimeSpan.FromSeconds(sessionTimeSpanInSecs), sessionID).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Exception - SaveSessionOnPrem {@Exception}, {@ValidateParam} {@SessionId}", JsonConvert.SerializeObject(ex), JsonConvert.SerializeObject(validateParams), sessionID);
                    }

                }
                return true;
            }
            else
            {
                return await _sessionOnCloudService.SaveSessionONCloud(saveData, sessionID, validateParams, sessionObjectName, TimeSpan.FromSeconds(sessionTimeSpanInSecs), sessionID).ConfigureAwait(false);
            }
        }
        public async Task<bool> SaveSessions<T>(T data, string sessionID, List<string> validateParams, string objectName = "")
        {
            var sessionObjectName = (string.IsNullOrEmpty(objectName)) ? typeof(T).FullName : objectName;
            var sessionTimeExpiry = TimeSpan.FromSeconds(_configuration.GetValue<double>("SessionDataExp") == 0 ? 10 : _configuration.GetValue<double>("SessionDataExp"));
            try
            {

                if (!_configuration.GetValue<bool>("SavePersistInXML"))
                {
                    var saveData = XmlSerializerHelper.SaveObjectFromXml<T>(data);
                    //SaveSessionValue from OnCloud Service
                    return await _sessionOnCloudService?.SaveSessionONCloudOnPrem(saveData, sessionID, validateParams, sessionObjectName, sessionTimeExpiry, sessionID);
                }
                else
                {

                    return await _sessionOnCloudService.SaveSessionONCloud<T>(data, sessionID, new List<string> { sessionID, objectName }, objectName, sessionTimeExpiry, sessionID);

                }
            }
            catch (Exception)
            { }
            return false;
        }


    }
}
