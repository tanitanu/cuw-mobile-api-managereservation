using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class ValidateAccountFC : IValidateAccountFC
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<ValidateAccountFC> _logger;

        public ValidateAccountFC([KeyFilter("ValidateAccountOnPremClientKey")] IResilientClient resilientClient, ICacheLog<ValidateAccountFC> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<bool> ValidateAccount(string accountNumber, string pinCode, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
            string path = string.Format("/ValidateAccount?accountNumber={0}&pinCode={1}&sessionId={2}", accountNumber, pinCode, sessionId);
            using (_logger.BeginTimedOperation("Total time taken for ValidateAccountFromCache call", transationId: sessionId))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("ValidateAccount SQLDB Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                _logger.LogInformation("ValidateAccount SQLDB Service {requestUrl} , {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                return Convert.ToBoolean(responseData.response);
            }
        }
    }
}
