using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.SaveTrip;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.SaveTrip
{
    public class SaveTripService : ISaveTripService
    {
        private readonly IResilientClient _stResilientClient;
        private readonly ICacheLog<SaveTripService> _logger;
        public SaveTripService([KeyFilter("stClientKey")] IResilientClient stResilientClient, ICacheLog<SaveTripService> logger)
        {
            _stResilientClient = stResilientClient;
            _logger = logger;

        }

        public async Task<string> SaveTrip(string token, string requestPayload, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                                                         {
                                                              { "Authorization", token }
                                                         };

            var enrollmentData = await _stResilientClient.PostHttpAsync(string.Empty, requestPayload, headers);
            var response = enrollmentData.Item1;
            var statusCode = enrollmentData.Item2;
            var url = enrollmentData.Item3;

            _logger.LogInformation("eRes-SaveTrip-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-SaveTrip-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmpSaveTripResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }
}
