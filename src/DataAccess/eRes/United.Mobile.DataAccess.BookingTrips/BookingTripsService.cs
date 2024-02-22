using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Booking;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.BookingTrips
{
    public class BookingTripsService : IBookingTripsService
    {
        private readonly ICacheLog<BookingTripsService> _logger;
        private readonly IResilientClient _bookingTripResilientClient;
        public BookingTripsService(ICacheLog<BookingTripsService> logger, [KeyFilter("bookingTripsClientKey")]IResilientClient bookingTripResilientClient)
        {
            _logger = logger;
            _bookingTripResilientClient = bookingTripResilientClient;
        }

        public async Task<string> GetBookingTrips(string token, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var enrollmentData = await _bookingTripResilientClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = enrollmentData.Item1;
            var statusCode = enrollmentData.Item2;
            var url = enrollmentData.Item3;

            _logger.LogInformation("eRes-GetBookingTrips-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetBookingTrips-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmpBookingTripsResponse>(response), sessionId);

                //MB-6204 Due to this issue, handling this exception so commenting this code
                //if (statusCode != HttpStatusCode.BadRequest)
                //  throw new Exception(response);

            }
            return response;
           
        }       
    }
}
