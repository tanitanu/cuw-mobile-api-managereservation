using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Club
{
    public class UnitedAndStarLoungesService : IUnitedAndStarLoungesService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<UnitedAndStarLoungesService> _logger;

        public UnitedAndStarLoungesService([KeyFilter("UnitedAndStarLoungesServiceKey")] IResilientClient resilientClient, ICacheLog<UnitedAndStarLoungesService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetUnitedAndStarLoungesByAirport(string token, string airportCode, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
            string requestData = string.Format("?airport={0}", airportCode);
            _logger.LogInformation("GetUnitedAndStarLoungesByAirport request payload {airportCode} {sessionId} ", airportCode, sessionId);
            using (_logger.BeginTimedOperation("Total time taken for GetUnitedAndStarLoungesByAirport business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("ClubPasses CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("ClubPasses CSL Service {requestUrl} and {sessionId}", responseData.url, sessionId);
                
                return responseData.response;
            }
        }
    }
}
