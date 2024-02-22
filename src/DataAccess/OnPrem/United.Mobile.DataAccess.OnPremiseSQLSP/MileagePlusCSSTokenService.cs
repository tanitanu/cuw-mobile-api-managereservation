using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Common;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class MileagePlusCSSTokenService : IMileagePlusCSSTokenService
    {
        private readonly ICacheLog<MileagePlusCSSTokenService> _logger;
        private readonly IResilientClient _resilientClient;

        public MileagePlusCSSTokenService(ICacheLog<MileagePlusCSSTokenService> logger, [KeyFilter("OnPremSQLServiceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<bool> UpdateMileagePlusCSSToken(string sessionId, string transactionId, string mpNumber, string deviceID, int appID, string appVersion, string authToken, bool isAuthTokenValid, DateTime authTokenExpirationDateTime, double tokenExpireInSeconds, bool isTokenAnonymous = false, long customerID = 0)
        {
            using (_logger.BeginTimedOperation("Total time taken for UpdateMileagePlusCSSToken OnPrem service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                //var rbody = Convert.ToDateTime(localTime).ToString("yyyy-MM-ddTHH:mm:ss");
                var path = string.Format("/MileagePlusCSSToken/UpdateMileagePlusCSSToken?transactionId={0}&sessionId={1}&mpNumber={2}&deviceID={3}" +
                    "&appID={4}&appVersion={5}&authToken={6}&isAuthTokenValid={7}&authTokenExpirationDateTime={8}" +
                    "&tokenExpireInSeconds={9}&isTokenAnonymous={10}&customerID={11}",transactionId, sessionId, mpNumber, deviceID, appID, appVersion, authToken, isAuthTokenValid, authTokenExpirationDateTime, tokenExpireInSeconds, isTokenAnonymous, customerID);

                _logger.LogInformation("UpdateMileagePlusCSSToken-OnPrem Service {Path} and {SessionID} ", path,sessionId);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("UpdateMileagePlusCSSToken-OnPrem Service {requestUrl} error {statusCode} for {SessionId}", responseData.url, responseData.statusCode, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("UpdateMileagePlusCSSToken-OnPrem Service {requestUrl} {response}  and {SessionId}", responseData.url, (responseData. response), sessionId);

                return Convert.ToBoolean(responseData.response);
            }
        }
    }
}
