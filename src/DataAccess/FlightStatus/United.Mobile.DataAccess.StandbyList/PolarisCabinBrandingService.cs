using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.StandbyList
{
    public class PolarisCabinBrandingService : IPolarisCabinBrandingService
    {
        private readonly ICacheLog<PolarisCabinBrandingService> _logger;
        private readonly IResilientClient _resilientClient;

        public PolarisCabinBrandingService(ICacheLog<PolarisCabinBrandingService> logger, [KeyFilter("PolarisCabinBrandingClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetPolarisCabinBranding(string token, string request, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetPolarisCabinBranding service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                //"https://csmc.stage.api.united.com/8.0/referencedata/CabinBranding"
                //string url = _configuration.GetValue<string>("CabinBrandingService - URL");
                var path = "/CabinBranding";

                _logger.LogInformation("CSL service-GetPolarisCabinBranding {path} and {sessionId}", path, sessionId);

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);
                
                _logger.LogInformation("CSL service-GetPolarisCabinBranding {requestUrl} and {sessionId}", responseData.url, sessionId);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetPolarisCabinBranding {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetPolarisCabinBranding {requestUrl} , {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }
    }
}
