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
    public class DataVaultService : IDataVaultService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<DataVaultService> _logger;

        public DataVaultService(ICacheLog<DataVaultService> logger, [KeyFilter("dataVaultClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> GetDataVault(string token, string requestData, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var dataVaultData = await _resilientClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = dataVaultData.Item1;
            var statusCode = dataVaultData.Item2;
            var url = dataVaultData.Item3;

            _logger.LogInformation("eRes-GetDataVault-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetDataVault-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EmpDataVaultResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
           
        }
    }
}
