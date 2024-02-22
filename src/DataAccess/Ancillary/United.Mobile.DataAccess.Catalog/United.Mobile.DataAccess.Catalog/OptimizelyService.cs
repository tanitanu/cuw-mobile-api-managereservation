using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Catalog
{
    public class OptimizelyService : IOptimizelyService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<OptimizelyService> _logger;
        private readonly IConfiguration _configuration;

        public OptimizelyService([KeyFilter("OptimizelyServiceClientKey")] IResilientClient resilientClient,
            ICacheLog<OptimizelyService> logger,
            IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetFormatJson(string token, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetFormatJson service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var responseData = string.Empty;
                var path = string.Format(_configuration.GetValue<string>("OptimizelyURL") + "{0}.json", _configuration.GetValue<string>("ExperimentSdkKey"));
                try
                {
                    using (var client = new System.Net.WebClient())
                        responseData = await client.DownloadStringTaskAsync(path);

                }
                catch (WebException wex)
                {
                    _logger.LogError("WebClient service-GetFormatJson {requestUrl} error {response} for {sessionId}", path, responseData, sessionId);
                    throw (wex);
                }

                _logger.LogInformation("WebClient service-GetFormatJson {requestUrl} and {sessionId}", path, sessionId);

                return responseData;
            }
        }
    }
}
