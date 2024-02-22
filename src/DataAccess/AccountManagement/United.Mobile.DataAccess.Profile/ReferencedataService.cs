using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Http;
using Autofac.Features.AttributeFilters;
using Newtonsoft.Json;
using United.Utility.Serilog;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.Profile
{
    public class ReferencedataService : IReferencedataService
    {
        private readonly ICacheLog<ReferencedataService> _logger;
        private readonly IResilientClient _resilientClient;
        public ReferencedataService(ICacheLog<ReferencedataService> logger, [KeyFilter("ReferencedataClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetAndValidateStateCode(string token, string urlPath, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetAndValidateStateCode service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };            
                var pnrData = await _resilientClient.GetHttpAsyncWithOptions(urlPath, headers);

                if (pnrData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Account Management-GetAndValidateStateCode-service {requestUrl} error {response} for {sessionId}", pnrData.url, pnrData.statusCode, sessionId);
                    if (pnrData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(pnrData.response);
                }

                _logger.LogInformation("Account Management-GetAndValidateStateCode-service {requestUrl} and {sessionId}", pnrData.url, sessionId);

                return pnrData.response;
            }
        }
        public async Task<T> GetDataPostAsync<T>(string actionName, string token, string sessionId, string request)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetDataPostAsync service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var response = await _resilientClient.PostHttpAsyncWithOptions(actionName, request, headers);
                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-Referencedata PostAsync {requestUrl} error {response} for {sessionId}", response.url, response.response, sessionId);
                    if (response.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-Referencedata PostAsync {requestUrl} and {sessionId}", response.url, sessionId);
                return (response.response == null) ? default : JsonConvert.DeserializeObject<T>(response.response);
            }
        }

        public async Task<T> GetDataGetAsync<T>(string actionName, string token, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            _logger.LogInformation("CSL service GetDataGetAsync parameters ActionName: {@ActionName}", actionName);

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetDataGetAsync service call", transationId: sessionId))
            {
                try
                {
                    var response = await _resilientClient.GetHttpAsyncWithOptions(actionName, headers);
                    if (response.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-Referencedata GetAsync {@RequestUrl} error {@Response}", response.url, response.response);
                        if (response.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-Referencedata Referencedata {@RequestUrl} and {@Response}", response.url, response.response);
                    return (response.response == null) ? default : JsonConvert.DeserializeObject<T>(response.response);
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-Referencedata Referencedata error {@Exception}", JsonConvert.SerializeObject(ex));
                }

                return default;

            }
        }

        public async Task<(T Response, long callDuartion)> RewardPrograms<T>(string token, string sessionId)
        {
            
            string returnValue = string.Empty;
            string actionName = @"/RewardPrograms";

            Dictionary<string, string> headers = new Dictionary<string, string>
                 {
                      {"Accept", "application/json"},
                      { "Authorization", token }
                 };
            var response = await _resilientClient.GetHttpAsyncWithOptions(actionName, headers);
            if (response.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-RewardPrograms GetAsync {@RequestUrl} error {@Response}", response.url, response.response);
                if (response.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            returnValue = response.response != null ? response.response : string.Empty;
            _logger.LogInformation("CSL service-RewardPrograms GetAsync {@RequestUrl} and {@Response}", response.url, response.response);
            
            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), 0);
        }

        public async Task<T> GetNationalityResidence<T>(string actionName, string token, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetNationalityResidence service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("/{0}", actionName);
                _logger.LogInformation("CSL service-GetNationalityResidence {@Url}", requestData);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetNationalityResidence {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetNationalityResidence {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<T> GetSpecialNeedsInfo<T>(string actionName, string request, string token, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetSpecialNeedsInfo service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("/{0}", actionName);             
                using (_logger.BeginTimedOperation("Total time taken for CSL service-GetSpecialNeedsInfo business call", transationId: sessionId))
                {
                    _logger.LogInformation("CSL service-GetSpecialNeedsInfo {@Request} {@Url}", request, requestData);
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-GetSpecialNeedsInfo {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    _logger.LogInformation("CSL service-GetSpecialNeedsInfo {@RequestUrl} {@Response}", responseData.url, responseData.response);

                    return JsonConvert.DeserializeObject<T>(responseData.response);
                }
            }
        }

        public async Task<string> GetRewardPrograms(string token, string urlPath, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                 {
                      { "Authorization", token }
                 };

            var pnrData = await _resilientClient.GetHttpAsyncWithOptions(urlPath, headers);

            if (pnrData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("GetRewardPrograms-service {requestUrl} error {response} for {sessionId}", pnrData.url, pnrData.statusCode, sessionId);
                if (pnrData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(pnrData.response);
            }

            _logger.LogInformation("GetRewardPrograms-service {requestUrl} and {sessionId}", pnrData.url, sessionId);

            return pnrData.response;
        }
    }
}
