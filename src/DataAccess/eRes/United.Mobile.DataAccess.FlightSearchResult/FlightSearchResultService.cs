using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.FlightSearchResult;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightSearchResult
{
    public class FlightSearchResultService : IFlightSearchResultService
    {
        private readonly IResilientClient _fsrResilientClient;
        private readonly ICacheLog<FlightSearchResultService> _logger;
        public FlightSearchResultService(ICacheLog<FlightSearchResultService> logger,[KeyFilter("fsClientKey")]IResilientClient fsrResilientClient)
        {
             _logger=logger;
            _fsrResilientClient = fsrResilientClient;
        }
        public async Task<string> GetFlightSearchResult(string token, string endPoint, string requestData, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                                                         {
                                                              { "Authorization", token }
                                                         };

            var fsrData = await _fsrResilientClient.PostHttpAsync(endPoint, requestData, headers);
            var response = fsrData.Item1;
            var statusCode = fsrData.Item2;
            var url = fsrData.Item3;

            _logger.LogInformation("eRes-GetFlightSearchResult-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetFlightSearchResult-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EResFsrResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }           
            return response;
            
        }
    }
}
