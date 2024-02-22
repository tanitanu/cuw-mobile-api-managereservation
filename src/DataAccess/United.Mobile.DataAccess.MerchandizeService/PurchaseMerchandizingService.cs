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
    public class PurchaseMerchandizingService : IPurchaseMerchandizingService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<PurchaseMerchandizingService> _logger;
        private readonly IConfiguration _configuration;
        public PurchaseMerchandizingService([KeyFilter("MerchandizingClientKey")] IResilientClient resilientClient
            , ICacheLog<PurchaseMerchandizingService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;

        }
        public async Task<(T response, long callDuration)> GetInflightPurchaseInfo<T>(string token, string action, string request, string sessionId)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetInflightPurchaseInfo CSL call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/{0}", action);
                _logger.LogInformation("CSL service-GetInflightPurchaseInfo {@Request} {@RequestUrl}", request, path);

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetInflightPurchaseInfo {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                returnValue = responseData.response;
                _logger.LogInformation("CSL service-GetInflightPurchaseInfo {@RequestUrl} {@Response}", responseData.url, responseData.response);
            }

            return (JsonConvert.DeserializeObject<T>(returnValue), (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0);
        }

        public async Task<T> GetMerchOfferInfo<T>(string token, string action, string request, string sessionId)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetMerchOfferInfo CSL call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("/{0}", action);
                _logger.LogInformation("CSL service-GetMerchOfferInfo Request:{@Request} Path:{@Path}", request, requestData);

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetMerchOfferInfo {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                returnValue = responseData.response;
                _logger.LogInformation("CSL service-GetMerchOfferInfo {@RequestUrl}, {@Response}", responseData.url, responseData.response);
            }

            return JsonConvert.DeserializeObject<T>(returnValue);
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

                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/getvendoroffers", request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetVendorOfferInfo {requestUrl} error {response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                returnValue = responseData.response;
                _logger.LogInformation("CSL service-GetVendorOfferInfo {response} ", returnValue);
            }

            return JsonConvert.DeserializeObject<T>(returnValue);
        }

        public async Task<(T response, long callDuration)> GetInflightPurchaseEligibility<T>(string token, string request, string sessionId)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            using (timer = _logger.BeginTimedOperation("Total time taken forGetInflightPurchaseEligibility CSL call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                string url = "/GetProductEligibility";

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(url, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetInflightPurchaseEligibility {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                returnValue = responseData.response;
                _logger.LogInformation("CSL service-GetInflightPurchaseEligibility {requestUrl} and {sessionId}", responseData.url, sessionId);
            }

            return (JsonConvert.DeserializeObject<T>(returnValue), (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0);
        }

    }
}
