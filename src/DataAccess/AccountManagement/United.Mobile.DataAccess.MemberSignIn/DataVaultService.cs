using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.MemberSignIn
{
    public class DataVaultService : IDataVaultService
    {
        private readonly ICacheLog<DataVaultService> _logger;
        private readonly IResilientClient _resilientClient;

        public DataVaultService(ICacheLog<DataVaultService> logger, [KeyFilter("DataVaultTokenClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetPersistentToken(string token, string requestData, string url, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetPersistentToken service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                         {"Accept", "application/json"},
                         { "Authorization", token }
                     };
                _logger.LogInformation("AccountManagement-GetPersistentToken-service {@RequestUrl}", url);
                var gPTokenData = await _resilientClient.GetHttpAsyncWithOptions(url, headers);

                if (gPTokenData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetPersistentToken-service {@RequestUrl} error {@Response}", gPTokenData.url, gPTokenData.response);
                    if (gPTokenData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(gPTokenData.response);
                }

                _logger.LogInformation("AccountManagement-GetPersistentToken-service {@RequestUrl} and {@Response}", gPTokenData.url, gPTokenData.response);

                return gPTokenData.response;
            }
        }
        public async Task<string> PersistentToken(string token, string requestData, string url, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for PersistentToken1 service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                _logger.LogInformation("AccountManagement-GetPersistentToken1-service {@RequestUrl} and {@Request}", url, requestData);
                var pTokenData = await _resilientClient.PostHttpAsyncWithOptions(url, requestData, headers);

                if (pTokenData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetPersistentToken1-service {@RequestUrl} error {@Response}", pTokenData.url, pTokenData.response);
                    if (pTokenData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(pTokenData.response);
                }

                _logger.LogInformation("AccountManagement-GetPersistentToken1-service {@RequestUrl} and {@Response}", pTokenData.url, pTokenData.response);

                return pTokenData.response;
            }
        }
        public async Task<string> GetCCTokenWithDataVault(string token, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
            string path = "/AddPayment";
            using (_logger.BeginTimedOperation("Total time taken for GetCCTokenWithDataVault call", transationId: sessionId))
            {
                _logger.LogInformation("United ClubPasses GetCCTokenWithDataVault Service {@RequestPayload} {@RequestUrl}", requestData, path);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("United ClubPasses GetCCTokenWithDataVault CSL Service {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("United ClubPasses GetCCTokenWithDataVault CSL Service {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }

        public async Task<string> GetRSAWithDataVault(string token, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
            string path = string.Format("/{0}/RSA", requestData);
            using (_logger.BeginTimedOperation("Total time taken for GetRSAWithDataVault call", transationId: sessionId))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetRSAWithDataVault CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("GetRSAWithDataVault CSL Service {requestUrl}  {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<(T response, long callDuration)> GetCSLWithDataVault<T>(string token, string action, string sessionId, string jsonRequest)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetCSLWithDataVault service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/{0}", action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetCSLWithDataVault {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogError("CSL service-GetCSLWithDataVault {requestUrl} for {sessionId}", responseData.url, sessionId);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }

        public async Task<string> GetDecryptedTextFromDataVault(string token, string action, string sessionId, string jsonRequest)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetDecryptedTextFromDataVault service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/{0}", action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetDecryptedTextFromDataVault CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("GetDecryptedTextFromDataVault CSL Service {requestUrl} , {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }
    }
}
