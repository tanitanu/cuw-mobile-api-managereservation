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
    public class RequestReceiptByEmailService:IRequestReceiptByEmailService
    {
        private readonly ICacheLog<RequestReceiptByEmailService> _logger;
        private readonly IResilientClient _resilientClient;

        public RequestReceiptByEmailService(ICacheLog<RequestReceiptByEmailService> logger, [KeyFilter("RequestReceiptByEmailClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        } 

        public async Task<string> PostReceiptByEmailViaCSL(string token, string request, string sessionId, string ConfirmationID)
        {
            using (_logger.BeginTimedOperation("Total time taken for PostReceiptByEmailViaCSL service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("?ConfirmationID={0}&PutInReceiptQueue=true", ConfirmationID);
                _logger.LogError("CSL service-PostReceiptByEmailViaCSL {requestData} {path}", request, path);

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-PostReceiptByEmailViaCSL {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-PostReceiptByEmailViaCSL {response} and {sessionId}", responseData.response, sessionId);
                return responseData.response;
            }
        }

    }
}
