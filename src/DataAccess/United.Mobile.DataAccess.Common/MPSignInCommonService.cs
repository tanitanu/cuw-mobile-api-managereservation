using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class MPSignInCommonService : IMPSignInCommonService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ILogger<MPSignInCommonService> _logger;

        public MPSignInCommonService([KeyFilter("MPSignInCommonClientKey")] IResilientClient resilientClient, ILogger<MPSignInCommonService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<T> VerifyMileagePlusHashpin<T>(string request, string transactionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for VerifyMileagePlusHashpin service call", transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
                string path = "/VerifyMileagePlusHashpin";

                _logger.LogInformation("MPSignInCommonService-VerifyMileagePlusHashpin {@request}", request);
                using (_logger.BeginTimedOperation("Total time taken for VerifyMileagePlusHashpin call", transationId: transactionId))
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("MPSignInCommonService-VerifyMileagePlusHashpin Service {@Url} error {@response}", responseData.url, responseData.response);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            throw new System.Exception(responseData.response);
                    }

                    _logger.LogInformation("MPSignInCommonService-VerifyMileagePlusHashpin Service {@Url} , {@response}", responseData.url, responseData.response);
                    return JsonConvert.DeserializeObject<T>(responseData.response);
                }
            }

        }
    }
}
