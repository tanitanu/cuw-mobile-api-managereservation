using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Loyalty
{
    public class LoyaltyMemberProfileService : ILoyaltyMemberProfileService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<LoyaltyAccountService> _logger;
        private readonly IConfiguration _configuration;
        public LoyaltyMemberProfileService([KeyFilter("LoyaltyMemberProfileClientKey")] IResilientClient resilientClient, ICacheLog<LoyaltyAccountService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<T> GetAccountMemberProfile<T>(string token, string mileagePlusNumber, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}", mileagePlusNumber);
            using (_logger.BeginTimedOperation("Total time taken for CSL service-GetAccountMemberProfile service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetAccountMemberProfile {@Request}", requestData);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetAccountMemberProfile {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL service-GetAccountMemberProfile {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}