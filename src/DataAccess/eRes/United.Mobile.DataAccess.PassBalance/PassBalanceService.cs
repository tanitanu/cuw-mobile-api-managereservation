using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassBalance;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PassBalance
{
    public class PassBalanceService : IPassBalanceService
    {
        private readonly ICacheLog<PassBalanceService> _logger;
        private readonly IResilientClient _pbResilientClient;
        public PassBalanceService(ICacheLog<PassBalanceService> logger, [KeyFilter("pbClientKey")]IResilientClient pbResilientClient)
        {
            _logger = logger;
            _pbResilientClient = pbResilientClient;
        }

        public async Task<string> GetPassBalance(string token, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var passData = await _pbResilientClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = passData.Item1;
            var statusCode = passData.Item2;
            var url = passData.Item3;

            _logger.LogInformation("eRes-GetPassBalance-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetPassBalance-service {requestUrl} error {@response} for this {sessionId}", url, JsonConvert.DeserializeObject<PassBalanceResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }
}
