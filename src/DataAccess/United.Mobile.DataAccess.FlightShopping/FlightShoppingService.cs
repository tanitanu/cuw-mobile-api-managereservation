using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.FlightShopping
{
    public class FlightShoppingService : IFlightShoppingService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<FlightShoppingService> _logger;
        private readonly IConfiguration _configuration;
        public FlightShoppingService([KeyFilter("FlightShoppingClientKey")] IResilientClient resilientClient
            , ICacheLog<FlightShoppingService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<string> GetShopPinDown(string token, string action, string request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            
            string requestData = string.Format("/{0}", action);

            _logger.LogInformation("CSL service-GetShopPinDown {@Request} {@RequestUrl}", request, requestData);
            using (_logger.BeginTimedOperation("Total time taken for CSL service-GetShopPinDown call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetShopPinDown {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL service-GetShopPinDown {@RequestUrl} {@Response}", responseData.url, responseData.response);

                return responseData.response;
            }
        }
        public async Task<(T response, long callDuration)> UpdateAmenitiesIndicators<T>(string token, string sessionId, string jsonRequest)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for UpdateAmenitiesIndicators service call", transationId: sessionId))
            {
                string actionName = @"\UpdateAmenitiesIndicators";

                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                _logger.LogInformation("CSL service-UpdateAmenitiesIndicators {@Request} {@RequestUrl}", jsonRequest, actionName);
                var response = await _resilientClient.PostHttpAsyncWithOptions(actionName, jsonRequest, headers);
                returnValue = response.response;
                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-UpdateAmenitiesIndicators {@RequestUrl} error {@Response}}", response.url, response.response);
                    if (response.statusCode != HttpStatusCode.BadRequest)
                        throw new MOBUnitedException("Failed to retrieve booking details.");
                }
                _logger.LogInformation("CSL service-UpdateAmenitiesIndicators {@RequestUrl} {@Response}", response.url, response.response);
            }
            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());

        }

        public async Task<(T response, long callDuration)> GetLmxQuote<T>(string token, string sessionId, string cartId, string hashList)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;

            using ((timer = _logger.BeginTimedOperation("Total time taken for GetLmxQuote service call", transationId: sessionId)))
            {
                string actionName = @"\GetLmxQuote";
                string jsonRequest = "{\"CartId\":\"" + cartId + "\"}";
                if (!string.IsNullOrEmpty(hashList))
                {
                    jsonRequest = "{\"CartId\":\"" + cartId + "\", \"hashList\":[" + hashList + "]}";
                }

                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                _logger.LogInformation("CSL service-GetLmxQuote {@Request} {@actionName}", jsonRequest, actionName);
                var response = await _resilientClient.PostHttpAsyncWithOptions(actionName, jsonRequest, headers);
                returnValue = response.response;

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetLmxQuote {@RequestUrl} error {@Response}", response.url, response.response);
                    if (response.statusCode != HttpStatusCode.BadRequest)
                        throw new MOBUnitedException("Failed to retrieve booking details.");
                }

                _logger.LogInformation("CSL service-GetLmxQuote {@RequestUrl} {@Response}", response.url, response.response);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }

    }
}
