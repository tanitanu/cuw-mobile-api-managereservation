using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Payment
{
    public class GooglePayAccessTokenService : IGooglePayAccessTokenService
    {
        private readonly ICacheLog<GooglePayAccessTokenService> _logger;
        private readonly IResilientClient _resilientClient;

        public GooglePayAccessTokenService(ICacheLog<GooglePayAccessTokenService> logger, [KeyFilter("GooglePayAccessToeknClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetGooglePayAccessToken(string token, string requestData, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GooglePay-GeGooglePaytAccessToken-service service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Content-Type", "application/x-www-form-urlencoded" }
                     };
                
               var Data = await _resilientClient.PostHttpAsyncWithOptions(string.Empty, requestData, headers, "application/x-www-form-urlencoded");

                if (Data.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GooglePay-GeGooglePaytAccessToken-service {requestUrl} error {response} for {sessionId}", Data.url, Data.statusCode, sessionId);
                    if (Data.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(Data.response);
                }

                _logger.LogInformation("GooglePay-GeGooglePaytAccessToken-service {requestUrl} and {sessionId}", Data.url, sessionId);

                return Data.response;
            }
        }
    }

}
