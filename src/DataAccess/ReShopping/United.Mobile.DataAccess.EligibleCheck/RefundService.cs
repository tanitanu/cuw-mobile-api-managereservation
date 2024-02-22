using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ReShop
{
    public class RefundService : IRefundService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<RefundService> _logger;

        public RefundService([KeyFilter("RefundServiceClientKey")] IResilientClient resilientClient, ICacheLog<RefundService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        //public async Task<T> EligibleCheck<T>(string token, string sessionId, string recordLocator, string appId, bool isAllowAward, bool allowFFC)
        //{
        //    using (_logger.BeginTimedOperation("Total time taken for EligibleCheck service call", transationId: sessionId))
        //    {
        //        Dictionary<string, string> headers = new Dictionary<string, string>
        //             {
        //                  { "Authorization", token }
        //             };

        //        var path = string.Format("/Eligible?recordLocator={0}&channel={1}&path=change&AllowAward={2}&allowFutureFlightCredit={3}",
        //                    recordLocator, appId, isAllowAward, allowFFC);

        //        var glbData = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

        //        if (glbData.statusCode != HttpStatusCode.OK)
        //        {
        //            _logger.LogError("EligibleCheck-service {requestUrl} error {response} for {sessionId}", glbData.url, glbData.statusCode, sessionId);
        //            if (glbData.statusCode != HttpStatusCode.BadRequest)
        //                throw new Exception(glbData.response);
        //        }

        //        _logger.LogInformation("EligibleCheck-service {requestUrl} and {sessionId}", glbData.url, sessionId);

        //        return JsonConvert.DeserializeObject<T>(glbData.response);
        //    }
        //}

        public async Task<string> PostEligibleCheck<T>(string token, string sessionId, string path, string request)
        {
            using (_logger.BeginTimedOperation("Total time taken for PostEligibleCheck service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                _logger.LogInformation("CSL service-PostEligibleCheck Request:{@Request} Path:{@Path}", request, path);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-PostEligibleCheck {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-PostEligibleCheck {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }

        public async Task<string> GetEligibleCheck<T>(string token, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetEligibleCheck service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                _logger.LogInformation("CSL service-GetEligibleCheck {@RequestData}", path);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);


                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetEligibleCheck {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetEligibleCheck {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}
