using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Currency
{
    public class CurrencyConvertService : ICurrencyConvertService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<CurrencyConvertService> _logger;
        private readonly IConfiguration _configuration;
        public CurrencyConvertService([KeyFilter("CurrencyConvertServiceClientKey")] IResilientClient resilientClient
            , ICacheLog<CurrencyConvertService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;

        }
        public async Task<string> GetCurrency(string token, string request, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetCurrency service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(string.Empty, request, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetCurrency {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetCurrency {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }
    }
}
