using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.MerchandizeService
{
    public class VendorMerchandizingService:IVendorMerchandizingService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<PurchaseMerchandizingService> _logger;
        private readonly IConfiguration _configuration;
        public VendorMerchandizingService([KeyFilter("MerchandizingNewClientKey")] IResilientClient resilientClient
            , ICacheLog<PurchaseMerchandizingService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<T> GetVendorOfferInfo<T>(string token, string request, string sessionId)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetVendorOfferInfo CSL call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                _logger.LogInformation("CSL service-GetVendorOfferInfo {requestData}", request);

                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions("/getvendoroffers", request, headers, "application/json", true);
                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-GetVendorOfferInfo {requestUrl} error {response}", responseData.url, responseData.response);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }
                    returnValue = responseData.response;
                }
                catch (Exception)
                {
                    _logger.LogInformation("CircuitBreakerStatus {CircuitState} {machinename}", _resilientClient.GetCircuitBreakerStatus(), System.Environment.MachineName);
                    throw;
                }

                _logger.LogInformation("CSL service-GetVendorOfferInfo {response} ", returnValue);
            }

            return JsonConvert.DeserializeObject<T>(returnValue);
        }
    }
}
