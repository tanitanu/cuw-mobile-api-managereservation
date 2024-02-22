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
    public class SessionService : ISessionService
    {
        private readonly ICacheLog<SessionService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IResilientClient _resilientClient;
        private readonly int _expiryTime;
        private readonly int _absoluteExpirationInMin;

        public SessionService([KeyFilter("sessionConfigKey")] IResilientClient resilientClient, ICacheLog<SessionService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
            int expiryTime = _configuration.GetValue<int>("SlidingExpiration");
            _expiryTime = expiryTime == 0 ? 10 : expiryTime;
            int absoluteExpirationInMin = _configuration.GetValue<int>("absoluteExpirationInMin");
            _absoluteExpirationInMin = absoluteExpirationInMin == 0 ? 10 : absoluteExpirationInMin;
        }

        public async Task<string> GetSessionId<T>(string objectName, string transactionId, string deviceId, string appId, TimeSpan expiry)
        {
            SessionIdRequest sessionIdRequest = new SessionIdRequest()
            {
                TransactionId = transactionId,
                ValidationParams = new List<string>() { appId, deviceId },
                ExpirationOptions = new ExpirationOptions()
                {
                    SlidingExpiration = expiry
                }
            };
            using (_logger.BeginTimedOperation("Total time taken for Service call GetSessionId", transationId: transactionId))
            {
                string requestData = JsonConvert.SerializeObject(sessionIdRequest);
                return await _resilientClient.PostAsync("GetSessionId", requestData);
            }
        }
        public async Task<string> GetSession<T>(string sessionId, string transactionId, string objectName, string deviceId, string appId, TimeSpan expiry)
        {
            SessionRequest sessionRequest = new SessionRequest()
            {
                TransactionId = transactionId,
                ValidationParams = new List<string>() { appId, deviceId },
                SessionId = sessionId,
                ObjectName = objectName,
                ExpirationOptions = new ExpirationOptions()
                {
                    SlidingExpiration = expiry
                }
            };
            using (_logger.BeginTimedOperation("Total time taken for Service call GetSession", transationId: transactionId))
            {
                var requestData = JsonConvert.SerializeObject(sessionRequest);
                return await _resilientClient.PostAsync("GetSession", requestData);
            }

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
            using (_logger.BeginTimedOperation("Total time taken for Service call GetSession", transationId: transactionId))
            {
                var requestData = JsonConvert.SerializeObject(sessionRequest);
                var pData = await _resilientClient.PostAsync("GetSession", requestData);


                var sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(pData);

                if (!sessionResponse.Succeed)
                {
                    var sessionResults = GetInvalidSessionResponse<P>();
                    _logger.LogError("GetSessionResponse {@Errors} and {sessionObjectName} and {sessionId}", sessionResults.Errors, sessionObjectName, sessionId);
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

        public async Task<bool> SaveSession<T>(string sessionId, string transactionId, string objectName, T data, string deviceId, string appId, TimeSpan expiry)
        {
            SaveSessionRequest saveSessionRequest = new SaveSessionRequest()
            {
                TransactionId = transactionId,
                SessionId = sessionId,
                ObjectName = objectName,
                Data = data,
                ValidationParams = new List<string>() { appId, deviceId },
                ExpirationOptions = new ExpirationOptions()
                {
                    SlidingExpiration = expiry
                }
            };
            using (_logger.BeginTimedOperation("Total time taken for Service call SaveSession", transationId: transactionId))
            {
                var requestData = JsonConvert.SerializeObject(saveSessionRequest);
                var savedResult = await _resilientClient.PostAsync("SaveSession", requestData);
                if (!string.IsNullOrEmpty(savedResult))
                    return true;
            }
            return false;
        }
    }
}
