using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.UnitedClub
{
    public class UnitedClubMembershipService : IUnitedClubMembershipService
    {

        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<UnitedClubMembershipService> _logger;
        private readonly IConfiguration _configuration;

        public UnitedClubMembershipService([KeyFilter("UnitedClubMembershipKey")] IResilientClient resilientClient, ICacheLog<UnitedClubMembershipService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

    

        public async Task<string> GetCurrentMembershipInfo(string mPNumber)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                         {"Accept", "application/json"}
                     };
            var path = string.Format("{0}/uclub/history", mPNumber);
            _logger.LogInformation("United ClubPasses CSL Service- GetCurrentMembershipInfo MPNumber {requestPayload} and {transactionId}", mPNumber);


            using (_logger.BeginTimedOperation("Total time taken for GetCurrentMembershipInfo service call", transationId: string.Empty))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("United ClubPasses CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, string.Empty);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception();
                }

                _logger.LogInformation("United ClubPasses CSL Service {requestUrl} , {@response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }

        public async Task<string> GetCurrentMembershipInfoV2(string mPNumber, string Token)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                         {"Accept", "application/json"},
                         { "Authorization", Token }
                     };
            var path = string.Format("{0}/uclub/history", mPNumber);
            _logger.LogInformation("United ClubPasses CSL Service- GetCurrentMembershipInfo v2 MPNumber {requestPayload} and {transactionId}", mPNumber, Token);


            using (_logger.BeginTimedOperation("Total time taken for GetCurrentMembershipInfo v2 service call", transationId: string.Empty))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("United ClubPasses CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, string.Empty);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception();
                }

                _logger.LogInformation("United ClubPasses CSL Service {requestUrl} , {@response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}
