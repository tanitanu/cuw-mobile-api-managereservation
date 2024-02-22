using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ManageReservation
{
    public class SendReceiptByEmailService:ISendReceiptByEmailService
    {
        private readonly ICacheLog<RequestReceiptByEmailService> _logger;
        private readonly IResilientClient _resilientClient;

        public SendReceiptByEmailService(ICacheLog<RequestReceiptByEmailService> logger, [KeyFilter("SendReceiptByEmailClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        } 

        public async Task<string> SendReceiptByEmailViaCSL(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for SendReceiptByEmailViaCSL service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                _logger.LogInformation("CSL service-SendReceiptByEmailViaCSL {requestData} {sessionId}", request, sessionId);

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-SendReceiptByEmailViaCSL {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-SendReceiptByEmailViaCSL {response} and {sessionId}", responseData.response, sessionId);
                return responseData.response;
            }
        }

    }
}
