using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.UnfinishedBooking
{
    public class OmniChannelCartService:IOmniChannelCartService
    {
        private readonly ICacheLog<OmniChannelCartService> _logger;
        private readonly IResilientClient _resilientClient;

        public OmniChannelCartService(ICacheLog<OmniChannelCartService> logger, [KeyFilter("OmniChannelCartServiceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> PurgeUnfinshedBookings(string token, string action, string sessionId)
        {
            _logger.LogInformation("CSL service-PurgeUnfinshedBookings {@Action}", action);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            using (_logger.BeginTimedOperation("Total time taken for PurgeUnfinshedBookings service call", transationId: sessionId))
            {
                var responseData = await _resilientClient.DeleteAsync(action, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-PurgeUnfinshedBookings {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }


                _logger.LogInformation("CSL service-PurgeUnfinshedBookings {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}
