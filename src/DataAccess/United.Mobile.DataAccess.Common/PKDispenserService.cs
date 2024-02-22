using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class PKDispenserService : IPKDispenserService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<PKDispenserService> _logger;
        private readonly IConfiguration _configuration;
        public PKDispenserService([KeyFilter("PKDispenserClientKey")] IResilientClient resilientClient
            , ICacheLog<PKDispenserService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<T> GetPkDispenserPublicKey<T>(string token, string sessionId, string path)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("/dispenser/key", path);
            using (_logger.BeginTimedOperation("Total time taken for GetPkDispenserPublicKey business call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetPkDispenserPublicKey {@RequestData}", requestData);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetPkDispenserPublicKey {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetPkDispenserPublicKey {@Response} {@RequestUrl}", responseData.response, responseData.url);
                return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
