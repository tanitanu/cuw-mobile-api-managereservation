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
    public class SpecialNeedService : ISpecialNeedService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<PassRidersService> _logger;

        public SpecialNeedService([KeyFilter("bookingClientKey")] IResilientClient resilientClient, ICacheLog<PassRidersService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetSpecialNeeds(string token, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetSpecialService eres service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var passRidersData = await _resilientClient.GetHttpAsync(@"/GetSpecialServices", headers);
                var response = passRidersData.Item1;
                var statusCode = passRidersData.Item2;
                var url = passRidersData.Item3;

                if (statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("eRes-GetSpecialService-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmpPassRidersResponse>(response), sessionId);
                    if (statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(response);
                }

                _logger.LogInformation("eRes-GetSpecialService-service {requestUrl} and {sessionId}", url, sessionId);
                return response;
            }
        }
    }
}   