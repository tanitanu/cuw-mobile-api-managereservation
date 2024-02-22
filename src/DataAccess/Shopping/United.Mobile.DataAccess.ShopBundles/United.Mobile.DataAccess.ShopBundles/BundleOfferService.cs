using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.ShopBundles
{
    public class BundleOfferService : IBundleOfferService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<BundleOfferService> _logger;
        private readonly IConfiguration _configuration;

        public BundleOfferService([KeyFilter("BundleOfferServiceClientKey")] IResilientClient resilientClient, ICacheLog<BundleOfferService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<(T response, long callDuration)> DynamicOfferdetail<T>(string token, string sessionId, string action, string shopRequest)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;

            _logger.LogInformation("CSL service-DynamicOfferdetail {@Action}, {@Request}", action, shopRequest);

            using ((timer = _logger.BeginTimedOperation("Total time taken for DynamicOfferdetail CSL service call", transationId: sessionId)))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                var response = await _resilientClient.PostHttpAsyncWithOptions(action, shopRequest, headers);
                returnValue = response.response;

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-DynamicOfferdetail {@RequestUrl} error {@Response}", response.url, response.response);
                    throw new MOBUnitedException(_configuration.GetValue<string>("NoAvailabilityError2.0"));
                }

                _logger.LogInformation("CSL service-DynamicOfferdetail {@RequestUrl}, {@Response}", response.url, response.response);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }

        public async Task<(T response, long callDuration)> UnfinishedBookings<T>(string token, string sessionId, string action, string request)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            _logger.LogInformation("CSL service-UnfinishedBookings {@Action}, {@Request}", action, request);
            using ((timer = _logger.BeginTimedOperation("Total time taken for UnfinishedBookings service call", transationId: sessionId)))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                var response = await _resilientClient.PostHttpAsyncWithOptions(action, request, headers);
                returnValue = response.response;

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-UnfinishedBookings {@RequestUrl} error {@Response}", response.url, response.response);
                    throw new MOBUnitedException(_configuration.GetValue<string>("NoAvailabilityError2.0"));
                }

                _logger.LogInformation("CSL service-UnfinishedBookings {@RequestUrl}, {@Response}", response.url, response.response);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());            
        }
        public async Task<(string response, long callDuration)> GetCCEContent<T>(string token, string sessionId, string action, string request)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            _logger.LogInformation("CSL service-GetCCEContent {@Action}, {@Request}", action, request);
            using ((timer = _logger.BeginTimedOperation("Total time taken for GetCCEContent service call", transationId: sessionId)))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                var response = await _resilientClient.PostHttpAsyncWithOptions(action, request, headers);
                returnValue = response.response;

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetCCEContent {@RequestUrl} error {@Response}", response.url, response.response);
                    throw new MOBUnitedException(_configuration.GetValue<string>("NoAvailabilityError2.0"));
                }

                _logger.LogInformation("CSL service-GetCCEContent {@RequestUrl} {@Response}", response.url, returnValue);
            }

            return ((returnValue == null) ? default : returnValue, ((TimedOperation)timer).GetElapseTime());
        }


    }
}
