using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Shopping
{
    public class OptimizelyPersistService : IOptimizelyPersistService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<OptimizelyPersistService> _logger;
        private readonly IConfiguration _configuration;

        public OptimizelyPersistService([KeyFilter("OptimizelyServiceClientKey")] IResilientClient resilientClient,
            ICacheLog<OptimizelyPersistService> logger,
            IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetFormatJson(string sessionId)
        {
            sessionId = string.IsNullOrEmpty(sessionId) ? "Test" : sessionId;
            using (_logger.BeginTimedOperation("Total time taken for GetFormatJson service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };
                var path = string.Format(@"/{0}.json", _configuration.GetValue<string>("ExperimentSdkKey"));
                try
                {
                    //using (var client = new System.Net.WebClient())
                    //    responseData = await client.DownloadStringTaskAsync(path);
                    _logger.LogInformation("CSL service-GetCurrency {path} for {sessionId}", path,  sessionId);
                    var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-GetCurrency {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    _logger.LogInformation("CSL service-GetFormatJson {requestUrl}, {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                    return responseData.response;

                }
                catch (WebException wex)
                {
                    _logger.LogError("WebClient service-GetFormatJson {requestUrl} error {response} for {sessionId}", path, string.Empty, sessionId);
                    throw (wex);
                }
            }
        }
    }
}
