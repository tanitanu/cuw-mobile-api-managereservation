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
    public class CustomerSearchService : ICustomerSearchService
    {
        private readonly ICacheLog<CustomerDataService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerSearchService(
              [KeyFilter("CustomerSearchClientKey")] IResilientClient resilientClient
            , ICacheLog<CustomerDataService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> Search(string token, string sessionId, string path = "")
        {
            using (_logger.BeginTimedOperation("Total time taken for GetOnlyEmpID service call", transationId: sessionId))
            {
                 var headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var data = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

                if (data.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-Search-service {requestUrl} error {response} for {sessionId}", data.url, data.statusCode, sessionId);
                    if (data.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(data.response);
                }

                _logger.LogInformation("AccountManagement-Search-service {requestUrl} and {sessionId}", data.url, sessionId);

                return data.response;
            }
        }
    }
}
