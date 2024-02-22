using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.StandbyList
{
    public class StandbyListPPRService : IStandbyListPPRService
    {
        private readonly ICacheLog<StandbyListPPRService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;

        public StandbyListPPRService(ICacheLog<StandbyListPPRService> logger, [KeyFilter("StandbyListPPRClientKey")] IResilientClient resilientClient
            , IConfiguration configuration)
        {
            _logger = logger;
            _resilientClient = resilientClient;
            _configuration = configuration;
        }

        public async Task<string> GetStandbyListPPR(string token, string sessionId, string flightNumber, DateTime flightDate, string departureAirportCode)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            var requestData = string.Format("/{0},{1},{2}", flightNumber, flightDate.ToString("yyyy-MM-dd"), departureAirportCode);

            //string url = "https://upgradeswebapi.ual.com/upgrades-list-extended/api/flights/list-display/678,2022-04-05,EWR";

            _logger.LogInformation("CSL service-GetStandbyListPPR {BaseURL},{requestData},{token} and {sessionId}", _resilientClient.BaseURL, Newtonsoft.Json.JsonConvert.SerializeObject(requestData), token, sessionId);

            //_logger.LogInformation("CSL service-GetStandbyListPPR {url} and {sessionId}", url, sessionId);

            using (_logger.BeginTimedOperation("Total time taken for GetStandbyListPPR business call", transationId: sessionId))
            {
                var (response, statusCode, url) = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers, true).ConfigureAwait(true);
                if (statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetStandbyListPPR {requestUrl} error {response} for {sessionId}", url, response, sessionId);
                    if (statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetStandbyListPPR {requestUrl} , {response} and {sessionId}", url, response, sessionId);
                
                return response ?? default;
            }
        }
    }
}
