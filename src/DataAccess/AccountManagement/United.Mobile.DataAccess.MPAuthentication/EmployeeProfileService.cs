using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public class EmployeeProfileService : IEmployeeProfileService
    {
        private readonly IResilientClient _employeeProfileResilientClient;
        private readonly ICacheLog<EmployeeProfileService> _logger;
        public EmployeeProfileService([KeyFilter("employeeProfileClientKey")] IResilientClient employeeProfileResilientClient, ICacheLog<EmployeeProfileService> logger)
        {
            _employeeProfileResilientClient = employeeProfileResilientClient;
            _logger = logger;
        }

        public async Task<string> GetEmployeeProfile(string token, string path, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetEmployeeProfile service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                var serviceResponse = await _employeeProfileResilientClient.GetHttpAsyncWithOptions(path, headers);

                if (serviceResponse.statusCode != HttpStatusCode.OK)
                {
                    //_logger.LogError("Accountmanagnement-GetStatusLiftBanner {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmployeeProfileEResResponse>(response), sessionId);
                    if (serviceResponse.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(serviceResponse.response);
                }
               
                _logger.LogInformation("Accountmanagnement-GetEmployeeProfile-service {requestUrl} and {sessionId}", serviceResponse.url, sessionId);
                return serviceResponse.response;
            }
        }
    }
}
