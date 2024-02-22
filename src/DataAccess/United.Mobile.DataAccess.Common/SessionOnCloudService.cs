using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Common;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class SessionOnCloudService : ISessionOnCloudService
    {
        private readonly ICacheLog<SessionOnCloudService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IResilientClient _resilientClient;
        public SessionOnCloudService([KeyFilter("sessionOnCloudConfigKey")] IResilientClient resilientClient, ICacheLog<SessionOnCloudService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetSessionId(string transactionId, string mpNumber, string deviceId, string appId)
        {
            using (_logger.BeginTimedOperation("Total time taken for Service call GetSessionId On Cloud", transationId: transactionId))
            {
                var vparams = new List<string>();
                if (!string.IsNullOrEmpty(appId)) vparams.Add(appId);
                if (!string.IsNullOrEmpty(deviceId)) vparams.Add(deviceId);
                if (!string.IsNullOrEmpty(mpNumber)) vparams.Add(mpNumber);

                SessionIdRequest sessionIdRequest = new SessionIdRequest()
                {
                    TransactionId = transactionId,
                    ValidationParams = vparams
                };

                string requestData = JsonConvert.SerializeObject(sessionIdRequest);
                var getResult = await _resilientClient.PostHttpAsyncWithOptions("GetSessionId", requestData);

                if (getResult.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetSessionId {requestUrl} error {@response} for {transactionId}", getResult.url, JsonConvert.SerializeObject(getResult.response), transactionId);
                    throw new Exception();
                }

                var sessionData = JsonConvert.DeserializeObject<SessionResponse>(getResult.response);
                _logger.LogInformation("GetSessionId {requestUrl} Information {sessionData} for {transactionId}", getResult.url, sessionData.SessionId, transactionId);
                return sessionData.SessionId;
            }
        }

        public async Task<string> GetSession(string sessionId, string objectName, string transactionId, string mpNumber, string deviceId, string appId)
        {
            var vparams = new List<string>();
            if (!string.IsNullOrEmpty(sessionId)) vparams.Add(sessionId);
            if (!string.IsNullOrEmpty(objectName)) vparams.Add(objectName);
            //   if (!string.IsNullOrEmpty(mpNumber)) vparams.Add(mpNumber);

            SessionRequest sessionRequest = new SessionRequest()
            {
                TransactionId = transactionId,
                ValidationParams = vparams,
                SessionId = sessionId,

                ObjectName = objectName
            };
            var requestData = JsonConvert.SerializeObject(sessionRequest);
            var sessionReturnValue = await _resilientClient?.PostHttpAsync("GetSession", requestData, null, "application/json");
            var response = sessionReturnValue.Item1;
            var statusCode = sessionReturnValue.Item2;
            var url = sessionReturnValue.Item3;

            if ((statusCode != HttpStatusCode.OK) && (statusCode != HttpStatusCode.NotFound))
            {
                _logger.LogError("GetSession  {requestUrl} error {response} for {sessionId}", url, response, sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception();
            }

            return response;

        }

        public async Task<string> GetSession(string sessionID, string objectName, List<string> vParams = null, string transactionId = "Test", bool isReadOnPrem = false)
        {
            SessionRequest sessionRequest = new SessionRequest()
            {
                TransactionId = transactionId,
                ValidationParams = vParams,
                SessionId = sessionID,

                ObjectName = objectName
            };
            var requestData = JsonConvert.SerializeObject(sessionRequest);

            var endpoint = isReadOnPrem ? $"GetSessionV2?isReadOnPrem={isReadOnPrem}" : $"GetSession";
            _logger.LogInformation("GetSession {endpoint} - request {RequestData},{@SessionId}", endpoint, requestData, sessionID);

            var sessionReturnValue = await _resilientClient?.PostHttpAsyncWithOptions(endpoint, requestData);

            if ((sessionReturnValue.statusCode != HttpStatusCode.OK) && (sessionReturnValue.statusCode != HttpStatusCode.NotFound))
            {
                _logger.LogError("GetSession  {requestUrl}, error {response} for {@SessionID}", sessionReturnValue.url, sessionReturnValue.response, sessionID);
                if (sessionReturnValue.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception();
            }
            _logger.LogInformation("GetSession {requestUrl}, {response} for {@SessionID}", sessionReturnValue.url, sessionReturnValue.response, sessionID);

            return sessionReturnValue.response;
        }

        public async Task<Dictionary<string, dynamic>> GetAllSession<T>(string sessionId, string transactionId, string mpNumber, string deviceId, string appId, string objectName)
        {
            var vparams = new List<string>();
            if (!string.IsNullOrEmpty(appId)) vparams.Add(appId);
            if (!string.IsNullOrEmpty(deviceId)) vparams.Add(deviceId);
            if (!string.IsNullOrEmpty(mpNumber)) vparams.Add(mpNumber);

            SessionRequest sessionRequest = new SessionRequest()
            {
                TransactionId = transactionId,
                ValidationParams = vparams,
                SessionId = sessionId,

            };

            using (_logger.BeginTimedOperation("Total time taken for Service call GetAllSession On Cloud", transationId: transactionId))
            {
                var requestData = JsonConvert.SerializeObject(sessionRequest);
                var sessionReturnValue = await _resilientClient.PostHttpAsyncWithOptions("GetSession", requestData);

                if (sessionReturnValue.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetAllSession  {requestUrl} error {@response} for {transactionId}", sessionReturnValue.url, sessionReturnValue.response, transactionId);
                    if (sessionReturnValue.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception();
                }

                var sessionData = JsonConvert.DeserializeObject<SessionResponse>(sessionReturnValue.response);
                _logger.LogInformation("GetAllSession  {requestUrl}, {@response} and {transactionId}", sessionReturnValue.url, sessionReturnValue.response, transactionId);

                if (sessionData.Data != null)
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(sessionData.Data));
                }
            }
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(string.Empty);
        }

        public async Task<bool> SaveSessionONCloud<T>(T data, string sessionID, List<string> validateParams, string objectName, TimeSpan expiry, string transactionId = "Test")
        {
            SaveSessionRequest saveSessionRequest = new SaveSessionRequest()
            {
                TransactionId = transactionId,
                SessionId = sessionID,
                ObjectName = objectName,
                Data = data,
                ValidationParams = validateParams,
                ExpirationOptions = new ExpirationOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddMinutes(90),
                    SlidingExpiration = expiry
                }
            };
            if (!_configuration.GetValue<bool>("DisableSaveSessionDataNullFix") && data == null)
            {
                saveSessionRequest.Data = "null";
            }
            try
            {
                var requestData = JsonConvert.SerializeObject(saveSessionRequest);
                _logger.LogInformation("SaveSessionONCloud {requestData} and {@SessionId}", requestData, sessionID);

                var savedResult = await _resilientClient.PostHttpAsyncWithOptions("SaveSessionONCloudV2", requestData);

                if (savedResult.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("SaveSessionONCloud {requestUrl} error {response} and {@SessionId}", savedResult.url, savedResult.response, sessionID);
                    if (savedResult.statusCode != HttpStatusCode.BadRequest)
                        return false;
                }

                _logger.LogInformation("SaveSessionONCloud {requestUrl}, {response} and {@SessionId}", savedResult.url, savedResult.response, sessionID);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveSessionONCloudOnPrem(string data, string sessionID, List<string> validateParams, string objectName, TimeSpan expiry, string transactionId = "Test", bool SaveXMLFormatToOnPrem = false)
        {
            SaveSessionRequest saveSessionRequest = new SaveSessionRequest()
            {
                TransactionId = transactionId,
                SessionId = sessionID,
                ObjectName = objectName,
                Data = data,
                ValidationParams = validateParams,
                ExpirationOptions = new ExpirationOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddMinutes(90),
                    SlidingExpiration = expiry
                }
            };

            try
            {
                using (_logger.BeginTimedOperation("Total time taken for Service call SaveSessionONCloudOnPrem OnCloud", transationId: transactionId))
                {
                    var requestData = JsonConvert.SerializeObject(saveSessionRequest);
                    _logger.LogInformation("SaveSessionONCloudOnPrem {requestData} and {@SessionId}", requestData, sessionID);

                    var path = SaveXMLFormatToOnPrem ? $"SaveSessionONCloudOnPrem?saveXMLFormat=true" : $"SaveSessionONCloudOnPrem";
                    var savedResult = await _resilientClient.PostHttpAsyncWithOptions(path, requestData);

                    if (savedResult.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("SaveSessionONCloudOnPrem {requestUrl} error {@response} , {@requestData} and {@SessionId}", savedResult.url, savedResult.response, requestData, sessionID);
                        if (savedResult.statusCode != HttpStatusCode.BadRequest)
                            return false;
                    }

                    _logger.LogInformation("SaveSessionONCloudOnPrem {requestUrl}, {@requestData}, {@response} and {@SessionId}", savedResult.url, requestData, savedResult.response, sessionID);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception - SaveSessionONCloudOnPrem {Exception}, {ValidateParam}, {StackTrace} and {@SessionId}", ex.Message, validateParams, ex.StackTrace, sessionID);
                throw ex;
            }
            return true;
        }

        public async Task<bool> SaveSessionOnPrem(string data, string sessionID, List<string> validateParams, string objectName, TimeSpan expiry, string transactionId = "Test")
        {
            SaveSessionRequest saveSessionRequest = new SaveSessionRequest()
            {
                TransactionId = transactionId,
                SessionId = sessionID,
                ObjectName = objectName,
                Data = data,
                ValidationParams = validateParams,
                ExpirationOptions = new ExpirationOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddMinutes(90),
                    SlidingExpiration = expiry
                }
            };

            try
            {
                var requestData = JsonConvert.SerializeObject(saveSessionRequest);
                _logger.LogInformation("SaveSessionOnPrem {requestData} and {@SessionId}", requestData, sessionID);

                var savedResult = await _resilientClient.PostHttpAsyncWithOptions("SaveSessionOnCouchbase", requestData);

                if (savedResult.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("SaveSessionOnPrem {requestUrl} error {response} and {@SessionId}", savedResult.url, savedResult.response, sessionID);
                    if (savedResult.statusCode != HttpStatusCode.BadRequest)
                        return false;
                }

                _logger.LogInformation("SaveSessionOnPrem {requestUrl}, {response} and {@SessionId}", savedResult.url, savedResult.response, sessionID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception - SaveSessionOnPrem {Exception}, {ValidateParam}, {StackTrace} and {@SessionId}", ex.Message, validateParams, ex.StackTrace, sessionID);
                throw ex;
            }
            return true;
        }

        public async Task<(bool IsValidPersistData, Response<P> InvalidSessionData, T PersistData)> GetSessionResponse<T, P>(string sessionId, string transactionId, string deviceId, string appId)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(_configuration.GetValue<double>("eResSessionDataExp"));
            string sessionObjectName = typeof(T).FullName;

            SessionRequest sessionRequest = new SessionRequest()
            {
                TransactionId = transactionId,
                ValidationParams = new List<string>() { appId, deviceId },
                SessionId = sessionId,
                ObjectName = sessionObjectName,
                ExpirationOptions = new ExpirationOptions()
                {
                    SlidingExpiration = expiry
                }
            };
            using (_logger.BeginTimedOperation("Total time taken for Service call GetSessionResponse", transationId: transactionId))
            {
                var requestData = JsonConvert.SerializeObject(sessionRequest);
                var pData = await _resilientClient.PostAsync("GetSession", requestData);


                var sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(pData);

                if (!sessionResponse.Succeed)
                {
                    var sessionResults = GetInvalidSessionResponse<P>();
                    _logger.LogError("GetSessionResponse {@Errors} and {sessionObjectName} and {@SessionId}", sessionResults.Errors, sessionObjectName, sessionId);
                    return (false, sessionResults, default);
                }


                T responseData = JsonConvert.DeserializeObject<Persist<T>>(@sessionResponse.Data).Data;

                return (true, null, responseData);
            }
        }

        private Response<T> GetInvalidSessionResponse<T>()
        {
            var response = new Response<T>();
            response.Data = default;
            var statusCode = (int)HttpStatusCode.RequestTimeout;
            response.Errors.Add($"{statusCode}", new List<string> { _configuration.GetSection("invalidSession").GetValue<string>("Message") });
            response.Status = statusCode;
            response.Title = _configuration.GetSection("invalidSession").GetValue<string>("Title");
            return response;
        }
    }
}
