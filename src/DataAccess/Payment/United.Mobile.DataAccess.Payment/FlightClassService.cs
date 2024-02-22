using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Payment
{
    public class FlightClassService : IFlightClassService
    {
        private readonly ICacheLog<FlightClassService> _logger;
        private readonly IResilientClient _resilientClient;

        public FlightClassService(ICacheLog<FlightClassService> logger, [KeyFilter("GooglePayFlightClassClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> InsertFlightClass(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for GooglePay-InsertFlightClass service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var enrollmentData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (enrollmentData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GooglePay-InsertFlightClass-service {requestUrl} error {response} for {sessionId}", enrollmentData.url, enrollmentData.statusCode, sessionId);
                    if (enrollmentData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(enrollmentData.response);
                }

                _logger.LogInformation("GooglePay-InsertFlightClass-service {requestUrl} and {sessionId}", enrollmentData.url, sessionId);

                return enrollmentData.response;
            }
        }

        public async Task<string> UpdateFlightClass(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for GooglePay-InsertFlightClass service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var enrollmentData = await _resilientClient.PutAsync(path, request, headers);

                if (enrollmentData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GooglePay-InsertFlightClass-service {requestUrl} error {response} for {sessionId}", enrollmentData.url, enrollmentData.statusCode, sessionId);
                    if (enrollmentData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(enrollmentData.response);
                }

                _logger.LogInformation("GooglePay-InsertFlightClass-service {requestUrl} and {sessionId}", enrollmentData.url, sessionId);

                return enrollmentData.response;
            }
        }
    }
}
