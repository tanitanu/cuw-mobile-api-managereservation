using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.CancelReservation
{
    public class CancelRefundService : ICancelRefundService
    {
        private readonly ICacheLog<CancelRefundService> _logger;
        private readonly IResilientClient _resilientClient;
        public CancelRefundService(ICacheLog<CancelRefundService> logger, [KeyFilter("CancelRefundClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetRefund(string token, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetRefund service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetRefund {path} and {sessionId}", path, sessionId);

                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetRefund {requestUrl} error {response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetRefund {response}", responseData.response);
                return responseData.response;
            }
        }
        public async Task<string> GetQuoteRefund(string token, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetQuoteRefund service call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetQuoteRefund {path} and {sessionId}", path, sessionId);

                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetQuoteRefund {requestUrl} error {response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetQuoteRefund {response}", responseData.response);
                return responseData.response;
            }
        }
    }
}
