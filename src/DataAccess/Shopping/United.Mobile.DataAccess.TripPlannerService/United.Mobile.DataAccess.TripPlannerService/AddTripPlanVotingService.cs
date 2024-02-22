using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.TripPlannerService
{
    public class AddTripPlanVotingService : IAddTripPlanVotingService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<AddTripPlanVotingService> _logger;
        private readonly IConfiguration _configuration;

        public AddTripPlanVotingService([KeyFilter("AddTripPlanVotingServiceClientKey")] IResilientClient resilientClient, ICacheLog<AddTripPlanVotingService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> TripPlanVoting(string token, string sessionId, string action, string request)
        {
            _logger.LogInformation("CSL service-TripPlanVoting parameters Request:{@Request} Action:{@Action}", request, action);

            IDisposable timer = null;
            string returnValue = string.Empty;
            using ((timer = _logger.BeginTimedOperation("Total time taken for TripPlanVoting service call", transationId: sessionId)))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                var response = await _resilientClient.PostHttpAsyncWithOptions(action, request, headers);
                returnValue = response.response;

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-TripPlanVoting {@RequestUrl} error {@Response}", response.url, response.response);
                    throw new MOBUnitedException(_configuration.GetValue<string>("NoAvailabilityError2.0"));
                }

                _logger.LogInformation("CSL service-TripPlanVoting {@RequestUrl} {@Response}", response.url, response.response);
            }

            return returnValue;
        }

        public async Task<string> DeleteTripPlan(string token, string action, string sessionId)
        {
            _logger.LogInformation("CSL service-DeleteTripPlan parameters Action:{@Action}", action);

            IDisposable timer = null;
            string returnValue = string.Empty;
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            using (timer = _logger.BeginTimedOperation("Total time taken for DeleteTripPlan service call", transationId: sessionId))
            {
                var responseData = await _resilientClient.DeleteAsync(action, headers);
                returnValue = responseData.response;
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-DeleteTripPlan {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-DeleteTripPlan {@RequestUrl} {@Response}", responseData.url, responseData.response);
            }

            return returnValue;
        }
    }
}
