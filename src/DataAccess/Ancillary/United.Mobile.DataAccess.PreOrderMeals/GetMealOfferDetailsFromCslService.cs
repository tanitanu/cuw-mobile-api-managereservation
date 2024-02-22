using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PreOrderMeals
{
    public class GetMealOfferDetailsFromCslService:IGetMealOfferDetailsFromCslService
    {
        private readonly ICacheLog<GetMealOfferDetailsFromCslService> _logger;
        private readonly IResilientClient _resilientClient;
        public GetMealOfferDetailsFromCslService(ICacheLog<GetMealOfferDetailsFromCslService> logger, [KeyFilter("GetMealOfferDetailsFromCslServiceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetMealOfferDetailsFromCsl(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetMealOfferDetailsFromCsl service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-PostFeedback {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-PostFeedback {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }
    }
}
