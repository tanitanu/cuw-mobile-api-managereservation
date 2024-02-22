using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightShopping
{
    public class LMXInfo : ILMXInfo
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<LMXInfo> _logger;
        private readonly IConfiguration _configuration;
        public LMXInfo([KeyFilter("FlightShoppingClientKey")] IResilientClient resilientClient
            , ICacheLog<LMXInfo> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<T> GetLmxFlight<T>(string token, string request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", "GetLmxQuote");

            using (_logger.BeginTimedOperation("Total time taken for GetLmxFlight business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetLmxFlight {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetLmxFlight {requestUrl} and {sessionId}", responseData.url, sessionId);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

    }
}
