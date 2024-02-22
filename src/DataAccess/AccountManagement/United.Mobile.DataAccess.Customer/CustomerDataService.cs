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

namespace United.Mobile.DataAccess.Customer
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly ICacheLog<CustomerDataService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerDataService(
              [KeyFilter("CustomerDataClientKey")] IResilientClient resilientClient
            , ICacheLog<CustomerDataService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        
        public async Task<(T response, long callDuration)> GetCustomerData<T>(string token, string sessionId, string jsonRequest)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetCustomerData service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/GetProfile");
                _logger.LogInformation("CSL service-GetCustomerData-request {@Request} {@RequestUrl}", jsonRequest, path);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetCustomerData-requestUrl {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-GetCustomerData-response {@RequestUrl} {@Response}", responseData.url, returnValue);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }
        public async Task<T> InsertMPEnrollment<T>(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for InsertMPEnrollment service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                _logger.LogInformation("CSL Call Request-InsertMPEnrollment {@Request} {@RequestUrl}", request, path);

                var enrollmentData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (enrollmentData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL Call -InsertMPEnrollment {@RequestUrl} error {@Response}", enrollmentData.url, enrollmentData.response);
                    if (enrollmentData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(enrollmentData.response);
                }

                _logger.LogInformation("CSL Call Response-InsertMPEnrollment {@RequestUrl} {@Response}", enrollmentData.url, enrollmentData.response);

                return JsonConvert.DeserializeObject<T>(enrollmentData.response);
            }
        }

        public async Task<T> GetProfile<T>(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for CSL-GetProfile service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                _logger.LogInformation("CSL Call-GetProfile Request {@Path} {@Request}", path, request);

                var profileData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (profileData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL Call-GetProfile {@RequestUrl} error {@Response}", profileData.url, profileData.response);
                    if (profileData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(profileData.response);
                }

                _logger.LogInformation("CSL Call Response-GetProfile {@RequestUrl} {@Response}", profileData.url, profileData.response);

                return JsonConvert.DeserializeObject<T>(profileData.response);
            }
        }

        public async Task<T> InsertTraveler<T>(string token, string request, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for InsertTraveler service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
                string path = "/InsertTraveler";
                _logger.LogInformation("CSL Call Request InsertTraveler {@Request} and {@RequestUrl}", request, path);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("InsertTraveler Request url for get profile {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                _logger.LogInformation("CSL Call Response-InsertTraveler {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
        public async Task<string> GetOnlyEmpID(string token, string requestData, string sessionId, string path = "")
        {
            using (_logger.BeginTimedOperation("Total time taken for GetOnlyEmpID service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var enrollmentData = await _resilientClient.PostHttpAsyncWithOptions(path, requestData, headers);

                if (enrollmentData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetOnlyEmpID-service {requestUrl} error {response} for {sessionId}", enrollmentData.url, enrollmentData.statusCode, sessionId);
                    if (enrollmentData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(enrollmentData.response);
                }

                _logger.LogInformation("AccountManagement-GetOnlyEmpID-service {requestUrl} and {sessionId}", enrollmentData.url, sessionId);

                return enrollmentData.response;
            }
        }
        public async Task<(T response, long callDuration)> GetMileagePluses<T>(string token, string sessionId, string jsonRequest)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetMileagePluses service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/GetMileagePluses");
                _logger.LogInformation("CSL service-GetMileagePluses {@RequestUrl} {@Request}", path, jsonRequest);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetMileagePluses {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-GetMileagePluses {@RequestUrl} {@Response}", responseData.url, responseData.response);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }
    }
}
