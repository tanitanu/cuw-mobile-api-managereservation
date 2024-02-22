using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.Common
{
    public class SSOTokenKeyService : ISSOTokenKeyService
    {
        private readonly ICacheLog<SSOTokenKeyService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;
        public SSOTokenKeyService([KeyFilter("SSOTokenClientKey")] IResilientClient resilientClient, ICacheLog<SSOTokenKeyService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> DecryptToken(string token, string sessionId, string encryptedData)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                           {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for SSOTokenKey DecryptToken call", transationId: sessionId))
            {
                string path = "?token=" + encryptedData;
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path,string.Empty, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL PartnerSingleSignOn Decrypt {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                _logger.LogInformation("CSL PartnerSingleSignOn Decrypt {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> EncryptKey(string token, string sessionId, string Key)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                           {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for SSOTokenKey EncryptKey call", transationId: sessionId))
            {
                string path = "?key=" + Key;
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, string.Empty, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL PartnerSingleSignOn Encrypt {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                _logger.LogInformation("CSL PartnerSingleSignOn Encrypt {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }

    }
}
