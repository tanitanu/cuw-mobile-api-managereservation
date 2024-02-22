using Autofac.Features.AttributeFilters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class GetBagTrackingService : IGetBagTrackingService
    {
        private readonly ICacheLog<GetBagTrackingService> _logger;
        private readonly IResilientClient _resilientClient;

        public GetBagTrackingService(ICacheLog<GetBagTrackingService> logger, [KeyFilter("GetBagTrackingOnPremSQLClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<int> HasCheckedBag(string request)
        {
            using (_logger.BeginTimedOperation("Total time taken for HasCheckedBag OnPremService call"))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };
                _logger.LogInformation("OnPremService-HasCheckedBag {requestData}", request);

                string path = "/iPhone/GetSelectBags";

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("OnPremService-HasCheckedBag {requestUrl} error {response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("OnPremService - HasCheckedBag {requestUrl} {response}", responseData.url, responseData.response);

                var responseObject = Convert.ToInt32(responseData.response);
                return responseObject;
            }
        }
        public async Task<string> HasCheckedBagV2(string request, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for HasCheckedBag cloud call"))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };
                _logger.LogInformation("cloud-HasCheckedBag {requestData}", request);

                var responseData = await _resilientClient.PostAsyncCache(path, request, headers).ConfigureAwait(false);
                return responseData;
            }
        }
    }
}
