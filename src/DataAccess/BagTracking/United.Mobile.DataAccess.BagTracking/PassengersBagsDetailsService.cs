using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.BagTracking
{
    public class PassengersBagsDetailsService : IPassengersBagsDetailsService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<PassengersBagsDetailsService> _logger;

        public PassengersBagsDetailsService([KeyFilter("PassengersBagsDetailsClientKey")] IResilientClient resilientClient, ICacheLog<PassengersBagsDetailsService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetPassengersBagsDetails(string token, string PNRInfo, string BagTrackNumber, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                           {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("pnrInfo={0}&bagTagNbr={1}", PNRInfo, BagTrackNumber);
            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetPassengersBagsDetails call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(true);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-GetPassengersBagsDetails {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            throw new Exception(responseData.response);
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-GetPassengersBagsDetails {requestUrl} and {sessionId}", responseData.url, sessionId);

                    return responseData.response;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-GetPassengersBagsDetails error {stackTrace} for {sessionId}", ex.StackTrace, sessionId);
                }

                return default;
            }
        }
    }
}
