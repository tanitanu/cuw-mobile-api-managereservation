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

namespace United.Mobile.DataAccess.CodeTable
{
    public class JumioService : IJumioService
    {
        private readonly IResilientClient _resilientClient;
       private readonly ICacheLog<JumioService> _logger;
        public JumioService([KeyFilter("JumioClientKey")] IResilientClient resilientClient, ICacheLog<JumioService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<T> GetDeleteJumioScan<T>(string token,string passportScanReferenceID, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetBoardingPass service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("/{0}", passportScanReferenceID);
                var responseData = await _resilientClient.DeleteAsync(requestData, headers).ConfigureAwait(false);
                _logger.LogInformation("CSL service-GetDeleteJumioScan {requestUrl} and {sessionId}", responseData.url, sessionId);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetDeleteJumioScan {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
