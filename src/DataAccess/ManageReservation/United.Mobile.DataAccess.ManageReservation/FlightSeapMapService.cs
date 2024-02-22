using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ManageReservation
{
    public class FlightSeapMapService : IFlightSeapMapService
    {
        private readonly ICacheLog<FlightSeapMapService> _logger;
        private readonly IResilientClient _resilientClient;

        public FlightSeapMapService(ICacheLog<FlightSeapMapService> logger, [KeyFilter("FlightSeapMapClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<T> ViewChangeSeats<T>(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for ViewChangeSeats service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var vSecurityQuestions = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (vSecurityQuestions.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-ViewChangeSeats-service {requestUrl} error {response} for {sessionId}", vSecurityQuestions.url, vSecurityQuestions.statusCode, sessionId);
                    if (vSecurityQuestions.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(vSecurityQuestions.response);
                }

                _logger.LogInformation("AccountManagement-ViewChangeSeats-service {requestUrl} and {sessionId}", vSecurityQuestions.url, sessionId);

                return JsonConvert.DeserializeObject<T>(vSecurityQuestions.response);
            }
        }
    }
}
