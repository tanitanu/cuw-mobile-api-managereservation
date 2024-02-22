using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.SeatEngine
{
    public class SeatMapAvailabilityService : ISeatMapAvailabilityService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<SeatMapAvailabilityService> _logger;

        public SeatMapAvailabilityService([KeyFilter("SeatMapAvailabilityServiceKey")] IResilientClient resilientClient, ICacheLog<SeatMapAvailabilityService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetCSL30SeatMap(string token, string channelId, string channelName, string flightNumber, string departureAirportCode, string arrivalAirportCode, string flightDate, string marketingCarrierCode, string OperatingCarrierCode, string sessionId, string transactionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" },
                          {"ChannelId",channelId },
                          {"ChannelName",channelName }
                     };

            string path = $"{flightNumber},{departureAirportCode},{arrivalAirportCode},{flightDate}?marketingCarrierCode={marketingCarrierCode}&operatingCarrierCode={OperatingCarrierCode}";

            _logger.LogInformation("SeatEngine CSL call- GetCSL30SeatMap {@Request}", path);

            IDisposable timer = null;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetCSL30SeatMap business call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("GetCSL30SeatMap Service {@RequestUrl} error {@Response}", responseData.url, responseData.response);

                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            throw new Exception(responseData.response);

                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("GetCSL30SeatMap Service {@RequestUrl} {@Response}", responseData.url, responseData.response);

                    return responseData.response;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-GetCSL30SeatMap error {@Exception}", JsonConvert.SerializeObject(ex));
                }

                return default;
            }
        }
    }
}
