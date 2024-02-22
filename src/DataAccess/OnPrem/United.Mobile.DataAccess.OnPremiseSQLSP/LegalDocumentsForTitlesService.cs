using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Definition;
using United.Mobile.DataAccess.DynamoDB;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class LegalDocumentsForTitlesService : ILegalDocumentsForTitlesService
    {
        private readonly ICacheLog<LegalDocumentsForTitlesService> _logger;
        private readonly IResilientClient _resilientClient;

        public LegalDocumentsForTitlesService(ICacheLog<LegalDocumentsForTitlesService> logger, [KeyFilter("LegalDocumentsOnPremSqlClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<List<MOBLegalDocument>> GetNewLegalDocumentsForTitles(string titles, string transactionId, bool isTermsnConditions)
        {
            if (titles == null)
            {
                _logger.LogError("GetNewLegalDocumentsForTitles titles is null.");
                return default;
            }

            using (_logger.BeginTimedOperation("Total time taken for GetNewLegalDocumentsForTitles OnPrem service call", transationId: transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var requestObj = string.Format("/GetNewLegalDocumentsForTitles?Titles={0}&transactionId={1}&isTermsnConditions={2}", titles, transactionId, isTermsnConditions);

                _logger.LogInformation("GetNewLegalDocumentsForTitles-OnPrem Service {@RequestObj}", requestObj);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestObj, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetNewLegalDocumentsForTitles-OnPrem Service {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest && responseData.statusCode != HttpStatusCode.NoContent)
                        throw new Exception(null, new Exception(responseData.response));
                }

                _logger.LogInformation("GetNewLegalDocumentsForTitles-OnPrem Service {@RequestUrl} {@Response}", responseData.url, responseData.response);

                return JsonConvert.DeserializeObject<List<MOBLegalDocument>>(responseData.response);
            }
        }
    }
}
