using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class CSLStatisticsService : ICSLStatisticsService
    {
        private readonly ICacheLog<CSLStatisticsService> _logger;
        private readonly IResilientClient _resilientClient;

        public CSLStatisticsService(ICacheLog<CSLStatisticsService> logger, [KeyFilter("CSLStatisticsOnPremSqlClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<bool> AddCSLStatistics(string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for AddCSLStatistics call", transationId: sessionId))
            {
                _logger.LogInformation("Shopping - AddCSLStatistics info {@RequestData}", requestData);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("", requestData).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Shopping - AddCSLStatistics {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("Shopping - AddCSLStatistics {@RequestUrl} info {@Response}", responseData.url, responseData.response);

                var responseObject = Convert.ToBoolean(responseData.response);

                return responseObject;
            }
        }
    }
}
