using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassRiders;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PassRiders
{
    public class PassRidersService : IPassRidersService
    {
        private readonly IResilientClient _passRidersResilientClient;
        private readonly ICacheLog<PassRidersService> _logger;
        public PassRidersService([KeyFilter("passRidersClientKey")] IResilientClient passRidersResilientClient, ICacheLog<PassRidersService> logger)
        {
            _passRidersResilientClient = passRidersResilientClient;
            _logger = logger;
        }

        public async Task<string> GetPassRiders(string token, string requestData, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var passRidersData = await _passRidersResilientClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = passRidersData.Item1;
            var statusCode = passRidersData.Item2;
            var url = passRidersData.Item3;

            _logger.LogInformation("eRes-GetPassRiders-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetPassRiders-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmpPassRidersResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }
}   