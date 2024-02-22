using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Common;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightStatus
{
    public class FlightRequestSqlSpOnPremService : IFlightRequestSqlSpOnPremService
    {
        private readonly ICacheLog<FlightRequestSqlSpOnPremService> _logger;
        private readonly IResilientClient _resilientClient;

        public FlightRequestSqlSpOnPremService([KeyFilter("FlightRequestSqlSpOnPremClientKey")] IResilientClient resilientClient
            , ICacheLog<FlightRequestSqlSpOnPremService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<List<string>> GetCabinCountByShipNumber(List<string> ship, string sessionId)
        {
            if(ship == null)
            {
                _logger.LogError("GetCabinCountByShipNumber listOfShip is null {sessionId}", sessionId);
                return default;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetCabinCountByShipNumber call", transationId: sessionId))
            {
                var stringShips = string.Empty;
                foreach (var item in ship)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        stringShips += string.IsNullOrEmpty(stringShips) ? item : "," + item;
                    }
                }

                if (string.IsNullOrEmpty(stringShips))
                {
                    return default;
                }

                var requestObj = string.Format("/VerifyWiFiAvailable?shipNumbers={0}&&sessionId={1}", stringShips, sessionId);
                requestObj = requestObj.Replace("\r", "").Replace("\n", "").Trim();
                if (!string.IsNullOrEmpty(sessionId))
                    sessionId = sessionId.Replace("\r", "").Replace("\n", "").Trim();
                else
                    sessionId = string.Empty;
                _logger.LogInformation("GetCabinCountByShipNumber {requestPayLoad} and {sessionId} ", requestObj, sessionId);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestObj, headers).ConfigureAwait(true);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetCabinCountByShipNumber {requestUrl} error {response} for {Shipment} and {sessionId} ", responseData.url, responseData.response, ship, sessionId);
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("GetCabinCountByShipNumber {requestUrl} info {response} for {Shipment} and {sessionId}", responseData.url, responseData.response, ship, sessionId);
                return JsonConvert.DeserializeObject<List<string>>(responseData.response);
            }
        }
    }
}
