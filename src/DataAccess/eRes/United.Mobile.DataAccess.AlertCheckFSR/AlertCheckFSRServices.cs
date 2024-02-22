using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.AlertCheckFSR;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.AlertCheckFSR
{
    public class AlertCheckFSRService:  IAlertCheckFSRService
    {

        private readonly IResilientClient _alertCheckFSRResilientClient;
        private readonly ICacheLog<AlertCheckFSRService> _logger;
        public AlertCheckFSRService([KeyFilter("alertCheckFSRClientKey")] IResilientClient alertCheckFSRResilientClient, ICacheLog<AlertCheckFSRService> logger)
        {
            _alertCheckFSRResilientClient = alertCheckFSRResilientClient;
            _logger = logger;
        }
        public async Task<string> GetAlertCheckFSR(string token, string requestData, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var alertCheckData = await _alertCheckFSRResilientClient.PostHttpAsync(string.Empty, requestData, headers).ConfigureAwait(false);

            var response = alertCheckData.Item1;
            var statusCode = alertCheckData.Item2;
            var url = alertCheckData.Item3;

            _logger.LogInformation("eRes-AlertCheckFSR-service {requestUrl} and {sessionId}", url,  sessionId);
            
            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-AlertCheckFSR-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<AlertResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }
}
