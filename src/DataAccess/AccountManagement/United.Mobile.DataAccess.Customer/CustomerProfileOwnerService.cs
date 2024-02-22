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
    public class CustomerProfileOwnerService : ICustomerProfileOwnerService
    {
        private readonly ICacheLog<CustomerProfileOwnerService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerProfileOwnerService(
              [KeyFilter("CSLGetProfileOwnerServiceKey")] IResilientClient resilientClient
            , ICacheLog<CustomerProfileOwnerService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<T> GetProfileOwnerInfo<T>(string token, string sessionId, string mpNumber)
        {
            string actionName = string.Empty;
            using (_logger.BeginTimedOperation("Total time taken for GetProfile Owner service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("{0}", mpNumber);

                _logger.LogInformation("CSL GetProfile Owner service {@RequestUrl}", requestData);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL GetProfile Owner service {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL GetProfile Owner service {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
