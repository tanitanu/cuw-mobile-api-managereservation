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

namespace United.Mobile.DataAccess.MemberSignIn
{
    public class MembershipHistoryService : IMembershipHistoryService
    {
        private readonly ICacheLog<MembershipHistoryService> _logger;
        private readonly IResilientClient _resilientClient;

        public MembershipHistoryService(ICacheLog<MembershipHistoryService> logger, [KeyFilter("MembershipHistoryClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<T> GetMembershipHistory<T>(string token, string mpNumber, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetMembershipHistory service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var memberShipHistoryData = await _resilientClient.GetHttpAsyncWithOptions( mpNumber, headers);

                if (memberShipHistoryData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-GetMembershipHistory-service {requestUrl} error {response} for {sessionId}", memberShipHistoryData.url, memberShipHistoryData.statusCode, sessionId);
                    if (memberShipHistoryData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(memberShipHistoryData.response);
                }

                _logger.LogInformation("AccountManagement-GetMembershipHistory-service {requestUrl} and {sessionId}", memberShipHistoryData.url, sessionId);

                return JsonConvert.DeserializeObject<T>(memberShipHistoryData.response);
            }
        }
    }
}
