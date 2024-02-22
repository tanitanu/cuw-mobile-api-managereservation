using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.FlightSearchResult;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightSearchResult
{
    public class FlightPBTSevice : IFlightPBTSevice
    {
        private readonly IResilientClient _pbtResilientClient;
        private readonly ICacheLog<FlightPBTSevice> _logger;
        public FlightPBTSevice([KeyFilter("pbtClientKey")] IResilientClient pbtResilientClient, ICacheLog<FlightPBTSevice> logger)
        {
            _pbtResilientClient = pbtResilientClient;
            _logger = logger;
        }
        public async Task<string> GetFlightPBT(string token, EResFlightPBTRequest request)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            string requestData = string.Format("?orgin={0}&destination={1}&flightNumber={2}&flightDate={3}&carrierCode={4}", request.Origin, request.Destination, request.FlightNumber, request.FlightDate, request.CarrierCode);
            var responseData = await _pbtResilientClient.GetHttpAsync(requestData, headers).ConfigureAwait(false);        
            var response = responseData.Item1;
            var statusCode = responseData.Item2;
            var url = responseData.Item3;

            _logger.LogInformation("eRes--GetFlightPBT-service {requestUrl} and {sessionId}", url, request.SessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetFlightPBT-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EresFlightPBTResponse>(response), request.SessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;

        }
    }
}
