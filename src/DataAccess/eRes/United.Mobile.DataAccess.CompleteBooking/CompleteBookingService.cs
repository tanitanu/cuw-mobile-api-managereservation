using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.CompleteBooking;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.CompleteBooking
{
    public class CompleteBookingService : ICompleteBookingService
    {
        private readonly IResilientClient _cbResilientClient;
        private readonly ICacheLog<CompleteBookingService> _logger;
        public CompleteBookingService(ICacheLog<CompleteBookingService> logger,[KeyFilter("cbClientKey")] IResilientClient cbResilientClient)
        {
            _logger = logger;
            _cbResilientClient = cbResilientClient;
        }

        public async Task<string> CompleteBooking(string token, string requestPayload, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var enrollmentData = await _cbResilientClient.PostHttpAsync(string.Empty, requestPayload, headers);
            var response = enrollmentData.Item1;
            var statusCode = enrollmentData.Item2;
            var url = enrollmentData.Item3;

            _logger.LogInformation("eRes-CompleteBooking-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-CompleteBooking-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<BookingBuildPNRResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
            
        }

    }
}
