using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.UpgradeList
{
    public class EnDecryptDataVaultService : IEnDecryptDataVaultService
    {
        private readonly ICacheLog<EnDecryptDataVaultService> _logger;
        private readonly IResilientClient _resilientClient;

        public EnDecryptDataVaultService(ICacheLog<EnDecryptDataVaultService> logger, [KeyFilter("DataVaultTokenClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetDecryptedTextFromDataVault(string token, string sessionId, string jsonRequest)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            _logger.LogInformation("GetDecryptedTextFromDataVault request payload {clientRequest} and {sessionId} ", jsonRequest, sessionId);

            using (_logger.BeginTimedOperation("Total time taken for GetDecryptedTextFromDataVault service call", transationId: sessionId))
            {
                string path = "/AddText";
                _logger.LogInformation("GetDecryptedTextFromDataVault CSL Service {path} and {sessionId}", path, sessionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetDecryptedTextFromDataVault CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("GetDecryptedTextFromDataVault CSL Service {requestUrl} , {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }
    }
}
