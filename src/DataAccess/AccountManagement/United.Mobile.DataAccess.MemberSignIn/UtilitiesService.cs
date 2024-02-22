using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.MemberSignIn
{
    public class UtilitiesService : IUtilitiesService
    {
        private readonly ICacheLog<UtilitiesService> _logger;
        private readonly IResilientClient _resilientClient;

        public UtilitiesService(ICacheLog<UtilitiesService> logger, [KeyFilter("UtilitiesServiceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> ValidateMPNames(string token, string requestData, string sessionId, string path = "")
        {
            using (_logger.BeginTimedOperation("Total time taken for ValidateMPNames service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var vMPNamesData = await _resilientClient.PostHttpAsyncWithOptions(path, requestData, headers);

                if (vMPNamesData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-ValidateMPNames-service {requestUrl} error {response} for {sessionId}", vMPNamesData.url, vMPNamesData.statusCode, sessionId);
                    if (vMPNamesData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(vMPNamesData.response);
                }

                _logger.LogInformation("AccountManagement-ValidateMPNames-service {requestUrl} and {sessionId}", vMPNamesData.url, sessionId);

                return vMPNamesData.response;
            }
        }
        public async Task<T> ValidateMileagePlusNames<T>(string token, string requestData, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for ValidateMileagePlusNames service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                string url = string.Format("/profilevalidation/api/{0}", path);
                var profileValidateData = await _resilientClient.PostHttpAsyncWithOptions(url, requestData, headers);

                if (profileValidateData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-ValidateMileagePlusNames-service {requestUrl} error {response} for {sessionId}", profileValidateData.url, profileValidateData.statusCode, sessionId);
                    if (profileValidateData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(profileValidateData.response);
                }

                _logger.LogInformation("AccountManagement-ValidateMileagePlusNames-service {requestUrl} and {sessionId}", profileValidateData.url, sessionId);

                return JsonConvert.DeserializeObject<T>(profileValidateData.response);
            }
        }

        public async Task<T> ValidatePhoneWithCountryCode<T>(string token, string path, string requestData, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for ValidatePhoneWithCountryCode service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var pnrData = await _resilientClient.PostHttpAsyncWithOptions(path, requestData, headers);

                if (pnrData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Account Management-GetAndValidateStateCode-service {requestUrl} error {response} for {sessionId}", pnrData.url, pnrData.statusCode, sessionId);
                    if (pnrData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(pnrData.response);
                }

                _logger.LogInformation("Account Management-GetAndValidateStateCode-service {requestUrl} and {sessionId}", pnrData.url, sessionId);

                return JsonConvert.DeserializeObject<T>(pnrData.response);
            }
        }
    }
}
