using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Payment;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Payment
{
    public class GenerateHashService : IGenerateHashService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<GenerateHashService> _logger;
        public GenerateHashService([KeyFilter("generateHashClientKey")] IResilientClient resilientClient, ICacheLog<GenerateHashService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GenerateHash(string token, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var documentData = await _resilientClient.GetHttpAsync(string.Empty, headers).ConfigureAwait(false);
            var response = documentData.Item1;
            var statusCode = documentData.Item2;
            var url = documentData.Item3;

            _logger.LogInformation("eRes-GenerateHash-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GenerateHash-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmpGenerateHashResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }
}
