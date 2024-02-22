using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Profile
{
    public class ClubMembershipService: IClubMembershipService
    {
        private readonly ICacheLog<ClubMembershipService> _logger;
        private readonly IResilientClient _resilientClient;

        public ClubMembershipService(ICacheLog<ClubMembershipService> logger, [KeyFilter("ClubMembershipClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<T> GetClubMembership<T>(string token, string requestData, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetClubMembership service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                var glbData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers);

                if (glbData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetClubMembership-service {requestUrl} error {response} for {sessionId}", glbData.url, glbData.statusCode, sessionId);
                    if (glbData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(glbData.response);
                }

                _logger.LogInformation("AccountManagement-GetClubMembership-service {requestUrl} and {sessionId}", glbData.url, sessionId);

                return JsonConvert.DeserializeObject<T>( glbData.response);
            }
        }
    }
}
