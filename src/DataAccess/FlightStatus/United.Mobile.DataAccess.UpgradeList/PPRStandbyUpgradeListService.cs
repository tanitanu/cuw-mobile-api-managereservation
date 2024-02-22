using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.UpgradeList
{
    public class PPRStandbyUpgradeListService : IPPRStandbyUpgradeListService
    {
        private readonly ICacheLog<PPRStandbyUpgradeListService> _logger;
        private readonly IResilientClient _resilientClient;

        public PPRStandbyUpgradeListService([KeyFilter("PPRStandbyUpgradeListClientKey")] IResilientClient resilientClient
            , ICacheLog<PPRStandbyUpgradeListService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetPPRStandbyUpgradeList(string token, string sessionId,  string flightNumber, DateTime flightDateTime,string departureAirportCode)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };


            using (_logger.BeginTimedOperation("Total time taken for GetPPRStandbyUpgradeList service call", transationId: sessionId))
            {
                string path = string.Format("/{0},{1},{2}", flightNumber, flightDateTime.ToString("yyyy-MM-dd"), departureAirportCode);

                _logger.LogInformation("CSL service-GetPPRStandbyUpgradeList {path} for {sessionId} ", path, sessionId);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetPPRStandbyUpgradeList {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    throw new WebException(responseData.response);
                }

                _logger.LogInformation("CSL service-GetPPRStandbyUpgradeList {requestUrl} and {sessionId}", responseData.url, sessionId);

                return responseData.response;
            }
        }
    }
}
