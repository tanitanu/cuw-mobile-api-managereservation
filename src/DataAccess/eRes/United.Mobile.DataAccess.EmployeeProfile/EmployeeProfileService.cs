using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.EmployeeProfile;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.EmployeeProfile
{
    public class EmployeeProfileService : IEmployeeProfileService
    {
        private readonly IResilientClient _employeeProfileResilientClient;
        private readonly ICacheLog<EmployeeProfileService> _logger;
        public EmployeeProfileService([KeyFilter("empProfileClientKey")] IResilientClient employeeProfileResilientClient, ICacheLog<EmployeeProfileService> logger)
        {
            _employeeProfileResilientClient = employeeProfileResilientClient;
            _logger = logger;
        }

        public async Task<string> GetEmployeeProfile(string token, string requestPayload, string sessionId)        
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var serviceResponse = await _employeeProfileResilientClient.PostHttpAsync(string.Empty, requestPayload, headers);

            var response = serviceResponse.Item1;
            var statusCode = serviceResponse.Item2;
            var url = serviceResponse.Item3;

            _logger.LogInformation("eRes-EmployeeProfile-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-EmployeeProfile-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmployeeProfileEResResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;

        }
    }
}
