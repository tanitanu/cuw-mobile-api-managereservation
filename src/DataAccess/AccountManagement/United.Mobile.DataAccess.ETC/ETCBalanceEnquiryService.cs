using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.ETC
{
    public class ETCBalanceEnquiryService: IETCBalanceEnquiryService
    {
        private readonly ICacheLog<ETCBalanceEnquiryService> _logger;
        private readonly IResilientClient _resilientClient;

        public ETCBalanceEnquiryService(ICacheLog<ETCBalanceEnquiryService> logger, [KeyFilter("ETCBalanceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetETCBalanceInquiry(string path, string request, string sessionId, string token)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                           {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            _logger.LogInformation("CSL service-GetETCBalanceInquiry Request:{request} Path:{path} and {sessionId}", JsonConvert.SerializeObject(request), path, sessionId);

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetETCBalanceInquiry call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(true);

                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-GetETCBalanceInquiry {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            throw new Exception(responseData.response);
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-GetETCBalanceInquiry {requestUrl} and {sessionId}", responseData.url, sessionId);

                    return responseData.response;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-GetETCBalanceInquiry error {stackTrace} for {sessionId}", ex.StackTrace, sessionId);
                }

                return default;
            }
        }
    }
}
