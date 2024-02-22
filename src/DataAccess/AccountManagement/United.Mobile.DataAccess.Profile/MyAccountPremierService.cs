using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Profile
{
    public class MyAccountPremierService : IMyAccountPremierService
    {
        private readonly ICacheLog<MyAccountPremierService> _logger;
        private readonly IResilientClient _resilientClient;

        public MyAccountPremierService(ICacheLog<MyAccountPremierService> logger, [KeyFilter("AccountPremierClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<T> GetAccountPremier<T>(string token, string accountNumber, string accessCode, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetAccountPremier service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},//"application/xml;"
                          { "Authorization", token }
                     };

                string path = string.Format("?AccountNumber={0}&RunDate={1}&AccessCode={2}", accountNumber, DateTime.Now.ToString("MM/dd/yyyy"), accessCode);
                var glbData = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

                if (glbData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetAccountPremier-service {requestUrl} error {response} for {sessionId}", glbData.url, glbData.statusCode, sessionId);
                    if (glbData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(glbData.response);
                }

                _logger.LogInformation("AccountManagement-GetAccountPremier-service {requestUrl} and {sessionId}", glbData.url, sessionId);

                return JsonConvert.DeserializeObject<T>(glbData.response);
            }
        }
    }
}
