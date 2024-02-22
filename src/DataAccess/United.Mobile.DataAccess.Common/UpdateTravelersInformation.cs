using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class UpdateTravelersInformation : IUpdateTravelersInformation
    {
        private readonly IResilientClient _resilientClient;
        private readonly ILogger<UpdateTravelersInformation> _logger;

        public UpdateTravelersInformation([KeyFilter("UpdateTravelerInfoSeatPrefKey")] IResilientClient resilientClient, ILogger<UpdateTravelersInformation> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<T> UpdateTravelersInfo<T>(string request, string transactionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for VerifyMileagePlusHashpin service call", transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
                string path = "";

                _logger.LogInformation("UpdateTravelerInfoSeatPref {@request}", request);
                using (_logger.BeginTimedOperation("Total time taken for UpdateTravelerInfoSeatPref call", transationId: transactionId))
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("UpdateTravelerInfoSeatPref Service {@Url} error {@response}", responseData.url, responseData.response);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            throw new System.Exception(responseData.response);
                    }

                    _logger.LogInformation("UpdateTravelerInfoSeatPref Service {@Url} , {@response}", responseData.url, responseData.response);
                    return JsonConvert.DeserializeObject<T>(responseData.response);
                }
            }

        }
    }
}
