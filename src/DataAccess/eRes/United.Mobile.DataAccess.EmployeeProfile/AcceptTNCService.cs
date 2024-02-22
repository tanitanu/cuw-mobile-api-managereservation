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
    public class AcceptTNCService : IAcceptTNCService
    {        
        private readonly IResilientClient _acceptTNCResilientClient;
        private readonly ICacheLog<AcceptTNCService> _logger;  
        public AcceptTNCService([KeyFilter("acceptTNCClientKey")] IResilientClient acceptTNCResilientClient, ICacheLog<AcceptTNCService> logger)
        {
            _acceptTNCResilientClient = acceptTNCResilientClient;
            _logger = logger;
         }

            public async Task<string> AcceptTNC(string token, string encryptedEmployeeId, string sessionId)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                var serviceResponse = await _acceptTNCResilientClient.PostHttpAsync(string.Empty, encryptedEmployeeId, headers);

                var response = serviceResponse.Item1;
                var statusCode = serviceResponse.Item2;
                var url = serviceResponse.Item3;

            _logger.LogInformation("eRes-AcceptTNC-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("eRes-AcceptTNC-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<AcceptTNCEresResponse>(response), sessionId);
                    if (statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(response);
                }
                return response;
           
            }
    }
}
