using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.MicrositeServices
{
    public class PrepaidBagsService : IPrepaidBagsService
    {
        private readonly ICacheLog<PrepaidBagsService> _logger;
        private readonly IResilientClient _resilientClient;

        public PrepaidBagsService([KeyFilter("PrepaidBagsClientKey")] IResilientClient resilientClient, ICacheLog<PrepaidBagsService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<(string response, long callDuration)> GetMicrositeService(string token, string sessionId, string action)
        {
            string response = string.Empty;
            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetMicrositeService service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("/{0}", action);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetMicrositeService {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetMicrositeService {requestUrl}, {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                response = responseData.response;
            }

            var callDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            return (response, callDuration);
        }

        public async Task<(string response, long callDuration)> GetDynamicOffers(string token, string sessionId, string action, string request)
        {
            string response = string.Empty;
            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetDynamicOffers service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string requestData = string.Format("/{0}", action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetDynamicOffers {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetDynamicOffers {requestUrl}, {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                response = responseData.response;
            }

            var callDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            return (response, callDuration);
        }
    }
}
