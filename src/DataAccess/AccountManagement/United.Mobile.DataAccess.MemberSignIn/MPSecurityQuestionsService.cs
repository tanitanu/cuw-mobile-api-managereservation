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
    public class MPSecurityQuestionsService : IMPSecurityQuestionsService
    {
        private readonly ICacheLog<MPSecurityQuestionsService> _logger;
        private readonly IResilientClient _resilientClient;

        public MPSecurityQuestionsService(ICacheLog<MPSecurityQuestionsService> logger, [KeyFilter("MPSecurityQuestionsClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetMPPINPWDSecurityQuestions(string token, string requestData, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetMPPINPWDSecurityQuestions service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                      {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                string path = "/GetAllSecurityQuestions";
                var mpSecurityQuestions = await _resilientClient.PostHttpAsyncWithOptions(path, requestData);

                if (mpSecurityQuestions.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetMPPINPWDSecurityQuestions-service {requestUrl} error {response} for {sessionId}", mpSecurityQuestions.url, mpSecurityQuestions.statusCode, sessionId);
                    if (mpSecurityQuestions.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(mpSecurityQuestions.response);
                }

                _logger.LogInformation("AccountManagement-GetMPPINPWDSecurityQuestions-service {requestUrl} and {sessionId}", mpSecurityQuestions.url, sessionId);

                return mpSecurityQuestions.response;
            }
        }
        public async Task<T> ValidateSecurityAnswer<T>(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for ValidateSecurityAnswer service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var vSecurityQuestions = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (vSecurityQuestions.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-ValidateSecurityAnswer-service {requestUrl} error {response} for {sessionId}", vSecurityQuestions.url, vSecurityQuestions.statusCode, sessionId);
                    if (vSecurityQuestions.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(vSecurityQuestions.response);
                }

                _logger.LogInformation("AccountManagement-ValidateSecurityAnswer-service {requestUrl} and {sessionId}", vSecurityQuestions.url, sessionId);

                return JsonConvert.DeserializeObject<T>(vSecurityQuestions.response);
            }
        }
        public async Task<string> AddDeviceAuthentication(string token, string requestData, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for AddDeviceAuthentication service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var mpSecurityQuestions = await _resilientClient.PostHttpAsyncWithOptions(path,requestData,headers);

                if (mpSecurityQuestions.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-AddDeviceAuthentication-service {requestUrl} error {response} for {sessionId}", mpSecurityQuestions.url, mpSecurityQuestions.statusCode, sessionId);
                    if (mpSecurityQuestions.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(mpSecurityQuestions.response);
                }

                _logger.LogInformation("AccountManagement-AddDeviceAuthentication-service {requestUrl} and {sessionId}", mpSecurityQuestions.url, sessionId);

                return mpSecurityQuestions.response;
            }
        }
    }
}
