using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.AlertCheckFSR;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.BookingAlert
{
    public class BookingAlertService : IBookingAlertService
    {
        private readonly IResilientClient _bookingAlertResilientClient;
        private readonly ICacheLog<BookingAlertService> _logger;
        public BookingAlertService(ICacheLog<BookingAlertService> logger, [KeyFilter("bookingAlertClientKey")]IResilientClient bookingAlertResilientClient)
        {
            _logger = logger;
            _bookingAlertResilientClient = bookingAlertResilientClient;
        }

        public async Task<string> GetBookingAlert(string token, string requestData, string sessionId )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var enrollmentData = await _bookingAlertResilientClient.PostHttpAsyncWithOptions(string.Empty, requestData, headers);


            if (enrollmentData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetBookingAlert-service {requestUrl} error {@response} for {sessionId}", enrollmentData.url, JsonConvert.DeserializeObject<AlertResponse>(enrollmentData.response), sessionId);
                if (enrollmentData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(enrollmentData.response);
            }

            _logger.LogInformation("eRes-GetBookingAlert-service {requestUrl} and {sessionId}", enrollmentData.url, sessionId);
            return enrollmentData.response;
        }       
    }
}
