using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class IsInBetaService : IIsInBetaService
    {
        private readonly ICacheLog<IsInBetaService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IHostingEnvironment _hostingEnvironment;

        public IsInBetaService([KeyFilter("BetaOnPremSqlClientKey")] IResilientClient resilientClient, ICacheLog<IsInBetaService> logger, IHostingEnvironment hostingEnvironment)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<T> IsInBeta<T>(string mileagePlusAccountNumber, int featureId, int applicationId, string applicationVersion, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for IsInBeta call", transationId: sessionId))
            {
                var requestData = string.Format("MileagePlusAccountNumber = {0}", mileagePlusAccountNumber);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("", requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Ancillary Common - IsInBeta {requestUrl} error {response} for {sessionId} ", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("Ancillary Common - IsInBeta {requestUrl} info {response} for {sessionId}", responseData.url, responseData.response, sessionId);

                var responseObject = JsonConvert.DeserializeObject<T>(responseData.response);

                return responseObject;
            }
        }
    }
}
