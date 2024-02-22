using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightReservation
{
    public class ReservationService: IReservationService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<ReservationService> _logger;
        public ReservationService([KeyFilter("ReservationClientKey")] IResilientClient resilientClient
           , ICacheLog<ReservationService> logger)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> GetCheckInStatus(string token, string mileagePlusNumber, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("/MileagePlusNumber/{0}/100?IscheckinEligible=false&amp;PNRStatus=ACTIVE&amp;Channel=Mobile&amp;GetACITKT=true&amp;GetGMT=true&amp;SplitTitle=true", mileagePlusNumber);
            var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
            _logger.LogInformation("CSL service-GetCheckInStatus {requestUrl} and {sessionId}", responseData.url, sessionId);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetCheckInStatus {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            return (responseData.response);
        }
    }
}
