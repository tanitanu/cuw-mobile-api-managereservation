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
    public class UnitedClubMembershipV2Service : IUnitedClubMembershipV2Service
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<UnitedClubMembershipV2Service> _logger;
        private readonly IConfiguration _configuration;
        public UnitedClubMembershipV2Service([KeyFilter("UnitedClubMembershipV2ClientKey")] IResilientClient resilientClient, ICacheLog<UnitedClubMembershipV2Service> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetCurrentMembershipInfo(string mpNumber,string transactionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
            var path = string.Format("{0}", mpNumber);
            _logger.LogInformation("United ClubPasses CSL Service- GetCurrentMembershipInfo MPNumber {requestPayload} and {transactionId}", mpNumber, transactionId);
            using (_logger.BeginTimedOperation("Total time taken for GetCurrentMembershipInfo call", transationId: string.Empty))
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
