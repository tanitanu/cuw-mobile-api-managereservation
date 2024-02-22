using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.BagTracking
{
    public class ProdTokenService : IProdTokenService
    {
        private readonly IResilientClient  _resilientClient;
        private readonly ICacheLog<ProdTokenService> _logger;

        public ProdTokenService([KeyFilter("ProdTokenClientKey")] IResilientClient resilientClient, ICacheLog<ProdTokenService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetProdToken(string token, string sessionId, int applicationId, int appVersion, string accessCode, string transactionId, string languageCode)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                           {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("applicationId={0}appVersion={1}accessCode={2}transactionId={3}languageCode={4}", applicationId, appVersion, accessCode, transactionId, languageCode);
            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetProdToken dataAccess call", transationId: sessionId))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(true);
                _logger.LogInformation("mREST service-GetProdToken {requestUrl} and {sessionId}", responseData.url, sessionId);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("mREST service-GetProdToken {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                _logger.LogInformation("mREST service-GetProdToken {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }
    }
}
