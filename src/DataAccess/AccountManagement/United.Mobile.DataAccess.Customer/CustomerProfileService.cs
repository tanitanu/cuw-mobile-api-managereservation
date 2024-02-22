using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Customer
{
    public class CustomerProfileService : ICustomerProfileService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<CustomerProfileService> _logger;
        private readonly IConfiguration _configuration;
        public CustomerProfileService([KeyFilter("CustomerProfileServiceClientKey")] IResilientClient resilientClient, ICacheLog<CustomerProfileService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<T> GetAccountStatus<T>(string mileagPlusNumber, string token, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}", mileagPlusNumber);
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
    }
}
