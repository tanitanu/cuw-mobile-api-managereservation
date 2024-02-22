using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ReShop
{
    public class ReservationService : IReservationService
    {
        private readonly ICacheLog<ReservationService> _logger;
        private readonly IResilientClient _resilientClient;

        public ReservationService([KeyFilter("ReShopReservationClientKey")] IResilientClient resilientClient, ICacheLog<ReservationService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> ConfirmScheduleChange<T>(string token, string recordLocator, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            var path = $"/{recordLocator}" + $"/FlightSegments/UpdateStatus?IsScheduleChange=TRUE";

            _logger.LogInformation("CSL service-ConfirmScheduleChange-Reservation-service {path} and {sessionId}", path, sessionId);

            var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, string.Empty, headers);

            _logger.LogInformation("CSL service-ConfirmScheduleChange-Reservation-service {requestUrl} and {sessionId}", responseData.url, sessionId);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-ConfirmScheduleChange-Reservation-service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(responseData.response);
            }

            _logger.LogInformation("CSL service-GetPolarisCabinBranding {requestUrl} , {response} and {sessionId}", responseData.url, responseData.response, sessionId);
            return responseData.response;
        }
    }
}
