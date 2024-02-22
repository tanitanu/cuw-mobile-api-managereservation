using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class ValidateHashPinService : IValidateHashPinService
    {
        private readonly ICacheLog<ValidateHashPinService> _logger;
        private readonly IResilientClient _resilientClient;

        public ValidateHashPinService([KeyFilter("ValidateHashPinOnPremSqlClientKey")] IResilientClient resilientClient, ICacheLog<ValidateHashPinService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<T> ValidateHashPin<T>(string accountNumber, string hashPinCode, int applicationId, string deviceId, string appVersion, string transactionId, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for ValidateHashPin service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
                string requestData = string.Format("/ValidateHashPin/ValidateHashPinAndGetAuthToken?accountNumber={0}&hashPinCode={1}&applicationId={2}&deviceId={3}&appVersion={4}&transactionId={5}&sessionId={6}", accountNumber, hashPinCode, applicationId, deviceId, appVersion,transactionId,sessionId);
                _logger.LogInformation("ValidateHashPin service {@RequestData}", requestData);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("ValidateHashPin service {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("ValidateHashPin service {@RequestUrl} {@Response}", responseData.url, responseData.response);

                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
