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

namespace United.Mobile.DataAccess.Loyalty
{
    public class LoyaltyAccountService : ILoyaltyAccountService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<LoyaltyAccountService> _logger;
        private readonly IConfiguration _configuration;
        public LoyaltyAccountService([KeyFilter("LoyaltyAccountClientKey")] IResilientClient resilientClient, ICacheLog<LoyaltyAccountService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetCurrentMembershipInfo(string mpNumber)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
            var path = string.Format("{0}/uclub/history", mpNumber);
            using (_logger.BeginTimedOperation("Total time taken for GetCurrentMembershipInfo call", transationId: string.Empty))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL Service -GetCurrentMembershipInfo {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, string.Empty);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception();
                }
                _logger.LogInformation("CSL Service- GetCurrentMembershipInfo {requestUrl} , {@response}", responseData.url, responseData.response);

                return responseData.response;
            }
        }

        public async Task<T> GetAccountProfileInfo<T>(string token, string mileagePlusNumber, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}", mileagePlusNumber);
            using (_logger.BeginTimedOperation("Total time taken for GetAccountProfileInfo call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetAccountProfileInfo {@Request}", requestData);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetAccountProfileInfo {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL service-GetAccountProfileInfo {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);                
            }
        }

        public async Task<T> GetAccountSummary<T>(string token, string mileagePlusNumber, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}/", mileagePlusNumber);
            var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetAccountSummary {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            return JsonConvert.DeserializeObject<T>(responseData.response);
        }

        public async Task<string> GetAccountProfile(string token, string mileagePlusNumber, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}", mileagePlusNumber);           
            using (_logger.BeginTimedOperation("Total time taken for CSL service-GetAccountProfile service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetAccountProfile {@Request}", requestData);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetAccountProfile {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL service-GetAccountProfile {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}
