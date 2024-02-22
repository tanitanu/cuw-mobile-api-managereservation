using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.UpgradeList
{
    public class CabinBrandingService : ICabinBrandingService
    {
        private readonly ICacheLog<CabinBrandingService> _logger;
        private readonly IResilientClient _resilientClient;

        public CabinBrandingService([KeyFilter("CabinBrandingServiceClientKey")] IResilientClient resilientClient
            , ICacheLog<CabinBrandingService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetCabinBranding(string token, string sessionId, string jsonRequest)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            _logger.LogInformation("GetCabinBranding request payload {clientRequest} and {sessionId} ", jsonRequest, sessionId);

            using (_logger.BeginTimedOperation("Total time taken for GetCabinBranding service call", transationId: sessionId))
            {
                string path = "/CabinBranding";
                
                _logger.LogInformation("GetCabinBranding CSL Service {path} and {sessionId}", path, sessionId);
                
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetCabinBranding CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("GetCabinBranding CSL Service {requestUrl} , {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }
    }
}
