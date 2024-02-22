using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.BagTracking;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.BagTracking
{
    public class BaggageDeliveryService : IBaggageDeliveryService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<BaggageDeliveryService> _logger;

        public BaggageDeliveryService([KeyFilter("BaggageDeliveryClientKey")] IResilientClient resilientClient, ICacheLog<BaggageDeliveryService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<BaggageDeliveryDetails> SubmitBaggageDeliveryClaim(string token, BaggageDeliveryRequest request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "X-IDTOKEN", token }
                     };
            string requestData = JsonConvert.SerializeObject(request);
            IDisposable timer = null;

            using (timer = _logger.BeginTimedOperation("Total time taken for SubmitBaggageDeliveryClaim call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(string.Empty, requestData, headers).ConfigureAwait(false);

                    _logger.LogInformation("CSL service SubmitBaggageDeliveryClaim--Request {CSLRequest} and {requestUrl}", requestData, responseData.url);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-SubmitBaggageDeliveryClaim {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);

                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            throw new Exception(responseData.response);
                    }
                    _logger.LogInformation("CSL service-SubmitBaggageDeliveryClaim {CSLResponse} and {response} for {sessionId}", responseData.response, responseData.url, sessionId);
                    var baggageDeliveryResponse = JsonConvert.DeserializeObject<BaggageDeliveryDetails>(responseData.response);
                    return baggageDeliveryResponse;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-SubmitBaggageDeliveryClaim error {stackTrace} for {sessionId}", ex.StackTrace, sessionId);
                }
                return default;
            }            
        }
    }
}
