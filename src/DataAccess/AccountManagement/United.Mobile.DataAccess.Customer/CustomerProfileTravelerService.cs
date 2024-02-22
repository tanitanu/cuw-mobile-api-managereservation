using Autofac.Features.AttributeFilters;
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
    public class CustomerProfileTravelerService :ICustomerProfileTravelerService
    {
        private readonly ICacheLog<CustomerProfileTravelerService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerProfileTravelerService(
              [KeyFilter("CSLGetProfileTravelerDetailsServiceKey")] IResilientClient resilientClient
            , ICacheLog<CustomerProfileTravelerService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<T> GetProfileTravelerInfo<T>(string token, string sessionId, string mpNumber)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetProfile all traveler service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL GetProfile all traveler service {@Request}", mpNumber);

                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("{0}", mpNumber);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL GetProfile all traveler service {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL GetProfile all traveler service {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
