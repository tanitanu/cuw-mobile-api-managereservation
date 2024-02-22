using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightStatus
{
    public class FLIFOTokenService : IFLIFOTokenService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<FLIFOTokenService> _logger;

        public FLIFOTokenService([KeyFilter("FLIFOTokenServiceClientKey")] IResilientClient resilientClient, ICacheLog<FLIFOTokenService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetFLIFOTokenService(string token, string transactionId)
        {
            _logger.LogInformation("CSL service-GetFLIFOTokenService {token} and {transactionId}", token, transactionId);

            using (_logger.BeginTimedOperation("Total time taken for GetFLIFOTokenService service call", transationId: transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(string.Empty, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetFLIFOTokenService {URL} error {response} for {transactionId}", responseData.url, JsonConvert.DeserializeObject<Service.Presentation.FlightResponseModel.FlightInformation>(responseData.response), transactionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("CSL service-GetFLIFOTokenService {URL} , {response} and {transactionId}", responseData.url, responseData.response, transactionId);
                return responseData.response;
            }
        }
    }
}
