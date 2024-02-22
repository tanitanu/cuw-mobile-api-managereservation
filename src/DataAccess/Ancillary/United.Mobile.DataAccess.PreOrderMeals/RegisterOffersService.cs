using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
namespace United.Mobile.DataAccess.PreOrderMeals
{
    public class RegisterOffersService: IRegisterOffersService
    {
        private readonly ICacheLog<RegisterOffersService> _logger;
        private readonly IResilientClient _resilientClient;
        public RegisterOffersService(ICacheLog<RegisterOffersService> logger, [KeyFilter("RegisterOffersServiceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> RegisterOffers(string request, string sessionId, string path)
        {
            _logger.LogInformation("CSL-GetInflightMealRefreshments - RegisterOffers {request} for {sessionId}", request, sessionId);

            using (_logger.BeginTimedOperation("Total time taken for RegisterOffers service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL-GetInflightMealRefreshments - RegisterOffers {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL-GetInflightMealRefreshments - RegisterOffers {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }
    }
}
