using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Customer
{
    public class CustomerPreferencesService : ICustomerPreferencesService
    {
        private readonly ICacheLog<CustomerPreferencesService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerPreferencesService(ICacheLog<CustomerPreferencesService> logger, [KeyFilter("CustomerPreferencesClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<T> GetCustomerPreferences<T>(string token, string mpNumber, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetCustomerPreferences service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                string requestData = string.Format("/AirPreference/{0}?idType=LoyaltyID", mpNumber);

                using (_logger.BeginTimedOperation("Total time taken for GetCustomerPreferences service call", transationId: sessionId))
                {
                    var cPreferencesData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers);

                    if (cPreferencesData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("AccountManagement-GetCustomerPreferences-service {requestUrl} error {response} for {sessionId}", cPreferencesData.url, cPreferencesData.statusCode, sessionId);
                        if (cPreferencesData.statusCode != HttpStatusCode.BadRequest)
                            throw new Exception(cPreferencesData.response);
                    }

                    _logger.LogInformation("AccountManagement-GetCustomerPreferences-service {requestUrl} and {sessionId}", cPreferencesData.url, sessionId);

                    return JsonConvert.DeserializeObject<T>(cPreferencesData.response);
                }
            }
        }

        public async Task<string> GetSharedItinerary(string token, string action, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string request = string.Format("{0}", action);

            using (_logger.BeginTimedOperation("Total time taken for GetSharedItinerary service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetSharedItinerary {@RequestUrl} {@Request}", request, requestData);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(request, requestData, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetSharedItinerary {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetSharedItinerary {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }

        public async Task<T> GetCustomerPrefernce<T>(string token, string action, string savedUnfinishedBookingActionName, string savedUnfinishedBookingAugumentName, int customerID, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}/{1}/{2}/{3}", action, savedUnfinishedBookingActionName, savedUnfinishedBookingAugumentName, customerID);
            _logger.LogInformation("CSL service-GetCustomerPreference {@RequestData}", requestData);

            using (_logger.BeginTimedOperation("Total time taken for GetCustomerPrefernce service call", transationId: sessionId))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetCustomerPrefernce {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetCustomerPrefernce {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<T> GetUnfinishedCustomerPrefernce<T>(string token, string action, string savedUnfinishedBookingActionName, string savedUnfinishedBookingAugumentName, int customerID, string sessionId, string request)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}/{1}/{2}/{3}", action, savedUnfinishedBookingActionName, savedUnfinishedBookingAugumentName, customerID);
            _logger.LogInformation("CSL service-GetUnfinishedCustomerPrefernce {@Request} {@Url}", request, requestData);

            using (_logger.BeginTimedOperation("Total time taken for GetUnfinishedCustomerPrefernce service call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetUnfinishedCustomerPrefernce {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetUnfinishedCustomerPrefernce {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<T> GetSharedTrip<T>(string token, string sharedTripId, string sessionId )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("/SharedItinerary/AccessCode/{0}", sharedTripId);
            
            using (_logger.BeginTimedOperation("Total time taken for GetSharedTrip service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetSharedTrip {@RequestData}", requestData);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetSharedTrip {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetSharedTrip {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<string> PurgeAnUnfinishedBooking(string token, string action, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            using (_logger.BeginTimedOperation("Total time taken for PurgeAnUnfinishedBooking service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-PurgeAnUnfinishedBooking {@Action}", action);
                var responseData = await _resilientClient.DeleteAsync(action, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-PurgeAnUnfinishedBooking {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-PurgeAnUnfinishedBooking {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}

