using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Customer
{
    public class CustomerProfileCreditCardsService : ICustomerProfileCreditCardsService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<CustomerProfileCreditCardsService> _logger;        
        private readonly IConfiguration _configuration;

        public CustomerProfileCreditCardsService(
              [KeyFilter("CSLGetProfileCreditCardsServiceKey")] IResilientClient resilientClient
            , ICacheLog<CustomerProfileCreditCardsService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<T> GetProfileCreditCards<T>(string token, string sessionId, string mpNumber)
        {
            string actionName = string.Empty;
            using (_logger.BeginTimedOperation("Total time taken for GetProfile CreditCards service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("{0}", mpNumber);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL GetProfile CreditCards service {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL GetProfile CreditCards service {requestUrl},{response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
