using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Loyalty
{
    public class LoyaltyUCBService : ILoyaltyUCBService
    {
        private readonly ICacheLog<LoyaltyUCBService> _logger;
        private readonly IResilientClient _resilientClient;

        public LoyaltyUCBService([KeyFilter("LoyaltyUCBClientKey")] IResilientClient resilientClient, ICacheLog<LoyaltyUCBService> logger)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetLoyaltyBalance(string token, string mpnumber, string sessionId)
        {
            try
            {
                using (_logger.BeginTimedOperation("Total time taken for GetLoyaltyBalance service call", transationId: sessionId))
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                    var glbData = await _resilientClient.GetHttpAsyncWithOptions(mpnumber, headers,true);

                    if (glbData.statusCode == HttpStatusCode.OK)
                    {
                        _logger.LogInformation("CSL Service-GetLoyaltyBalance {requestUrl} and {response}", glbData.url, glbData.response);
                        return glbData.response;
                    }
                        
                    _logger.LogError("CSL Service-GetLoyaltyBalance {requestUrl} error {response}", glbData.url, glbData.statusCode);
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    _logger.LogError("CSL Service-GetLoyaltyBalance WebException {errorResponse}",errorResponse);
                }
            }

            return default;
        }

        public async Task<string> GetAccountPremier(string token, string requestData, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetAccountPremier service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var glbData = await _resilientClient.GetHttpAsyncWithOptions("", headers);

                if (glbData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetAccountPremier-service {requestUrl} error {response} for {sessionId}", glbData.url, glbData.statusCode, sessionId);
                    if (glbData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(glbData.response);
                }

                _logger.LogInformation("AccountManagement-GetAccountPremier-service {requestUrl} and {sessionId}", glbData.url, sessionId);

                return glbData.response;
            }
        }
    }
}
