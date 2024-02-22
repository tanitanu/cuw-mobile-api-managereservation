using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public class EResEmployeeProfileService : IEResEmployeeProfileService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<EResEmployeeProfileService> _logger;
        public EResEmployeeProfileService([KeyFilter("eResEmployeeProfileClientKey")] IResilientClient resilientClient, ICacheLog<EResEmployeeProfileService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetEResEmployeeProfile(string token, string path, string requestPayload, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetEResEmployeeProfile service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                var serviceResponse = await _resilientClient.PostHttpAsyncWithOptions(path, requestPayload, headers);

                if (serviceResponse.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Accountmanagnement-EResEmployeeProfile-service {requestUrl} error {@response} for {sessionId}", serviceResponse.url, JsonConvert.DeserializeObject<EResEmployeeProfileService>(serviceResponse.response), sessionId);
                    if (serviceResponse.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(serviceResponse.response);
                }
                
                _logger.LogInformation("Accountmanagnement-EResEmployeeProfile-service {requestUrl} and {sessionId}", serviceResponse.url, sessionId);
                return serviceResponse.response;
            }
        }

        public async Task<T> GetEResEmpProfile<T>(string token, string path, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetEResEmpProfile service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                _logger.LogInformation("GetEResEmpProfile-service {@RequestUrl}", path);
                var response = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetEResEmpProfile-service {@RequestUrl} error {@Response}", response.url, response.response);
                    if (response.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(response.response);
                }

                _logger.LogInformation("GetEResEmpProfile-service {@RequestUrl} and {@Response}", response.url, response.response);

                return JsonConvert.DeserializeObject<T>(response.response);
            }
        }
    }
}
