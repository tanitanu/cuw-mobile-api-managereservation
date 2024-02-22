using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ManageReservation
{
    public class SaveTriptoMyAccountService : ISaveTriptoMyAccountService
    {
        private readonly ICacheLog<PNRRetrievalService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;

        public SaveTriptoMyAccountService([KeyFilter("SaveTriptoMyAccountClientKey")] IResilientClient resilientClient, ICacheLog<PNRRetrievalService> logger
            , IConfiguration configuration)
        {
            _logger = logger;
            _resilientClient = resilientClient;
            _configuration = configuration;
        }

        public async Task<string> SaveTriptoMyAccount(string token, string action, string request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            using (_logger.BeginTimedOperation("Total time taken for CSL service-SavetriptoMyAccount business call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-SavetriptoMyAccount {@Request} {@RequestUrl}", request, action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(action, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-SavetriptoMyAccount {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL service-SavetriptoMyAccount {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}
