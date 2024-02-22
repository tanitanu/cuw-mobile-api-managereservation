using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PreOrderMeals
{
    public class GetPNRByRecordLocatorService: IGetPNRByRecordLocatorService
    {
        private readonly ICacheLog<GetPNRByRecordLocatorService> _logger;
        private readonly IResilientClient _resilientClient;
        public GetPNRByRecordLocatorService(ICacheLog<GetPNRByRecordLocatorService> logger, [KeyFilter("GetPNRByRecordLocatorClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetPNRByRecordLocator(string request, string transactionId, string path)
        {
            _logger.LogInformation("GetInflightMealOffersForDeeplink-GetPNRByRecordLocator request:{request} with transactionId:{Id}", request, transactionId);

            using (_logger.BeginTimedOperation("Total time taken for GetPNRByRecordLocator service call", transationId: transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetInflightMealOffersForDeeplink - GetPNRByRecordLocator {requestUrl} error {response} for {transactionId}", responseData.url, responseData.response, transactionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("GetInflightMealOffersForDeeplink - GetPNRByRecordLocator {requestUrl}, {response} and {transactionId}", responseData.url, responseData.response, transactionId);
                return responseData.response;
            }
        }
    }
}
